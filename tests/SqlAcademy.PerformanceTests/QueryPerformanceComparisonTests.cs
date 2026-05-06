using System.Diagnostics;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SqlAcademy.PerformanceTests.Testing;
using Xunit.Abstractions;

namespace SqlAcademy.PerformanceTests;

[Collection(PerformanceTestCollection.Name)]
public sealed class QueryPerformanceComparisonTests(SqlServerFixture databaseFixture, ITestOutputHelper testOutputHelper) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await databaseFixture.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Ef_core_and_dapper_return_the_same_trade_page()
    {
        await using var dbContext = databaseFixture.CreateDbContext(trackingEnabled: false);
        var efStopwatch = Stopwatch.StartNew();
        var efTradeIds = await dbContext.Trades
            .AsNoTracking()
            .OrderByDescending(trade => trade.TradedUtc)
            .Select(trade => trade.Id)
            .Take(3)
            .ToListAsync();
        efStopwatch.Stop();

        await using var connection = new SqlConnection(databaseFixture.ConnectionString);
        await connection.OpenAsync();

        var dapperStopwatch = Stopwatch.StartNew();
        var dapperTradeIds = (await connection.QueryAsync<int>("""
            SELECT TOP (3) Id
            FROM academy.Trades
            ORDER BY TradedUtc DESC, Id DESC;
            """))
            .ToList();
        dapperStopwatch.Stop();

        Assert.Equal(efTradeIds, dapperTradeIds);
        testOutputHelper.WriteLine($"EF Core: {efStopwatch.ElapsedMilliseconds} ms | Dapper: {dapperStopwatch.ElapsedMilliseconds} ms");
    }
}