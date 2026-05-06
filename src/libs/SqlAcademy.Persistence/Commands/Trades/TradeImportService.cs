using Microsoft.EntityFrameworkCore;
using SqlAcademy.Domain.Entities;
using SqlAcademy.Domain.Enums;
using SqlAcademy.Persistence.Database;

namespace SqlAcademy.Persistence.Commands.Trades;

public sealed record ImportTradesCommand(IReadOnlyList<TradeImportRow> Trades);

public sealed record TradeImportRow(
    string UserName,
    string InstrumentSymbol,
    string Side,
    decimal Quantity,
    decimal Price,
    DateTime TradedUtc);

public sealed record TradeImportResult(
    int SubmittedCount,
    int ValidatedCount,
    int DuplicateCount,
    int ImportedCount,
    int RejectedCount,
    IReadOnlyList<ImportedTradeRecord> ImportedTrades,
    IReadOnlyList<RejectedTradeRecord> Rejections);

public sealed record ImportedTradeRecord(
    int Id,
    string UserName,
    string InstrumentSymbol,
    string Side,
    decimal Quantity,
    decimal Price,
    DateTime TradedUtc);

public sealed record RejectedTradeRecord(int RowNumber, string Reason);

public sealed class TradeImportService(LearningDbContext dbContext)
{
    public async Task<TradeImportResult> ImportTradesAsync(ImportTradesCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.Trades is null || command.Trades.Count == 0)
        {
            throw new ArgumentException("Trades is required and must contain at least one row.", nameof(command));
        }

        var landedRows = command.Trades
            .Select((row, index) => new StagedTradeImportRow(
                index + 1,
                row.UserName?.Trim() ?? string.Empty,
                row.InstrumentSymbol?.Trim().ToUpperInvariant() ?? string.Empty,
                row.Side?.Trim() ?? string.Empty,
                row.Quantity,
                row.Price,
                NormalizeTradedUtc(row.TradedUtc)))
            .ToArray();

        var rejections = new List<RejectedTradeRecord>();
        var validatedRows = await ValidateRowsAsync(landedRows, rejections, cancellationToken);

        var deduplicatedRows = new List<ValidatedTradeImportRow>();
        var batchKeys = new HashSet<TradeImportBusinessKey>();
        var duplicateCount = 0;

        foreach (var validatedRow in validatedRows)
        {
            if (!batchKeys.Add(validatedRow.BusinessKey))
            {
                duplicateCount++;
                rejections.Add(new RejectedTradeRecord(validatedRow.RowNumber, "Duplicate batch row."));
                continue;
            }

            deduplicatedRows.Add(validatedRow);
        }

        var existingKeys = await LoadExistingKeysAsync(deduplicatedRows, cancellationToken);
        var tradesToPublish = new List<(ValidatedTradeImportRow Row, Trade Entity)>();

        foreach (var deduplicatedRow in deduplicatedRows)
        {
            if (existingKeys.Contains(deduplicatedRow.BusinessKey))
            {
                duplicateCount++;
                rejections.Add(new RejectedTradeRecord(deduplicatedRow.RowNumber, "Duplicate existing trade."));
                continue;
            }

            tradesToPublish.Add((
                deduplicatedRow,
                new Trade
                {
                    UserId = deduplicatedRow.UserId,
                    InstrumentId = deduplicatedRow.InstrumentId,
                    Side = deduplicatedRow.Side,
                    Quantity = deduplicatedRow.Quantity,
                    Price = deduplicatedRow.Price,
                    TradedUtc = deduplicatedRow.TradedUtc,
                }));
        }

