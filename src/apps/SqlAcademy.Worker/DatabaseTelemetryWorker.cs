using Microsoft.EntityFrameworkCore;
using SqlAcademy.Persistence.Database;

namespace SqlAcademy.Worker;

public sealed class DatabaseTelemetryWorker(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<DatabaseTelemetryWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var pollIntervalSeconds = configuration.GetValue("Worker:PollIntervalSeconds", 15);
        var pollInterval = TimeSpan.FromSeconds(Math.Max(5, pollIntervalSeconds));

        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LearningDbContext>();

            var postCount = await dbContext.Posts.AsNoTracking().CountAsync(stoppingToken);
            var tradeCount = await dbContext.Trades.AsNoTracking().CountAsync(stoppingToken);
            var orderCount = await dbContext.Orders.AsNoTracking().CountAsync(stoppingToken);

            logger.LogInformation(
                "LearningDb snapshot: {PostCount} posts, {TradeCount} trades, {OrderCount} orders.",
                postCount,
                tradeCount,
                orderCount);

            try
            {
                await Task.Delay(pollInterval, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }
}