using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Persistence.Queries.Trades;

public sealed record TradeImportBatchQueryRequest : PagedRequest
{
    public bool? DryRun { get; init; }

    public string? Source { get; init; }

    public string? CorrelationId { get; init; }
}