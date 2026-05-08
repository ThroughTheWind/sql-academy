using System.Xml.Linq;
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

public sealed class DeadlockGraphLabAssetTests
{
    [Fact]
    public async Task Senior_001_pack_includes_deadlock_graph_assets_and_retry_guidance()
    {
        var readmePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "001-concurrency-blocking-and-deadlocks",
            "README.md");

        var labPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "001-concurrency-blocking-and-deadlocks",
            "deadlock-graph-lab.md");

        var graphPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "001-concurrency-blocking-and-deadlocks",
            "deadlock-graph-sample.xml");

        var readmeText = await File.ReadAllTextAsync(readmePath);
        var labText = await File.ReadAllTextAsync(labPath);
        var graphDocument = XDocument.Load(graphPath);

        Assert.True(readmeText.Contains("deadlock-graph-lab.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("deadlock-graph-sample.xml", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("retry", StringComparison.OrdinalIgnoreCase));

        Assert.True(labText.Contains("deadlock-graph-sample.xml", StringComparison.OrdinalIgnoreCase));
        Assert.True(labText.Contains("retry", StringComparison.OrdinalIgnoreCase));

        var root = graphDocument.Root;
        Assert.NotNull(root);
        Assert.Equal("deadlock", root!.Name.LocalName);

        var victimProcessId = root.Element("victim-list")?.Element("victimProcess")?.Attribute("id")?.Value;
        Assert.False(string.IsNullOrWhiteSpace(victimProcessId));

        var processInputs = root.Element("process-list")?
            .Elements("process")
            .Select(process => process.Element("inputbuf")?.Value ?? string.Empty)
            .ToArray() ?? [];

        Assert.Contains(processInputs, input => input.Contains("academy.Trades", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(processInputs, input => input.Contains("academy.Orders", StringComparison.OrdinalIgnoreCase));

        var resourceNames = root.Element("resource-list")?
            .Elements()
            .Select(resource => resource.Attribute("objectname")?.Value ?? string.Empty)
            .ToArray() ?? [];

        Assert.Contains(resourceNames, name => name.Contains("academy.Orders", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(resourceNames, name => name.Contains("academy.Trades", StringComparison.OrdinalIgnoreCase));

        var logUsedValues = root.Element("process-list")?
            .Elements("process")
            .Select(process => process.Attribute("logused")?.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToArray() ?? [];

        Assert.Contains("96", logUsedValues);
    }

    private static string ResolveRepositoryPath(params string[] relativeSegments)
    {
        var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);

        while (currentDirectory is not null)
        {
            var candidateSolutionPath = Path.Combine(currentDirectory.FullName, "SqlAcademy.slnx");

            if (File.Exists(candidateSolutionPath))
            {
                return Path.Combine([currentDirectory.FullName, ..relativeSegments]);
            }

            currentDirectory = currentDirectory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate the repository root from the performance test output directory.");
    }
}