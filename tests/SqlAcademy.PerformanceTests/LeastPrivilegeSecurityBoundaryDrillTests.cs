namespace SqlAcademy.PerformanceTests;

public sealed class LeastPrivilegeSecurityBoundaryDrillAssetTests
{
    [Fact]
    public async Task Senior_011_pack_distinguishes_runtime_privilege_migration_authority_and_operator_admin_boundaries()
    {
        var readmePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "011-least-privilege-and-operational-security-boundaries",
            "README.md");

        var templatePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "011-least-privilege-and-operational-security-boundaries",
            "investigation-template.md");

        var surfacesPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "011-least-privilege-and-operational-security-boundaries",
            "privilege-surfaces.md");

        var gapsPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "011-least-privilege-and-operational-security-boundaries",
            "boundary-gaps.md");

        var solutionPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "011-least-privilege-and-operational-security-boundaries",
            "optional-solution.md");

        var readmeText = await File.ReadAllTextAsync(readmePath);
        var templateText = await File.ReadAllTextAsync(templatePath);
        var surfacesText = await File.ReadAllTextAsync(surfacesPath);
        var gapsText = await File.ReadAllTextAsync(gapsPath);
        var solutionText = await File.ReadAllTextAsync(solutionPath);

        Assert.True(readmeText.Contains("investigation pack", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("privilege-surfaces.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("boundary-gaps.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("least privilege", StringComparison.OrdinalIgnoreCase));

        Assert.True(templateText.Contains("Boundary Decision", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Privilege Surface Review", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Separation-Of-Duties Requirement", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Operational Proof", StringComparison.OrdinalIgnoreCase));

        Assert.True(surfacesText.Contains("sa", StringComparison.OrdinalIgnoreCase));
        Assert.True(surfacesText.Contains("InitializeLearningDatabaseAsync", StringComparison.OrdinalIgnoreCase));
        Assert.True(surfacesText.Contains("DesignTimeLearningDbContextFactory", StringComparison.OrdinalIgnoreCase));
        Assert.True(surfacesText.Contains("Grafana", StringComparison.OrdinalIgnoreCase));

        Assert.True(gapsText.Contains("application permissions", StringComparison.OrdinalIgnoreCase));
        Assert.True(gapsText.Contains("startup migrations", StringComparison.OrdinalIgnoreCase));
        Assert.True(gapsText.Contains("privileged fallback", StringComparison.OrdinalIgnoreCase));

        Assert.True(solutionText.Contains("local-learning only", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("sa", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("migration authority", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("Grafana", StringComparison.OrdinalIgnoreCase));
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
