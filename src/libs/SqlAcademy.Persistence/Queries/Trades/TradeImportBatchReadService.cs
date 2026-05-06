using Dapper;
using SqlAcademy.Persistence.Commands.Trades;
using SqlAcademy.Persistence.Diagnostics;
using SqlAcademy.Persistence.Infrastructure;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Persistence.Queries.Trades;

public sealed record TradeImportBatchListItem(
    int BatchId,
    DateTime ProcessedUtc,
    string? Source,
    string CorrelationId,
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
    string? Source,
    string CorrelationId,
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

public sealed record TradeImportBatchRowListItem(
    int RowNumber,
    string Outcome,
    string Stage,
    string? Code,
    string? Reason,
    int? TradeId,
    string UserName,
    string InstrumentSymbol,
    string Side,
    decimal Quantity,
    decimal Price,
    DateTime TradedUtc);

public sealed class TradeImportBatchReadService(ISqlConnectionFactory connectionFactory)
{
    public async Task<PagedResult<TradeImportBatchListItem>> GetBatchesAsync(TradeImportBatchQueryRequest request, CancellationToken cancellationToken)
    {
        using var activity = PersistenceDiagnostics.ActivitySource.StartActivity("dapper.trade_import_batches.list");
        var source = NormalizeFilterValue(request.Source);
        var correlationId = NormalizeFilterValue(request.CorrelationId);

        activity?.SetTag("trade_import_batches.page_size", request.NormalizedPageSize);
        activity?.SetTag("trade_import_batches.page_number", request.NormalizedPageNumber);
        activity?.SetTag("trade_import_batches.dry_run", request.DryRun?.ToString() ?? "all");
        activity?.SetTag("trade_import_batches.source", source ?? "all");
        activity?.SetTag("trade_import_batches.correlation_id", correlationId ?? "all");

        const string whereClause = """
WHERE (@DryRun IS NULL OR b.DryRun = @DryRun)
      AND (@Source IS NULL OR b.Source = @Source)
      AND (@CorrelationId IS NULL OR b.CorrelationId = @CorrelationId)
""";

        var sql = $"""
SELECT
    b.Id AS BatchId,
    b.ProcessedUtc,
        b.Source,
        b.CorrelationId,
    b.DryRun,
    b.SubmittedCount,
    b.ValidatedCount,
    b.DuplicateCount,
    b.ReadyToPublishCount,
    b.ImportedCount,
    b.RejectedCount
FROM academy.TradeImportBatches AS b
{whereClause}
ORDER BY b.ProcessedUtc DESC, b.Id DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

SELECT COUNT_BIG(1)
FROM academy.TradeImportBatches AS b
{whereClause};
""";

        var parameters = new
        {
            request.DryRun,
            Source = source,
            CorrelationId = correlationId,
            Offset = request.Skip,
            PageSize = request.NormalizedPageSize,
        };

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        using var reader = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        var rows = (await reader.ReadAsync<TradeImportBatchListRow>()).AsList();
        var totalCount = await reader.ReadSingleAsync<long>();

        var items = rows
            .Select(row => new TradeImportBatchListItem(
                row.BatchId,
                row.ProcessedUtc,
                row.Source,
                row.CorrelationId,
                row.DryRun,
                row.SubmittedCount,
                row.ValidatedCount,
                row.DuplicateCount,
                row.ReadyToPublishCount,
                row.ImportedCount,
                row.RejectedCount))
            .ToArray();

        return new PagedResult<TradeImportBatchListItem>(items, request.NormalizedPageNumber, request.NormalizedPageSize, totalCount);
    }

    public async Task<TradeImportBatchDetails?> GetBatchAsync(int batchId, CancellationToken cancellationToken)
    {
        using var activity = PersistenceDiagnostics.ActivitySource.StartActivity("dapper.trade_import_batches.detail");
        activity?.SetTag("trade_import_batches.batch_id", batchId);

        const string sql = """
SELECT
    b.Id AS BatchId,
    b.ProcessedUtc,
    b.Source,
    b.CorrelationId,
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
            batch.Source,
            batch.CorrelationId,
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

    public async Task<PagedResult<TradeImportBatchRowListItem>?> GetBatchRowsAsync(
        int batchId,
        TradeImportBatchRowQueryRequest request,
        CancellationToken cancellationToken)
    {
        using var activity = PersistenceDiagnostics.ActivitySource.StartActivity("dapper.trade_import_batches.rows");
        activity?.SetTag("trade_import_batches.batch_id", batchId);
        activity?.SetTag("trade_import_batches.rows.page_size", request.NormalizedPageSize);
        activity?.SetTag("trade_import_batches.rows.page_number", request.NormalizedPageNumber);
        activity?.SetTag("trade_import_batches.rows.outcome", NormalizeFilterValue(request.Outcome) ?? "all");
        activity?.SetTag("trade_import_batches.rows.stage", NormalizeFilterValue(request.Stage) ?? "all");

        const string whereClause = """
WHERE r.TradeImportBatchId = @BatchId
  AND (@Outcome IS NULL OR r.Outcome = @Outcome)
  AND (@Stage IS NULL OR r.Stage = @Stage)
""";

        var sql = $"""
SELECT COUNT_BIG(1)
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
{whereClause}
ORDER BY r.RowNumber ASC, r.Id ASC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

SELECT COUNT_BIG(1)
FROM academy.TradeImportBatchRows AS r
{whereClause};
""";

        var parameters = new
        {
            BatchId = batchId,
            Outcome = NormalizeFilterValue(request.Outcome),
            Stage = NormalizeFilterValue(request.Stage),
            Offset = request.Skip,
            PageSize = request.NormalizedPageSize,
        };

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        using var reader = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        var batchCount = await reader.ReadSingleAsync<long>();

        if (batchCount == 0)
        {
            return null;
        }

        var rows = (await reader.ReadAsync<TradeImportBatchRowRecord>()).AsList();
        var totalCount = await reader.ReadSingleAsync<long>();

        var items = rows
            .Select(row => new TradeImportBatchRowListItem(
                row.RowNumber,
                row.Outcome,
                row.Stage,
                row.Code,
                row.Reason,
                row.TradeId,
                row.UserName,
                row.InstrumentSymbol,
                row.Side,
                row.Quantity,
                row.Price,
                row.TradedUtc))
            .ToArray();

        return new PagedResult<TradeImportBatchRowListItem>(items, request.NormalizedPageNumber, request.NormalizedPageSize, totalCount);
    }

    private static string? NormalizeFilterValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    private sealed class TradeImportBatchListRow
    {
        public int BatchId { get; set; }

        public DateTime ProcessedUtc { get; set; }

        public string? Source { get; set; }

        public string CorrelationId { get; set; } = string.Empty;

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

        public string? Source { get; set; }

        public string CorrelationId { get; set; } = string.Empty;

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