using Microsoft.EntityFrameworkCore;
using SqlAcademy.Domain.Entities;
using SqlAcademy.Domain.Enums;
using SqlAcademy.Persistence.Database;

namespace SqlAcademy.Persistence.Commands.Trades;

public sealed record ImportTradesCommand(IReadOnlyList<TradeImportRow> Trades, bool DryRun = false);

public sealed record TradeImportRow(
    string UserName,
    string InstrumentSymbol,
    string Side,
    decimal Quantity,
    decimal Price,
    DateTime TradedUtc);

public sealed record TradeImportResult(
    int BatchId,
    bool DryRun,
    int SubmittedCount,
    int ValidatedCount,
    int DuplicateCount,
    int ReadyToPublishCount,
    int ImportedCount,
    int RejectedCount,
    IReadOnlyList<TradeImportPreviewRecord> ReadyToPublishTrades,
    IReadOnlyList<ImportedTradeRecord> ImportedTrades,
    IReadOnlyList<RejectedTradeRecord> Rejections);

public sealed record TradeImportPreviewRecord(
    string Stage,
    string UserName,
    string InstrumentSymbol,
    string Side,
    decimal Quantity,
    decimal Price,
    DateTime TradedUtc);

public sealed record ImportedTradeRecord(
    int Id,
    string UserName,
    string InstrumentSymbol,
    string Side,
    decimal Quantity,
    decimal Price,
    DateTime TradedUtc);

public sealed record RejectedTradeRecord(int RowNumber, string Stage, string Code, string Reason);

public sealed class TradeImportService(LearningDbContext dbContext)
{
    public async Task<TradeImportResult> ImportTradesAsync(ImportTradesCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.Trades is null || command.Trades.Count == 0)
        {
            throw new ArgumentException("Trades is required and must contain at least one row.", nameof(command));
        }

        var processedUtc = NormalizeUtcTimestamp(DateTime.UtcNow);

        var landedRows = command.Trades
            .Select((row, index) => new StagedTradeImportRow(
                index + 1,
                row.UserName?.Trim() ?? string.Empty,
                row.InstrumentSymbol?.Trim().ToUpperInvariant() ?? string.Empty,
                row.Side?.Trim() ?? string.Empty,
                row.Quantity,
                row.Price,
                NormalizeUtcTimestamp(row.TradedUtc)))
            .ToArray();

            var landedRowsByNumber = landedRows.ToDictionary(row => row.RowNumber);

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
                rejections.Add(CreateRejection(validatedRow.RowNumber, "DeduplicateBatch", "DuplicateBatchRow", "Duplicate batch row."));
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
                rejections.Add(CreateRejection(deduplicatedRow.RowNumber, "DeduplicateExisting", "DuplicateExistingTrade", "Duplicate existing trade."));
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

        var readyToPublishTrades = tradesToPublish
            .Select(item => new TradeImportPreviewRecord(
                "ReadyToPublish",
                item.Row.UserName,
                item.Row.InstrumentSymbol,
                item.Row.Side.ToString(),
                item.Row.Quantity,
                item.Row.Price,
                item.Row.TradedUtc))
            .ToArray();

        var importedTrades = Array.Empty<ImportedTradeRecord>();
        var batch = new TradeImportBatch
        {
            ProcessedUtc = processedUtc,
            DryRun = command.DryRun,
            SubmittedCount = landedRows.Length,
            ValidatedCount = validatedRows.Count,
            DuplicateCount = duplicateCount,
            ReadyToPublishCount = readyToPublishTrades.Length,
            ImportedCount = 0,
            RejectedCount = rejections.Count,
        };

        foreach (var trade in tradesToPublish)
        {
            batch.Rows.Add(CreateReadyToPublishBatchRow(trade.Row));
        }

        foreach (var rejection in rejections.OrderBy(rejection => rejection.RowNumber))
        {
            batch.Rows.Add(CreateRejectedBatchRow(landedRowsByNumber[rejection.RowNumber], rejection));
        }

