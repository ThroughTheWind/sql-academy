using SqlAcademy.SharedKernel.Pagination;
using SqlAcademy.SharedKernel.Sorting;

namespace SqlAcademy.Persistence.Queries.Posts;

public sealed record PostQueryRequest : PagedRequest
{
    public int? AuthorId { get; init; }

    public string? Search { get; init; }

    public string SortBy { get; init; } = "createdUtc";

    public SortDirection SortDirection { get; init; } = SortDirection.Desc;
}