using Dapper;
using Microsoft.EntityFrameworkCore;
using SqlAcademy.PerformanceTests.Testing;
using SqlAcademy.Persistence.Infrastructure;
using SqlAcademy.Persistence.MultiTenancy;

namespace SqlAcademy.PerformanceTests;

[Collection(PerformanceTestCollection.Name)]
public sealed class RowLevelSecuritySampleTests(SqlServerFixture databaseFixture) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await databaseFixture.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Ef_core_context_with_tenant_session_context_only_sees_matching_orders()
    {
        await using var tenant101Context = await databaseFixture.CreateTenantScopedDbContextAsync(101, trackingEnabled: false);
        var tenant101Orders = await tenant101Context.TenantOrders
            .OrderBy(order => order.OrderNumber)
            .Select(order => order.OrderNumber)
            .ToListAsync();

        await using var tenant202Context = await databaseFixture.CreateTenantScopedDbContextAsync(202, trackingEnabled: false);
        var tenant202Orders = await tenant202Context.TenantOrders
            .OrderBy(order => order.OrderNumber)
            .Select(order => order.OrderNumber)
            .ToListAsync();

        Assert.Equal(["TEN-101-0001", "TEN-101-0002"], tenant101Orders);
        Assert.Equal(["TEN-202-0001", "TEN-202-0002"], tenant202Orders);
    }

    [Fact]
    public async Task Sql_connection_factory_applies_session_context_before_dapper_queries()
    {
        var sessionContextAccessor = new SqlSessionContextAccessor { TenantId = 101 };
        var sessionContextApplier = new SqlSessionContextApplier(sessionContextAccessor);
        var connectionFactory = new SqlConnectionFactory(databaseFixture.ConnectionString, sessionContextApplier);

        await using var tenantConnection = await connectionFactory.OpenConnectionAsync(CancellationToken.None);
        var tenantOrderNumbers = (await tenantConnection.QueryAsync<string>("SELECT OrderNumber FROM academy.TenantOrders ORDER BY OrderNumber;"))
            .ToArray();

        sessionContextAccessor.TenantId = null;

        await using var anonymousConnection = await connectionFactory.OpenConnectionAsync(CancellationToken.None);
        var anonymousCount = await anonymousConnection.ExecuteScalarAsync<int>("SELECT COUNT_BIG(1) FROM academy.TenantOrders;");

        Assert.Equal(["TEN-101-0001", "TEN-101-0002"], tenantOrderNumbers);
        Assert.Equal(0, anonymousCount);
    }

    [Fact]
    public async Task Ef_core_write_path_persists_row_when_tenant_context_matches_inserted_tenant()
    {
        await using var tenantContext = await databaseFixture.CreateTenantScopedDbContextAsync(101);
        tenantContext.TenantOrders.Add(new()
        {
            TenantId = 101,
            OrderNumber = "TEN-101-0100",
            Description = "Northwind EF write sample",
            TotalAmount = 640.00m,
            CreatedUtc = DateTime.UtcNow,
        });

        await tenantContext.SaveChangesAsync();

        await using var verificationContext = await databaseFixture.CreateTenantScopedDbContextAsync(101, trackingEnabled: false);
        var orderNumbers = await verificationContext.TenantOrders
            .OrderBy(order => order.OrderNumber)
            .Select(order => order.OrderNumber)
            .ToListAsync();

        Assert.Contains("TEN-101-0100", orderNumbers);
    }

    [Fact]
    public async Task Ef_core_write_path_throws_when_tenant_context_does_not_match_inserted_tenant()
    {
        await using var tenantContext = await databaseFixture.CreateTenantScopedDbContextAsync(101);
        tenantContext.TenantOrders.Add(new()
        {
            TenantId = 202,
            OrderNumber = "TEN-202-0100",
            Description = "Cross-tenant EF write sample",
            TotalAmount = 910.00m,
            CreatedUtc = DateTime.UtcNow,
        });

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => tenantContext.SaveChangesAsync());

        Assert.Contains("block predicate", exception.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}