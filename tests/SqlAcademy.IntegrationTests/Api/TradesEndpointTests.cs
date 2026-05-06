using System.Net.Http.Json;
using SqlAcademy.IntegrationTests.Testing;
using SqlAcademy.Persistence.Commands.Trades;
using SqlAcademy.Persistence.Queries.Trades;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.IntegrationTests.Api;

[Collection(IntegrationTestCollection.Name)]
public sealed class TradesEndpointTests(SqlServerFixture databaseFixture) : IAsyncLifetime
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
    public async Task Get_trades_returns_seeded_page_in_deterministic_order()
    {
        var response = await _client.GetAsync("/api/v1/trades?pageNumber=1&pageSize=2&sortBy=tradedUtc&sortDirection=Desc");
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResult<TradeListItem>>();

        Assert.NotNull(payload);
        Assert.Equal(2, payload.Items.Count);
        Assert.True(payload.TotalCount >= 4);
        Assert.Equal("CL", payload.Items[0].InstrumentSymbol);
        Assert.Equal("EURUSD", payload.Items[1].InstrumentSymbol);
    }

    [Fact]
    public async Task Get_trades_filters_by_instrument_symbol()
    {
        var response = await _client.GetAsync("/api/v1/trades?pageNumber=1&pageSize=5&instrumentSymbol=MSFT");
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResult<TradeListItem>>();

        Assert.NotNull(payload);
        Assert.Single(payload.Items);
        Assert.All(payload.Items, item => Assert.Equal("MSFT", item.InstrumentSymbol));
    }

    [Fact]
    public async Task Get_trades_sorts_by_user_name()
    {
        var response = await _client.GetAsync("/api/v1/trades?pageNumber=1&pageSize=4&sortBy=userName&sortDirection=Asc");
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResult<TradeListItem>>();

        Assert.NotNull(payload);
        Assert.Equal(4, payload.Items.Count);
        Assert.Equal(["ada", "grace", "linus", "margaret"], payload.Items.Select(item => item.UserName).ToArray());
    }

    [Fact]
    public async Task Post_trade_import_imports_unique_rows_and_reports_rejections()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/v1/trades/import",
            new
            {
                trades = new object[]
                {
                    new
                    {
                        userName = "ada",
                        instrumentSymbol = "MSFT",
                        side = "Buy",
                        quantity = 12.5000m,
                        price = 430.1000m,
                        tradedUtc = "2025-02-07T09:00:00Z",
                    },
                    new
                    {
                        userName = "ada",
                        instrumentSymbol = "MSFT",
                        side = "Buy",
                        quantity = 12.5000m,
                        price = 430.1000m,
                        tradedUtc = "2025-02-07T09:00:00Z",
                    },
                    new
                    {
                        userName = "unknown-user",
                        instrumentSymbol = "MSFT",
                        side = "Buy",
                        quantity = 5.0000m,
                        price = 420.1000m,
                        tradedUtc = "2025-02-07T09:01:00Z",
                    },
                    new
                    {
                        userName = "grace",
                        instrumentSymbol = "AAPL",
                        side = "Sell",
                        quantity = 3.0000m,
                        price = 191.3000m,
                        tradedUtc = "2025-02-07T09:05:00Z",
                    },
                },
            });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<TradeImportResult>();

        Assert.NotNull(payload);
        Assert.Equal(4, payload.SubmittedCount);
        Assert.Equal(3, payload.ValidatedCount);
        Assert.Equal(1, payload.DuplicateCount);
        Assert.Equal(2, payload.ImportedCount);
        Assert.Equal(2, payload.RejectedCount);
        Assert.Equal(["Duplicate batch row.", "Unknown user."], payload.Rejections.Select(rejection => rejection.Reason).ToArray());

        await using var verificationContext = databaseFixture.CreateDbContext();
        var importedTradeCount = verificationContext.Trades.Count(trade =>
            trade.TradedUtc >= new DateTime(2025, 2, 7, 9, 0, 0, DateTimeKind.Utc)
            && trade.TradedUtc <= new DateTime(2025, 2, 7, 9, 5, 0, DateTimeKind.Utc));

        Assert.Equal(2, importedTradeCount);
    }

    [Fact]
    public async Task Post_trade_import_skips_existing_trade_duplicates()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/v1/trades/import",
            new
            {
                trades = new object[]
                {
                    new
                    {
                        userName = "ada",
                        instrumentSymbol = "MSFT",
                        side = "Buy",
                        quantity = 100.0000m,
                        price = 420.5000m,
                        tradedUtc = "2025-01-20T08:45:00Z",
                    },
                    new
                    {
                        userName = "linus",
                        instrumentSymbol = "EURUSD",
                        side = "Sell",
                        quantity = 10.0000m,
                        price = 1.1000m,
                        tradedUtc = "2025-02-07T09:15:00Z",
                    },
                },
            });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<TradeImportResult>();

        Assert.NotNull(payload);
        Assert.Equal(2, payload.SubmittedCount);
        Assert.Equal(2, payload.ValidatedCount);
        Assert.Equal(1, payload.DuplicateCount);
        Assert.Equal(1, payload.ImportedCount);
        Assert.Equal(1, payload.RejectedCount);
        Assert.Equal("Duplicate existing trade.", Assert.Single(payload.Rejections).Reason);
    }
}