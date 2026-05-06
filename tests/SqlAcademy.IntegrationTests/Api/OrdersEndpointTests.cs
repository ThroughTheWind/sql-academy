using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlAcademy.Domain.Enums;
using SqlAcademy.IntegrationTests.Testing;
using SqlAcademy.Persistence.Commands.Orders;

namespace SqlAcademy.IntegrationTests.Api;

[Collection(IntegrationTestCollection.Name)]
public sealed class OrdersEndpointTests(SqlServerFixture databaseFixture) : IAsyncLifetime
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
    public async Task Put_order_status_updates_order_when_rowversion_matches()
    {
        await using var dbContext = databaseFixture.CreateDbContext();
        var order = await dbContext.Orders.SingleAsync(existingOrder => existingOrder.OrderNumber == "ORD-2025-0002");
        var currentRowVersionHex = $"0x{Convert.ToHexString(order.RowVersion)}";

        var response = await _client.PutAsJsonAsync(
            "/api/v1/orders/ORD-2025-0002/status",
            new
            {
                Status = "Filled",
                RowVersionHex = currentRowVersionHex,
            });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<OrderStatusUpdateResult>();

        Assert.NotNull(payload);
        Assert.Equal("ORD-2025-0002", payload.OrderNumber);
        Assert.Equal("Filled", payload.Status);
        Assert.NotEqual(currentRowVersionHex, payload.RowVersionHex);

        await using var verificationContext = databaseFixture.CreateDbContext();
        var updatedOrder = await verificationContext.Orders.SingleAsync(existingOrder => existingOrder.OrderNumber == "ORD-2025-0002");

        Assert.Equal(OrderStatus.Filled, updatedOrder.Status);
    }

    [Fact]
    public async Task Put_order_status_returns_conflict_when_rowversion_is_stale()
    {
        await using var dbContext = databaseFixture.CreateDbContext();
        var order = await dbContext.Orders.SingleAsync(existingOrder => existingOrder.OrderNumber == "ORD-2025-0002");
        var staleRowVersionHex = $"0x{Convert.ToHexString(order.RowVersion)}";

        order.Status = OrderStatus.Filled;
        order.UpdatedUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        var response = await _client.PutAsJsonAsync(
            "/api/v1/orders/ORD-2025-0002/status",
            new
            {
                Status = "Cancelled",
                RowVersionHex = staleRowVersionHex,
            });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(payload);
        Assert.Equal(409, payload.Status);
        Assert.Equal("Concurrency conflict", payload.Title);
    }
}