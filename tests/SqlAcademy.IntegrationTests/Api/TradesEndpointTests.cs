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
        Assert.True(payload.BatchId > 0);
        Assert.False(payload.DryRun);
        Assert.Equal(4, payload.SubmittedCount);
        Assert.Equal(3, payload.ValidatedCount);
        Assert.Equal(1, payload.DuplicateCount);
        Assert.Equal(2, payload.ReadyToPublishCount);
        Assert.Equal(2, payload.ImportedCount);
        Assert.Equal(2, payload.RejectedCount);
        Assert.Equal(2, payload.ReadyToPublishTrades.Count);
        Assert.All(payload.ReadyToPublishTrades, trade => Assert.Equal("ReadyToPublish", trade.Stage));
        Assert.Equal(["Duplicate batch row.", "Unknown user."], payload.Rejections.Select(rejection => rejection.Reason).ToArray());
        Assert.Equal(["DeduplicateBatch", "Validate"], payload.Rejections.Select(rejection => rejection.Stage).ToArray());
        Assert.Equal(["DuplicateBatchRow", "UnknownUser"], payload.Rejections.Select(rejection => rejection.Code).ToArray());

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
        Assert.True(payload.BatchId > 0);
        Assert.False(payload.DryRun);
        Assert.Equal(2, payload.SubmittedCount);
        Assert.Equal(2, payload.ValidatedCount);
        Assert.Equal(1, payload.DuplicateCount);
        Assert.Equal(1, payload.ReadyToPublishCount);
        Assert.Equal(1, payload.ImportedCount);
        Assert.Equal(1, payload.RejectedCount);
        var rejection = Assert.Single(payload.Rejections);
        Assert.Equal("Duplicate existing trade.", rejection.Reason);
        Assert.Equal("DeduplicateExisting", rejection.Stage);
        Assert.Equal("DuplicateExistingTrade", rejection.Code);
    }

    [Fact]
    public async Task Post_trade_import_dry_run_reports_candidates_without_persisting_rows()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/v1/trades/import",
            new
            {
                dryRun = true,
                trades = new object[]
                {
                    new
                    {
                        userName = "margaret",
                        instrumentSymbol = "CL",
                        side = "Buy",
                        quantity = 4.0000m,
                        price = 80.5000m,
                        tradedUtc = "2025-02-07T10:00:00Z",
                    },
                    new
                    {
                        userName = "margaret",
                        instrumentSymbol = "CL",
                        side = "Buy",
                        quantity = 4.0000m,
                        price = 80.5000m,
                        tradedUtc = "2025-02-07T10:00:00Z",
                    },
                },
            });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<TradeImportResult>();

        Assert.NotNull(payload);
        Assert.True(payload.BatchId > 0);
        Assert.True(payload.DryRun);
        Assert.Equal(2, payload.SubmittedCount);
        Assert.Equal(2, payload.ValidatedCount);
        Assert.Equal(1, payload.DuplicateCount);
        Assert.Equal(1, payload.ReadyToPublishCount);
        Assert.Equal(0, payload.ImportedCount);
        var preview = Assert.Single(payload.ReadyToPublishTrades);
        Assert.Equal("ReadyToPublish", preview.Stage);
        Assert.Empty(payload.ImportedTrades);
        var rejection = Assert.Single(payload.Rejections);
        Assert.Equal("Duplicate batch row.", rejection.Reason);
        Assert.Equal("DeduplicateBatch", rejection.Stage);
        Assert.Equal("DuplicateBatchRow", rejection.Code);

        await using var verificationContext = databaseFixture.CreateDbContext();
        var dryRunTradeCount = verificationContext.Trades.Count(trade =>
            trade.TradedUtc == new DateTime(2025, 2, 7, 10, 0, 0, DateTimeKind.Utc)
            && trade.Price == 80.5000m);

        Assert.Equal(0, dryRunTradeCount);
    }

    [Fact]
    public async Task Post_trade_import_persists_batch_history_that_can_be_retrieved()
    {
        var importResponse = await _client.PostAsJsonAsync(
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
                        quantity = 2.0000m,
                        price = 421.0000m,
                        tradedUtc = "2025-02-08T09:00:00Z",
                    },
                    new
                    {
                        userName = "unknown-user",
                        instrumentSymbol = "MSFT",
                        side = "Buy",
                        quantity = 1.0000m,
                        price = 420.0000m,
                        tradedUtc = "2025-02-08T09:01:00Z",
                    },
                },
            });

        importResponse.EnsureSuccessStatusCode();

        var importPayload = await importResponse.Content.ReadFromJsonAsync<TradeImportResult>();

        Assert.NotNull(importPayload);
        Assert.True(importPayload.BatchId > 0);

        var detailResponse = await _client.GetAsync($"/api/v1/trades/import-batches/{importPayload.BatchId}");
        detailResponse.EnsureSuccessStatusCode();

        var detailPayload = await detailResponse.Content.ReadFromJsonAsync<TradeImportBatchDetails>();

        Assert.NotNull(detailPayload);
        Assert.Equal(importPayload.BatchId, detailPayload.BatchId);
        Assert.False(detailPayload.DryRun);
        Assert.Equal(1, detailPayload.ImportedCount);
        Assert.Single(detailPayload.ReadyToPublishTrades);
        Assert.Single(detailPayload.ImportedTrades);
        var rejection = Assert.Single(detailPayload.Rejections);
        Assert.Equal("Validate", rejection.Stage);
        Assert.Equal("UnknownUser", rejection.Code);
    }

    [Fact]
    public async Task Get_trade_import_batches_returns_recent_batches_first()
    {
        var firstResponse = await _client.PostAsJsonAsync(
            "/api/v1/trades/import",
            new
            {
                dryRun = true,
                trades = new object[]
                {
                    new
                    {
                        userName = "margaret",
                        instrumentSymbol = "CL",
                        side = "Buy",
                        quantity = 1.0000m,
                        price = 81.0000m,
                        tradedUtc = "2025-02-08T11:00:00Z",
                    },
                },
            });

        firstResponse.EnsureSuccessStatusCode();
        var firstBatch = await firstResponse.Content.ReadFromJsonAsync<TradeImportResult>();

        var secondResponse = await _client.PostAsJsonAsync(
            "/api/v1/trades/import",
            new
            {
                dryRun = true,
                trades = new object[]
                {
                    new
                    {
                        userName = "grace",
                        instrumentSymbol = "AAPL",
                        side = "Sell",
                        quantity = 2.0000m,
                        price = 190.0000m,
                        tradedUtc = "2025-02-08T11:05:00Z",
                    },
                },
            });

        secondResponse.EnsureSuccessStatusCode();
        var secondBatch = await secondResponse.Content.ReadFromJsonAsync<TradeImportResult>();

        Assert.NotNull(firstBatch);
        Assert.NotNull(secondBatch);

        var historyResponse = await _client.GetAsync("/api/v1/trades/import-batches?top=2");
        historyResponse.EnsureSuccessStatusCode();

        var historyPayload = await historyResponse.Content.ReadFromJsonAsync<TradeImportBatchListItem[]>();

        Assert.NotNull(historyPayload);
        Assert.Equal(2, historyPayload.Length);
        Assert.Equal(secondBatch.BatchId, historyPayload[0].BatchId);
        Assert.Equal(firstBatch.BatchId, historyPayload[1].BatchId);
    }
}