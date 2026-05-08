namespace SqlAcademy.PerformanceTests;

public sealed class RowLevelSecurityTenantIsolationDrillAssetTests
{
    [Fact]
    public async Task Rls_lesson_and_senior_012_guided_lab_keep_runtime_sample_and_workbook_prompts_aligned()
    {
        var lessonPath = ResolveRepositoryPath(
            "docs",
            "learning",
            "row-level-security-and-tenant-isolation.md");

        var readmePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "012-row-level-security-and-tenant-isolation",
            "README.md");

        var templatePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "012-row-level-security-and-tenant-isolation",
            "investigation-template.md");

        var policyPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "012-row-level-security-and-tenant-isolation",
            "starter-policy.sql");

        var surfacesPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "012-row-level-security-and-tenant-isolation",
            "tenant-context-surfaces.md");

        var failureModesPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "012-row-level-security-and-tenant-isolation",
            "policy-failure-modes.md");

        var solutionPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "012-row-level-security-and-tenant-isolation",
            "optional-solution.md");

        var lessonText = await File.ReadAllTextAsync(lessonPath);
        var readmeText = await File.ReadAllTextAsync(readmePath);
        var templateText = await File.ReadAllTextAsync(templatePath);
        var policyText = await File.ReadAllTextAsync(policyPath);
        var surfacesText = await File.ReadAllTextAsync(surfacesPath);
        var failureModesText = await File.ReadAllTextAsync(failureModesPath);
        var solutionText = await File.ReadAllTextAsync(solutionPath);

        Assert.True(lessonText.Contains("SESSION_CONTEXT", StringComparison.OrdinalIgnoreCase));
        Assert.True(lessonText.Contains("CREATE SECURITY POLICY", StringComparison.OrdinalIgnoreCase));
        Assert.True(lessonText.Contains("Senior 012", StringComparison.OrdinalIgnoreCase));
        Assert.True(lessonText.Contains("TenantId", StringComparison.OrdinalIgnoreCase));
        Assert.True(lessonText.Contains("academy.TenantOrders", StringComparison.OrdinalIgnoreCase));

        Assert.True(readmeText.Contains("guided lab", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("TenantOrdersEndpointTests", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("RowLevelSecuritySampleTests", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("starter-policy.sql", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("row-level security", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("tenant isolation", StringComparison.OrdinalIgnoreCase));

        Assert.True(templateText.Contains("Adoption Decision", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Schema And Session Preconditions", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Policy Review", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Proof And Rollback", StringComparison.OrdinalIgnoreCase));

        Assert.True(policyText.Contains("sp_set_session_context", StringComparison.OrdinalIgnoreCase));
        Assert.True(policyText.Contains("CREATE SECURITY POLICY", StringComparison.OrdinalIgnoreCase));
        Assert.True(policyText.Contains("FILTER PREDICATE", StringComparison.OrdinalIgnoreCase));
        Assert.True(policyText.Contains("IS_MEMBER", StringComparison.OrdinalIgnoreCase));
        Assert.True(policyText.Contains("academy.TenantOrders", StringComparison.OrdinalIgnoreCase));

        Assert.True(surfacesText.Contains("TenantId", StringComparison.OrdinalIgnoreCase));
        Assert.True(surfacesText.Contains("TenantSessionContextMiddleware", StringComparison.OrdinalIgnoreCase));
        Assert.True(surfacesText.Contains("SESSION_CONTEXT", StringComparison.OrdinalIgnoreCase));
        Assert.True(surfacesText.Contains("academy.TenantOrders", StringComparison.OrdinalIgnoreCase));

        Assert.True(failureModesText.Contains("predicate function", StringComparison.OrdinalIgnoreCase));
        Assert.True(failureModesText.Contains("block predicates", StringComparison.OrdinalIgnoreCase));
        Assert.True(failureModesText.Contains("background jobs", StringComparison.OrdinalIgnoreCase));
        Assert.True(failureModesText.Contains("rls_policy_admin", StringComparison.OrdinalIgnoreCase));

        Assert.True(solutionText.Contains("academy.TenantOrders", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("TenantId", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("SESSION_CONTEXT", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("rls_policy_admin", StringComparison.OrdinalIgnoreCase));
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