        if (tradesToPublish.Count > 0)
        {
            await dbContext.Trades.AddRangeAsync(tradesToPublish.Select(item => item.Entity), cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var importedTrades = tradesToPublish
            .Select(item => new ImportedTradeRecord(
                item.Entity.Id,
                item.Row.UserName,
                item.Row.InstrumentSymbol,
                item.Row.Side.ToString(),
                item.Row.Quantity,
                item.Row.Price,
                item.Row.TradedUtc))
            .ToArray();

        return new TradeImportResult(
            landedRows.Length,
            validatedRows.Count,
            duplicateCount,
            importedTrades.Length,
            rejections.Count,
            importedTrades,
            rejections.OrderBy(rejection => rejection.RowNumber).ToArray());
    }

    private async Task<List<ValidatedTradeImportRow>> ValidateRowsAsync(
        IReadOnlyList<StagedTradeImportRow> landedRows,
        ICollection<RejectedTradeRecord> rejections,
        CancellationToken cancellationToken)
    {
        var userNames = landedRows
            .Select(row => row.UserName)
            .Where(userName => !string.IsNullOrWhiteSpace(userName))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var instrumentSymbols = landedRows
            .Select(row => row.InstrumentSymbol)
            .Where(symbol => !string.IsNullOrWhiteSpace(symbol))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var users = (await dbContext.Users
                .Where(user => userNames.Contains(user.UserName))
                .Select(user => new { user.Id, user.UserName })
                .ToListAsync(cancellationToken))
            .ToDictionary(user => user.UserName, StringComparer.OrdinalIgnoreCase);

        var instruments = (await dbContext.Instruments
                .Where(instrument => instrumentSymbols.Contains(instrument.Symbol))
                .Select(instrument => new { instrument.Id, instrument.Symbol })
                .ToListAsync(cancellationToken))
            .ToDictionary(instrument => instrument.Symbol, StringComparer.OrdinalIgnoreCase);

        var validatedRows = new List<ValidatedTradeImportRow>();

        foreach (var landedRow in landedRows)
        {
            if (string.IsNullOrWhiteSpace(landedRow.UserName))
            {
                rejections.Add(new RejectedTradeRecord(landedRow.RowNumber, "UserName is required."));
                continue;
            }

            if (!users.TryGetValue(landedRow.UserName, out var user))
            {
                rejections.Add(new RejectedTradeRecord(landedRow.RowNumber, "Unknown user."));
                continue;
            }

            if (string.IsNullOrWhiteSpace(landedRow.InstrumentSymbol))
            {
                rejections.Add(new RejectedTradeRecord(landedRow.RowNumber, "InstrumentSymbol is required."));
                continue;
            }

            if (!instruments.TryGetValue(landedRow.InstrumentSymbol, out var instrument))
            {
                rejections.Add(new RejectedTradeRecord(landedRow.RowNumber, "Unknown instrument."));
                continue;
            }

            if (string.IsNullOrWhiteSpace(landedRow.Side))
            {
                rejections.Add(new RejectedTradeRecord(landedRow.RowNumber, "Side is required."));
                continue;
            }

            if (!Enum.TryParse<TradeSide>(landedRow.Side, true, out var side))
            {
                rejections.Add(new RejectedTradeRecord(landedRow.RowNumber, "Side must be Buy or Sell."));
                continue;
            }

            if (landedRow.Quantity <= 0)
            {
                rejections.Add(new RejectedTradeRecord(landedRow.RowNumber, "Quantity must be greater than zero."));
                continue;
            }

            if (landedRow.Price <= 0)
            {
                rejections.Add(new RejectedTradeRecord(landedRow.RowNumber, "Price must be greater than zero."));
                continue;
            }

            validatedRows.Add(new ValidatedTradeImportRow(
                landedRow.RowNumber,
                user.Id,
                user.UserName,
                instrument.Id,
                instrument.Symbol,
                side,
                landedRow.Quantity,
                landedRow.Price,
                landedRow.TradedUtc));
        }

        return validatedRows;
    }

    private async Task<HashSet<TradeImportBusinessKey>> LoadExistingKeysAsync(
        IReadOnlyList<ValidatedTradeImportRow> deduplicatedRows,
        CancellationToken cancellationToken)
    {
        if (deduplicatedRows.Count == 0)
        {
            return [];
        }

        var userIds = deduplicatedRows.Select(row => row.UserId).Distinct().ToArray();
        var instrumentIds = deduplicatedRows.Select(row => row.InstrumentId).Distinct().ToArray();
        var tradedUtcValues = deduplicatedRows.Select(row => row.TradedUtc).Distinct().ToArray();

        var existingTrades = await dbContext.Trades
            .Where(trade => userIds.Contains(trade.UserId)
                && instrumentIds.Contains(trade.InstrumentId)
                && tradedUtcValues.Contains(trade.TradedUtc))
            .Select(trade => new
            {
                trade.UserId,
                trade.InstrumentId,
                trade.Side,
                trade.Quantity,
                trade.Price,
                trade.TradedUtc,
            })
            .ToListAsync(cancellationToken);

        return existingTrades
            .Select(trade => new TradeImportBusinessKey(
                trade.UserId,
                trade.InstrumentId,
                trade.Side,
                trade.Quantity,
                trade.Price,
                trade.TradedUtc))
            .ToHashSet();
    }

    private static DateTime NormalizeTradedUtc(DateTime tradedUtc)
    {
        var normalizedKind = tradedUtc.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(tradedUtc, DateTimeKind.Utc)
            : tradedUtc;

        var truncatedTicks = normalizedKind.Ticks - (normalizedKind.Ticks % TimeSpan.TicksPerMillisecond);
        return new DateTime(truncatedTicks, normalizedKind.Kind);
    }

    private sealed record StagedTradeImportRow(
        int RowNumber,
        string UserName,
        string InstrumentSymbol,
        string Side,
        decimal Quantity,
        decimal Price,
        DateTime TradedUtc);

    private sealed record ValidatedTradeImportRow(
        int RowNumber,
        int UserId,
        string UserName,
        int InstrumentId,
        string InstrumentSymbol,
        TradeSide Side,
        decimal Quantity,
        decimal Price,
        DateTime TradedUtc)
    {
        public TradeImportBusinessKey BusinessKey => new(UserId, InstrumentId, Side, Quantity, Price, TradedUtc);
    }

    private readonly record struct TradeImportBusinessKey(
        int UserId,
        int InstrumentId,
        TradeSide Side,
        decimal Quantity,
        decimal Price,
        DateTime TradedUtc);
}