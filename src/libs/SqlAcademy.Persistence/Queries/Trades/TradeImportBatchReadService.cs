using Dapper;
using SqlAcademy.Persistence.Commands.Trades;
using SqlAcademy.Persistence.Diagnostics;
using SqlAcademy.Persistence.Infrastructure;

namespace SqlAcademy.Persistence.Queries.Trades;

public sealed record TradeImportBatchListItem(
    int BatchId,
    DateTime ProcessedUtc,
    bool DryRun,
    int SubmittedCount,
    int ValidatedCount,
    int DuplicateCount,
    int ReadyToPublishCount,
    int ImportedCount,
    int RejectedCount);

public sealed record TradeImportBatchDetails(
    int BatchId,
    DateTime ProcessedUtc,
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

public sealed class TradeImportBatchReadService(ISqlConnectionFactory connectionFactory)
{
    public async Task<IReadOnlyList<TradeImportBatchListItem>> GetRecentBatchesAsync(int top, CancellationToken cancellationToken)
    {
        using var activity = PersistenceDiagnostics.ActivitySource.StartActivity("dapper.trade_import_batches.list");

        var normalizedTop = Math.Clamp(top, 1, 50);
        activity?.SetTag("trade_import_batches.top", normalizedTop);

        const string sql = """
SELECT TOP (@Top)
    b.Id,
    b.ProcessedUtc,
    b.DryRun,
    b.SubmittedCount,
    b.ValidatedCount,
    b.DuplicateCount,
    b.ReadyToPublishCount,
    b.ImportedCount,
    b.RejectedCount
FROM academy.TradeImportBatches AS b
ORDER BY b.ProcessedUtc DESC, b.Id DESC;
""";

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = (await connection.QueryAsync<TradeImportBatchListRow>(
            new CommandDefinition(sql, new { Top = normalizedTop }, cancellationToken: cancellationToken))).AsList();

        return rows
            .Select(row => new TradeImportBatchListItem(
                row.Id,
                row.ProcessedUtc,
                row.DryRun,
                row.SubmittedCount,
                row.ValidatedCount,
                row.DuplicateCount,
                row.ReadyToPublishCount,
                row.ImportedCount,
                row.RejectedCount))
            .ToArray();
    }

    public async Task<TradeImportBatchDetails?> GetBatchAsync(int batchId, CancellationToken cancellationToken)
    {
        using var activity = PersistenceDiagnostics.ActivitySource.StartActivity("dapper.trade_import_batches.detail");
        activity?.SetTag("trade_import_batches.batch_id", batchId);

        const string sql = """
SELECT
    b.Id AS BatchId,
    b.ProcessedUtc,
    b.DryRun,
    b.SubmittedCount,
    b.ValidatedCount,
    b.DuplicateCount,
    b.ReadyToPublishCount,
    b.ImportedCount,
    b.RejectedCount
FROM academy.TradeImportBatches AS b
WHERE b.Id = @BatchId;

SELECT
    r.RowNumber,
    r.Outcome,
    r.Stage,
    r.Code,
    r.Reason,
    r.TradeId,
    r.UserName,
    r.InstrumentSymbol,
    r.Side,
    r.Quantity,
    r.Price,
    r.TradedUtc
FROM academy.TradeImportBatchRows AS r
WHERE r.TradeImportBatchId = @BatchId
ORDER BY r.RowNumber ASC, r.Id ASC;
""";

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        using var reader = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, new { BatchId = batchId }, cancellationToken: cancellationToken));

        var batch = await reader.ReadSingleOrDefaultAsync<TradeImportBatchHeader>();

        if (batch is null)
        {
            return null;
        }

        var rows = (await reader.ReadAsync<TradeImportBatchRowRecord>()).AsList();

        var readyToPublishTrades = rows
            .Where(row => string.Equals(row.Outcome, "ReadyToPublish", StringComparison.Ordinal))
            .Select(row => new TradeImportPreviewRecord(
                row.Stage,
                row.UserName,
                row.InstrumentSymbol,
                row.Side,
                row.Quantity,
                row.Price,
                row.TradedUtc))
            .ToArray();

        var importedTrades = rows
            .Where(row => string.Equals(row.Outcome, "Imported", StringComparison.Ordinal) && row.TradeId.HasValue)
            .Select(row => new ImportedTradeRecord(
                row.TradeId.GetValueOrDefault(),
                row.UserName,
                row.InstrumentSymbol,
                row.Side,
                row.Quantity,
                row.Price,
                row.TradedUtc))
            .ToArray();

        var rejections = rows
            .Where(row => string.Equals(row.Outcome, "Rejected", StringComparison.Ordinal))
            .Select(row => new RejectedTradeRecord(
                row.RowNumber,
                row.Stage,
                row.Code ?? string.Empty,
                row.Reason ?? string.Empty))
            .ToArray();

        return new TradeImportBatchDetails(
            batch.BatchId,
            batch.ProcessedUtc,
            batch.DryRun,
            batch.SubmittedCount,
            batch.ValidatedCount,
            batch.DuplicateCount,
            batch.ReadyToPublishCount,
            batch.ImportedCount,
            batch.RejectedCount,
            readyToPublishTrades,
            importedTrades,
            rejections);
    }

    private sealed class TradeImportBatchListRow
    {
        public int Id { get; set; }

        public DateTime ProcessedUtc { get; set; }

        public bool DryRun { get; set; }

        public int SubmittedCount { get; set; }

        public int ValidatedCount { get; set; }

        public int DuplicateCount { get; set; }

        public int ReadyToPublishCount { get; set; }

        public int ImportedCount { get; set; }

        public int RejectedCount { get; set; }
    }

    private sealed class TradeImportBatchHeader
    {
        public int BatchId { get; set; }

        public DateTime ProcessedUtc { get; set; }

        public bool DryRun { get; set; }

        public int SubmittedCount { get; set; }

        public int ValidatedCount { get; set; }

        public int DuplicateCount { get; set; }

        public int ReadyToPublishCount { get; set; }

        public int ImportedCount { get; set; }

        public int RejectedCount { get; set; }
    }

    private sealed class TradeImportBatchRowRecord
    {
        public int RowNumber { get; set; }

        public string Outcome { get; set; } = string.Empty;

        public string Stage { get; set; } = string.Empty;

        public string? Code { get; set; }

        public string? Reason { get; set; }

        public int? TradeId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string InstrumentSymbol { get; set; } = string.Empty;

        public string Side { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime TradedUtc { get; set; }
    }
}