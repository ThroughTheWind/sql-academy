namespace SqlAcademy.PerformanceTests;

public sealed class PostsApiObservabilityDrillAssetTests
{
    [Fact]
    public async Task Senior_004_pack_keeps_logs_metrics_traces_and_workbook_contract_aligned()
    {
        var readmePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "004-posts-api-latency-and-observability-triage",
            "README.md");

        var templatePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "004-posts-api-latency-and-observability-triage",
            "investigation-template.md");

        var logsPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "004-posts-api-latency-and-observability-triage",
            "logs-snapshot.md");

        var metricsPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "004-posts-api-latency-and-observability-triage",
            "metrics-snapshot.md");

        var tracePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "004-posts-api-latency-and-observability-triage",
            "trace-snapshot.md");

        var outcomesPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "004-posts-api-latency-and-observability-triage",
            "expected-outcomes.md");

        var readmeText = await File.ReadAllTextAsync(readmePath);
        var templateText = await File.ReadAllTextAsync(templatePath);
        var logsText = await File.ReadAllTextAsync(logsPath);
        var metricsText = await File.ReadAllTextAsync(metricsPath);
        var traceText = await File.ReadAllTextAsync(tracePath);
        var outcomesText = await File.ReadAllTextAsync(outcomesPath);

        Assert.True(readmeText.Contains("investigation pack", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("logs-snapshot.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("metrics-snapshot.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("trace-snapshot.md", StringComparison.OrdinalIgnoreCase));

        Assert.True(templateText.Contains("First Signal", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Narrowed Boundary", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Validation Plan", StringComparison.OrdinalIgnoreCase));

        Assert.True(logsText.Contains("GET /api/v1/posts", StringComparison.OrdinalIgnoreCase));
        Assert.True(logsText.Contains("ef.posts.list", StringComparison.OrdinalIgnoreCase));

        Assert.True(metricsText.Contains("p95 latency", StringComparison.OrdinalIgnoreCase));
        Assert.True(metricsText.Contains("database CPU", StringComparison.OrdinalIgnoreCase));

        Assert.True(traceText.Contains("db.query posts-list-with-comment-count-sort", StringComparison.OrdinalIgnoreCase));
        Assert.True(traceText.Contains("posts.sort_by = commentCount", StringComparison.OrdinalIgnoreCase));

        Assert.True(outcomesText.Contains("telemetry improvement", StringComparison.OrdinalIgnoreCase));
        Assert.True(outcomesText.Contains("test or benchmark", StringComparison.OrdinalIgnoreCase));
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
