namespace SqlAcademy.PerformanceTests;

public sealed class SqlServerAgentSchedulingDrillAssetTests
{
    [Fact]
    public async Task Senior_008_pack_distinguishes_agent_jobs_worker_loops_and_job_safety_rules()
    {
        var readmePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "008-sql-server-agent-job-safety-and-operational-scheduling",
            "README.md");

        var templatePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "008-sql-server-agent-job-safety-and-operational-scheduling",
            "investigation-template.md");

        var catalogPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "008-sql-server-agent-job-safety-and-operational-scheduling",
            "job-catalog.md");

        var workerNotesPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "008-sql-server-agent-job-safety-and-operational-scheduling",
            "worker-notes.md");

        var solutionPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "008-sql-server-agent-job-safety-and-operational-scheduling",
            "optional-solution.md");

        var readmeText = await File.ReadAllTextAsync(readmePath);
        var templateText = await File.ReadAllTextAsync(templatePath);
        var catalogText = await File.ReadAllTextAsync(catalogPath);
        var workerNotesText = await File.ReadAllTextAsync(workerNotesPath);
        var solutionText = await File.ReadAllTextAsync(solutionPath);

        Assert.True(readmeText.Contains("investigation pack", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("job-catalog.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("worker-notes.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("SQL Server Agent", StringComparison.OrdinalIgnoreCase));

        Assert.True(templateText.Contains("Job Placement Decision", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Overlap And Idempotency Risk", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Failure And Retry Rule", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Observability And Post-Run Validation", StringComparison.OrdinalIgnoreCase));

        Assert.True(catalogText.Contains("DatabaseTelemetrySnapshot", StringComparison.OrdinalIgnoreCase));
        Assert.True(catalogText.Contains("OutboxConsistencySweep", StringComparison.OrdinalIgnoreCase));
        Assert.True(catalogText.Contains("SchemaPreflightGateCheck", StringComparison.OrdinalIgnoreCase));

        Assert.True(workerNotesText.Contains("DatabaseTelemetryWorker", StringComparison.OrdinalIgnoreCase));
        Assert.True(workerNotesText.Contains("AddHostedService", StringComparison.OrdinalIgnoreCase));
        Assert.True(workerNotesText.Contains("PollIntervalSeconds", StringComparison.OrdinalIgnoreCase));

        Assert.True(solutionText.Contains("SqlAcademy.Worker", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("SQL Server Agent", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("release-step", StringComparison.OrdinalIgnoreCase));
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
