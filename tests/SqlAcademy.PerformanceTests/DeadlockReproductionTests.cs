using SqlAcademy.PerformanceTests.Testing;

namespace SqlAcademy.PerformanceTests;

[Collection(PerformanceTestCollection.Name)]
public sealed class DeadlockReproductionTests(SqlServerFixture databaseFixture) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await databaseFixture.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact(Skip = "Manual lab: reproduce the deadlock via the Senior exercise assets to inspect the deadlock graph and retry behavior.")]
    public Task Deadlock_scenario_is_available_as_a_guided_lab()
    {
        return Task.CompletedTask;
    }
}