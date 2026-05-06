using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Persistence.Queries.Trades;

public sealed record TradeImportBatchRowQueryRequest : PagedRequest
{
    public string? Outcome { get; init; }

    public string? Stage { get; init; }
}