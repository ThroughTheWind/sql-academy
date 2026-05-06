namespace SqlAcademy.SharedKernel.Pagination;

public abstract record PagedRequest
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 25;
    private const int MaxPageSize = 100;

    public int PageNumber { get; init; } = DefaultPageNumber;

    public int PageSize { get; init; } = DefaultPageSize;

    public int NormalizedPageNumber => Math.Max(PageNumber, DefaultPageNumber);

    public int NormalizedPageSize => Math.Clamp(PageSize, 1, MaxPageSize);

    public int Skip => (NormalizedPageNumber - 1) * NormalizedPageSize;
}