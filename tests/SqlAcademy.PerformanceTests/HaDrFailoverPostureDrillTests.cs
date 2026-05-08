namespace SqlAcademy.PerformanceTests;

public sealed class HaDrFailoverPostureDrillAssetTests
{
    [Fact]
    public async Task Senior_009_pack_combines_recovery_targets_replica_posture_and_failover_verification()
    {
        var readmePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "009-ha-dr-failover-posture-and-verification",
            "README.md");

        var templatePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "009-ha-dr-failover-posture-and-verification",
            "investigation-template.md");

        var posturePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "009-ha-dr-failover-posture-and-verification",
            "replica-posture.md");

        var readinessPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "009-ha-dr-failover-posture-and-verification",
            "failover-readiness.md");

        var solutionPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "009-ha-dr-failover-posture-and-verification",
            "optional-solution.md");

        var readmeText = await File.ReadAllTextAsync(readmePath);
        var templateText = await File.ReadAllTextAsync(templatePath);
        var postureText = await File.ReadAllTextAsync(posturePath);
        var readinessText = await File.ReadAllTextAsync(readinessPath);
        var solutionText = await File.ReadAllTextAsync(solutionPath);

        Assert.True(readmeText.Contains("investigation pack", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("replica-posture.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("failover-readiness.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("RPO", StringComparison.OrdinalIgnoreCase));

        Assert.True(templateText.Contains("Recovery Target Review", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Replica And Lag Review", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Failover Decision", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Post-Failover Validation", StringComparison.OrdinalIgnoreCase));

        Assert.True(postureText.Contains("target RPO = 5 minutes", StringComparison.OrdinalIgnoreCase));
        Assert.True(postureText.Contains("18 minutes", StringComparison.OrdinalIgnoreCase));
        Assert.True(postureText.Contains("/health/ready", StringComparison.OrdinalIgnoreCase));

        Assert.True(readinessText.Contains("10-minute RTO", StringComparison.OrdinalIgnoreCase));
        Assert.True(readinessText.Contains("order write", StringComparison.OrdinalIgnoreCase));
        Assert.True(readinessText.Contains("stay on the primary", StringComparison.OrdinalIgnoreCase));

        Assert.True(solutionText.Contains("Block the maintenance window", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("/health/ready", StringComparison.OrdinalIgnoreCase));
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
