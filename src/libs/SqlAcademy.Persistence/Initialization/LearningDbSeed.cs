using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlAcademy.Domain.Entities;
using SqlAcademy.Domain.Enums;
using SqlAcademy.Persistence.Database;

namespace SqlAcademy.Persistence.Initialization;

public static class LearningDbSeed
{
    public static async Task SeedAsync(LearningDbContext dbContext, ILogger logger, CancellationToken cancellationToken)
    {
        if (await dbContext.Users.AnyAsync(cancellationToken))
        {
            logger.LogInformation("LearningDb already contains seed data. Skipping application seed step.");
            return;
        }

        var baseTimestamp = new DateTime(2025, 1, 15, 8, 30, 0, DateTimeKind.Utc);

        var users = new[]
        {
            new User { UserName = "ada", Email = "ada@sqlacademy.local", CreatedUtc = baseTimestamp },
            new User { UserName = "grace", Email = "grace@sqlacademy.local", CreatedUtc = baseTimestamp.AddMinutes(5) },
            new User { UserName = "linus", Email = "linus@sqlacademy.local", CreatedUtc = baseTimestamp.AddMinutes(10) },
            new User { UserName = "margaret", Email = "margaret@sqlacademy.local", CreatedUtc = baseTimestamp.AddMinutes(15) },
        };

        var instruments = new[]
        {
            new Instrument { Symbol = "MSFT", Name = "Microsoft Corporation", AssetClass = AssetClass.Equity, TickSize = 0.01m, LotSize = 1m, CreatedUtc = baseTimestamp },
            new Instrument { Symbol = "AAPL", Name = "Apple Inc.", AssetClass = AssetClass.Equity, TickSize = 0.01m, LotSize = 1m, CreatedUtc = baseTimestamp.AddMinutes(1) },
            new Instrument { Symbol = "EURUSD", Name = "Euro / US Dollar", AssetClass = AssetClass.ForeignExchange, TickSize = 0.0001m, LotSize = 1_000m, CreatedUtc = baseTimestamp.AddMinutes(2) },
            new Instrument { Symbol = "CL", Name = "WTI Crude Oil", AssetClass = AssetClass.Commodity, TickSize = 0.01m, LotSize = 1_000m, CreatedUtc = baseTimestamp.AddMinutes(3) },
        };

        var posts = new[]
        {
            new Post { Author = users[0], Title = "Understanding clustered indexes", Body = "Clustered indexes define the storage order of table data and shape the cost of range scans.", CreatedUtc = baseTimestamp.AddDays(1) },
            new Post { Author = users[1], Title = "When to prefer window functions", Body = "Window functions are ideal when you need ranking, running totals, and gap analysis without collapsing result sets.", CreatedUtc = baseTimestamp.AddDays(2) },
            new Post { Author = users[2], Title = "Concurrency surprises in OLTP systems", Body = "Lock escalation, blocking chains, and transaction scope are usually visible only when load arrives.", CreatedUtc = baseTimestamp.AddDays(3) },
            new Post { Author = users[3], Title = "Operational playbooks for SQL releases", Body = "Safe deployments start with reversible migrations, targeted smoke tests, and a clear rollback plan.", CreatedUtc = baseTimestamp.AddDays(4) },
        };

        var comments = new[]
        {
            new Comment { Post = posts[0], Author = users[1], Body = "Use fill factor discussions here because learners usually miss fragmentation tradeoffs.", CreatedUtc = baseTimestamp.AddDays(1).AddHours(2) },
            new Comment { Post = posts[0], Author = users[2], Body = "Cover lookup amplification too, especially for wide tables.", CreatedUtc = baseTimestamp.AddDays(1).AddHours(3) },
            new Comment { Post = posts[1], Author = users[0], Body = "A running sum example with partitions makes the value obvious.", CreatedUtc = baseTimestamp.AddDays(2).AddHours(1) },
            new Comment { Post = posts[2], Author = users[3], Body = "Pair this with a deadlock graph walkthrough.", CreatedUtc = baseTimestamp.AddDays(3).AddHours(4) },
        };

        var orders = new[]
        {
            new Order { User = users[0], OrderNumber = "ORD-2025-0001", Status = OrderStatus.Filled, TotalAmount = 512.40m, CreatedUtc = baseTimestamp.AddDays(5), UpdatedUtc = baseTimestamp.AddDays(5).AddMinutes(5) },
            new Order { User = users[1], OrderNumber = "ORD-2025-0002", Status = OrderStatus.Submitted, TotalAmount = 980.10m, CreatedUtc = baseTimestamp.AddDays(5).AddHours(2), UpdatedUtc = baseTimestamp.AddDays(5).AddHours(2).AddMinutes(2) },
            new Order { User = users[2], OrderNumber = "ORD-2025-0003", Status = OrderStatus.Pending, TotalAmount = 143.00m, CreatedUtc = baseTimestamp.AddDays(6), UpdatedUtc = baseTimestamp.AddDays(6).AddMinutes(1) },
        };

        var trades = new[]
        {
            new Trade { User = users[0], Instrument = instruments[0], Side = TradeSide.Buy, Quantity = 100m, Price = 420.50m, TradedUtc = baseTimestamp.AddDays(5).AddMinutes(15) },
            new Trade { User = users[1], Instrument = instruments[1], Side = TradeSide.Buy, Quantity = 25m, Price = 187.20m, TradedUtc = baseTimestamp.AddDays(5).AddMinutes(25) },
            new Trade { User = users[2], Instrument = instruments[2], Side = TradeSide.Sell, Quantity = 25_000m, Price = 1.0842m, TradedUtc = baseTimestamp.AddDays(6).AddMinutes(10) },
            new Trade { User = users[3], Instrument = instruments[3], Side = TradeSide.Buy, Quantity = 2m, Price = 77.85m, TradedUtc = baseTimestamp.AddDays(6).AddMinutes(45) },
        };

        await dbContext.AddRangeAsync(users, cancellationToken);
        await dbContext.AddRangeAsync(instruments, cancellationToken);
        await dbContext.AddRangeAsync(posts, cancellationToken);
        await dbContext.AddRangeAsync(comments, cancellationToken);
        await dbContext.AddRangeAsync(orders, cancellationToken);
        await dbContext.AddRangeAsync(trades, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded LearningDb with baseline academy data for users, posts, comments, orders, trades, and instruments.");
    }
}