using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using SqlAcademy.IntegrationTests.Testing;
using SqlAcademy.Persistence.Commands.TenantOrders;
using SqlAcademy.Persistence.MultiTenancy;
using SqlAcademy.Persistence.Queries.TenantOrders;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.IntegrationTests.Api;

[Collection(IntegrationTestCollection.Name)]
public sealed class TenantOrdersEndpointTests(SqlServerFixture databaseFixture) : IAsyncLifetime
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
    public async Task Get_tenant_orders_returns_only_rows_for_the_requested_tenant()
    {
        _client.DefaultRequestHeaders.Remove(TenantRequestHeaderNames.TenantId);
        _client.DefaultRequestHeaders.Add(TenantRequestHeaderNames.TenantId, "101");

        var response = await _client.GetAsync("/api/v1/tenant-orders?pageNumber=1&pageSize=10&sortBy=createdUtc&sortDirection=Desc");
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResult<TenantOrderListItem>>();

        Assert.NotNull(payload);
        Assert.Equal(2, payload.Items.Count);
        Assert.Equal(2, payload.TotalCount);
        Assert.All(payload.Items, item => Assert.Equal(101, item.TenantId));
        Assert.Contains(payload.Items, item => item.OrderNumber == "TEN-101-0001");
        Assert.DoesNotContain(payload.Items, item => item.OrderNumber == "TEN-202-0001");
    }

    [Fact]
    public async Task Get_tenant_orders_returns_empty_page_without_tenant_header()
    {
        _client.DefaultRequestHeaders.Remove(TenantRequestHeaderNames.TenantId);

        var response = await _client.GetAsync("/api/v1/tenant-orders?pageNumber=1&pageSize=10");
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResult<TenantOrderListItem>>();

        Assert.NotNull(payload);
        Assert.Empty(payload.Items);
        Assert.Equal(0, payload.TotalCount);
    }

    [Fact]
    public async Task Get_tenant_orders_rejects_non_numeric_tenant_header()
    {
        _client.DefaultRequestHeaders.Remove(TenantRequestHeaderNames.TenantId);
        _client.DefaultRequestHeaders.Add(TenantRequestHeaderNames.TenantId, "northwind");

        var response = await _client.GetAsync("/api/v1/tenant-orders?pageNumber=1&pageSize=10");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_tenant_order_creates_row_when_header_matches_body_tenant()
    {
        _client.DefaultRequestHeaders.Remove(TenantRequestHeaderNames.TenantId);
        _client.DefaultRequestHeaders.Add(TenantRequestHeaderNames.TenantId, "101");

        var response = await _client.PostAsJsonAsync(
            "/api/v1/tenant-orders",
            new
            {
                TenantId = 101,
                OrderNumber = "TEN-101-0099",
                Description = "Northwind expansion order",
                TotalAmount = 777.45m,
            });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<TenantOrderWriteResult>();

        Assert.NotNull(payload);
        Assert.Equal(101, payload.TenantId);
        Assert.Equal("TEN-101-0099", payload.OrderNumber);

        var listResponse = await _client.GetAsync("/api/v1/tenant-orders?pageNumber=1&pageSize=10&sortBy=orderNumber&sortDirection=Asc");
        listResponse.EnsureSuccessStatusCode();

        var listPayload = await listResponse.Content.ReadFromJsonAsync<PagedResult<TenantOrderListItem>>();

        Assert.NotNull(listPayload);
        Assert.Contains(listPayload.Items, item => item.OrderNumber == "TEN-101-0099");
        Assert.DoesNotContain(listPayload.Items, item => item.OrderNumber == "TEN-202-0099");
    }

    [Fact]
    public async Task Post_tenant_order_returns_forbidden_when_header_and_body_tenant_do_not_match()
    {
        _client.DefaultRequestHeaders.Remove(TenantRequestHeaderNames.TenantId);
        _client.DefaultRequestHeaders.Add(TenantRequestHeaderNames.TenantId, "101");

        var response = await _client.PostAsJsonAsync(
            "/api/v1/tenant-orders",
            new
            {
                TenantId = 202,
                OrderNumber = "TEN-202-0099",
                Description = "Cross-tenant write attempt",
                TotalAmount = 888.10m,
            });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(payload);
        Assert.Equal(403, payload.Status);
        Assert.Equal("Write blocked by security policy", payload.Title);
    }
}