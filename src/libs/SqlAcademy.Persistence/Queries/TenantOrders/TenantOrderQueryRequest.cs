using SqlAcademy.SharedKernel.Pagination;
using SqlAcademy.SharedKernel.Sorting;

namespace SqlAcademy.Persistence.Queries.TenantOrders;

public sealed record TenantOrderQueryRequest : PagedRequest
{
    public string SortBy { get; init; } = "createdUtc";

    public SortDirection SortDirection { get; init; } = SortDirection.Desc;
}