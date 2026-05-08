namespace SqlAcademy.PerformanceTests;

public sealed class ReleaseReadinessDrillAssetTests
{
    [Fact]
    public async Task Senior_006_pack_combines_migration_telemetry_and_rollback_gates()
    {
        var readmePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "006-release-readiness-and-rollback-gates",
            "README.md");

        var templatePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "006-release-readiness-and-rollback-gates",
            "investigation-template.md");

        var migrationReviewPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "006-release-readiness-and-rollback-gates",
            "migration-review.md");

        var telemetryPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "006-release-readiness-and-rollback-gates",
            "telemetry-snapshot.md");

        var solutionPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "006-release-readiness-and-rollback-gates",
            "optional-solution.md");

        var readmeText = await File.ReadAllTextAsync(readmePath);
        var templateText = await File.ReadAllTextAsync(templatePath);
        var migrationReviewText = await File.ReadAllTextAsync(migrationReviewPath);
        var telemetryText = await File.ReadAllTextAsync(telemetryPath);
        var solutionText = await File.ReadAllTextAsync(solutionPath);

        Assert.True(readmeText.Contains("investigation pack", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("migration-review.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("telemetry-snapshot.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("rollback", StringComparison.OrdinalIgnoreCase));

        Assert.True(templateText.Contains("Go Or No-Go", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("First Five Minutes Watch Plan", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Rollback And Roll-Forward Gates", StringComparison.OrdinalIgnoreCase));

        Assert.True(migrationReviewText.Contains("NullExternalReferenceCount = 0", StringComparison.OrdinalIgnoreCase));
        Assert.True(migrationReviewText.Contains("DuplicateExternalReferenceCount = 0", StringComparison.OrdinalIgnoreCase));

        Assert.True(telemetryText.Contains("/health/ready", StringComparison.OrdinalIgnoreCase));
        Assert.True(telemetryText.Contains("latency", StringComparison.OrdinalIgnoreCase));

        Assert.True(solutionText.Contains("roll-forward", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("order write", StringComparison.OrdinalIgnoreCase));
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