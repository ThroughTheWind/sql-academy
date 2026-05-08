namespace SqlAcademy.PerformanceTests;

public sealed class ChangeCaptureReconciliationDrillAssetTests
{
    [Fact]
    public async Task Senior_010_pack_distinguishes_provenance_from_reconciliation_and_replay_proof()
    {
        var readmePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "010-change-capture-provenance-and-reconciliation",
            "README.md");

        var templatePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "010-change-capture-provenance-and-reconciliation",
            "investigation-template.md");

        var surfacesPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "010-change-capture-provenance-and-reconciliation",
            "capture-surfaces.md");

        var gapsPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "010-change-capture-provenance-and-reconciliation",
            "reconciliation-gaps.md");

        var solutionPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "010-change-capture-provenance-and-reconciliation",
            "optional-solution.md");

        var readmeText = await File.ReadAllTextAsync(readmePath);
        var templateText = await File.ReadAllTextAsync(templatePath);
        var surfacesText = await File.ReadAllTextAsync(surfacesPath);
        var gapsText = await File.ReadAllTextAsync(gapsPath);
        var solutionText = await File.ReadAllTextAsync(solutionPath);

        Assert.True(readmeText.Contains("investigation pack", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("capture-surfaces.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("reconciliation-gaps.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("CDC", StringComparison.OrdinalIgnoreCase));

        Assert.True(templateText.Contains("Provenance Decision", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Capture Surface Review", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Reconciliation Requirement", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Post-Movement Proof", StringComparison.OrdinalIgnoreCase));

        Assert.True(surfacesText.Contains("Source", StringComparison.OrdinalIgnoreCase));
        Assert.True(surfacesText.Contains("CorrelationId", StringComparison.OrdinalIgnoreCase));
        Assert.True(surfacesText.Contains("import-batches", StringComparison.OrdinalIgnoreCase));
        Assert.True(surfacesText.Contains("Senior 003", StringComparison.OrdinalIgnoreCase));

        Assert.True(gapsText.Contains("reconciliation proof", StringComparison.OrdinalIgnoreCase));
        Assert.True(gapsText.Contains("skipped", StringComparison.OrdinalIgnoreCase));
        Assert.True(gapsText.Contains("duplicated", StringComparison.OrdinalIgnoreCase));

        Assert.True(solutionText.Contains("not yet fully credible", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("Source", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("CorrelationId", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("stop condition", StringComparison.OrdinalIgnoreCase));
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