        if (!command.DryRun && tradesToPublish.Count > 0)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Trades.AddRangeAsync(tradesToPublish.Select(item => item.Entity), cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            importedTrades = tradesToPublish
                .Select(item => new ImportedTradeRecord(
                    item.Entity.Id,
                    item.Row.UserName,
                    item.Row.InstrumentSymbol,
                    item.Row.Side.ToString(),
                    item.Row.Quantity,
                    item.Row.Price,
                    item.Row.TradedUtc))
                .ToArray();

            batch.ImportedCount = importedTrades.Length;

            foreach (var trade in tradesToPublish)
            {
                batch.Rows.Add(CreateImportedBatchRow(trade.Row, trade.Entity.Id));
            }

            await dbContext.TradeImportBatches.AddAsync(batch, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        else
        {
            await dbContext.TradeImportBatches.AddAsync(batch, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return new TradeImportResult(
            batch.Id,
            command.DryRun,
            landedRows.Length,
            validatedRows.Count,
            duplicateCount,
            readyToPublishTrades.Length,
            importedTrades.Length,
            rejections.Count,
            readyToPublishTrades,
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
                rejections.Add(CreateRejection(landedRow.RowNumber, "Validate", "MissingUserName", "UserName is required."));
                continue;
            }

            if (!users.TryGetValue(landedRow.UserName, out var user))
            {
                rejections.Add(CreateRejection(landedRow.RowNumber, "Validate", "UnknownUser", "Unknown user."));
                continue;
            }

            if (string.IsNullOrWhiteSpace(landedRow.InstrumentSymbol))
            {
                rejections.Add(CreateRejection(landedRow.RowNumber, "Validate", "MissingInstrumentSymbol", "InstrumentSymbol is required."));
                continue;
            }

            if (!instruments.TryGetValue(landedRow.InstrumentSymbol, out var instrument))
            {
                rejections.Add(CreateRejection(landedRow.RowNumber, "Validate", "UnknownInstrument", "Unknown instrument."));
                continue;
            }

            if (string.IsNullOrWhiteSpace(landedRow.Side))
            {
                rejections.Add(CreateRejection(landedRow.RowNumber, "Validate", "MissingSide", "Side is required."));
                continue;
            }

            if (!Enum.TryParse<TradeSide>(landedRow.Side, true, out var side))
            {
                rejections.Add(CreateRejection(landedRow.RowNumber, "Validate", "InvalidSide", "Side must be Buy or Sell."));
                continue;
            }

            if (landedRow.Quantity <= 0)
            {
                rejections.Add(CreateRejection(landedRow.RowNumber, "Validate", "NonPositiveQuantity", "Quantity must be greater than zero."));
                continue;
            }

            if (landedRow.Price <= 0)
            {
                rejections.Add(CreateRejection(landedRow.RowNumber, "Validate", "NonPositivePrice", "Price must be greater than zero."));
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

    private static DateTime NormalizeUtcTimestamp(DateTime value)
    {
        var normalizedKind = value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
            : value;

        var truncatedTicks = normalizedKind.Ticks - (normalizedKind.Ticks % TimeSpan.TicksPerMillisecond);
        return new DateTime(truncatedTicks, normalizedKind.Kind);
    }

    private static RejectedTradeRecord CreateRejection(int rowNumber, string stage, string code, string reason)
    {
        return new RejectedTradeRecord(rowNumber, stage, code, reason);
    }

    private static TradeImportBatchRow CreateReadyToPublishBatchRow(ValidatedTradeImportRow row)
    {
        return new TradeImportBatchRow
        {
            RowNumber = row.RowNumber,
            Outcome = "ReadyToPublish",
            Stage = "ReadyToPublish",
            UserName = row.UserName,
            InstrumentSymbol = row.InstrumentSymbol,
            Side = row.Side.ToString(),
            Quantity = row.Quantity,
            Price = row.Price,
            TradedUtc = row.TradedUtc,
        };
    }

    private static TradeImportBatchRow CreateImportedBatchRow(ValidatedTradeImportRow row, int tradeId)
    {
        return new TradeImportBatchRow
        {
            RowNumber = row.RowNumber,
            Outcome = "Imported",
            Stage = "Publish",
            TradeId = tradeId,
            UserName = row.UserName,
            InstrumentSymbol = row.InstrumentSymbol,
            Side = row.Side.ToString(),
            Quantity = row.Quantity,
            Price = row.Price,
            TradedUtc = row.TradedUtc,
        };
    }

    private static TradeImportBatchRow CreateRejectedBatchRow(StagedTradeImportRow row, RejectedTradeRecord rejection)
    {
        return new TradeImportBatchRow
        {
            RowNumber = rejection.RowNumber,
            Outcome = "Rejected",
            Stage = rejection.Stage,
            Code = rejection.Code,
            Reason = rejection.Reason,
            UserName = row.UserName,
            InstrumentSymbol = row.InstrumentSymbol,
            Side = row.Side,
            Quantity = row.Quantity,
            Price = row.Price,
            TradedUtc = row.TradedUtc,
        };
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