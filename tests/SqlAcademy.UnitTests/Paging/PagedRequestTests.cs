using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.UnitTests.Paging;

public sealed class PagedRequestTests
{
    [Fact]
    public void Normalized_values_are_clamped_into_supported_bounds()
    {
        var request = new TestPagedRequest
        {
            PageNumber = 0,
            PageSize = 250,
        };

        Assert.Equal(1, request.NormalizedPageNumber);
        Assert.Equal(100, request.NormalizedPageSize);
        Assert.Equal(0, request.Skip);
    }

    [Fact]
    public void Skip_is_calculated_from_normalized_page_values()
    {
        var request = new TestPagedRequest
        {
            PageNumber = 3,
            PageSize = 10,
        };

        Assert.Equal(20, request.Skip);
    }

    private sealed record TestPagedRequest : PagedRequest;
}