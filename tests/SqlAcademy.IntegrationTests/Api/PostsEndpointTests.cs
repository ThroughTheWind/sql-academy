using System.Net.Http.Json;
using SqlAcademy.IntegrationTests.Testing;
using SqlAcademy.Persistence.Queries.Posts;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.IntegrationTests.Api;

[Collection(IntegrationTestCollection.Name)]
public sealed class PostsEndpointTests(SqlServerFixture databaseFixture) : IAsyncLifetime
{
    private readonly SqlAcademyApiFactory _factory = new(databaseFixture);
    private HttpClient _client = null!;

    public async Task InitializeAsync()
    {
        await databaseFixture.ResetAsync();
        _client = _factory.CreateClient();
    }

    public Task DisposeAsync()
    {
        _client.Dispose();
        _factory.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Get_posts_returns_seeded_page()
    {
        var response = await _client.GetAsync("/api/v1/posts?pageNumber=1&pageSize=2&sortBy=createdUtc&sortDirection=Desc");
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResult<PostListItem>>();

        Assert.NotNull(payload);
        Assert.Equal(2, payload.Items.Count);
        Assert.True(payload.TotalCount >= 4);
    }
}