using SqlAcademy.SharedKernel.Pagination;
using SqlAcademy.SharedKernel.Sorting;

namespace SqlAcademy.Persistence.Queries.Trades;

public sealed record TradeQueryRequest : PagedRequest
{
    public int? UserId { get; init; }

    public string? InstrumentSymbol { get; init; }

    public string SortBy { get; init; } = "tradedUtc";

    public SortDirection SortDirection { get; init; } = SortDirection.Desc;
}