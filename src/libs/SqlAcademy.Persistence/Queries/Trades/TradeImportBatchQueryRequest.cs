using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Persistence.Queries.Trades;

public sealed record TradeImportBatchQueryRequest : PagedRequest
{
    public bool? DryRun { get; init; }
}