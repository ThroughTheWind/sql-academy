using Microsoft.EntityFrameworkCore;
using SqlAcademy.PerformanceTests.Testing;

namespace SqlAcademy.PerformanceTests;

[Collection(PerformanceTestCollection.Name)]
public sealed class TrackingBehaviorTests(SqlServerFixture databaseFixture) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await databaseFixture.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Tracking_returns_the_same_entity_instance_while_no_tracking_does_not()
    {
        await using var trackingContext = databaseFixture.CreateDbContext();
        var trackedFirst = await trackingContext.Posts.OrderBy(post => post.Id).FirstAsync();
        var trackedSecond = await trackingContext.Posts.OrderBy(post => post.Id).FirstAsync();

        Assert.Same(trackedFirst, trackedSecond);

        await using var noTrackingContext = databaseFixture.CreateDbContext(trackingEnabled: false);
        var untrackedFirst = await noTrackingContext.Posts.AsNoTracking().OrderBy(post => post.Id).FirstAsync();
        var untrackedSecond = await noTrackingContext.Posts.AsNoTracking().OrderBy(post => post.Id).FirstAsync();

        Assert.NotSame(untrackedFirst, untrackedSecond);
    }
}