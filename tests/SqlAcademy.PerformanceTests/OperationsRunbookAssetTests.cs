namespace SqlAcademy.PerformanceTests;

public sealed class OperationsRunbookAssetTests
{
    [Fact]
    public async Task Release_and_incident_runbooks_keep_their_operational_checklists_aligned()
    {
        var releaseRunbookPath = ResolveRepositoryPath(
            "docs",
            "operations",
            "release-runbook.md");

        var incidentRunbookPath = ResolveRepositoryPath(
            "docs",
            "operations",
            "incident-triage-runbook.md");

        var releaseText = await File.ReadAllTextAsync(releaseRunbookPath);
        var incidentText = await File.ReadAllTextAsync(incidentRunbookPath);

        Assert.True(releaseText.Contains("Preflight Gates", StringComparison.OrdinalIgnoreCase));
        Assert.True(releaseText.Contains("/health/ready", StringComparison.OrdinalIgnoreCase));
        Assert.True(releaseText.Contains("First Five Minutes Watch Plan", StringComparison.OrdinalIgnoreCase));
        Assert.True(releaseText.Contains("Rollback Versus Roll-Forward", StringComparison.OrdinalIgnoreCase));
        Assert.True(releaseText.Contains("Post-Release Validation", StringComparison.OrdinalIgnoreCase));
        Assert.True(releaseText.Contains("Senior 006", StringComparison.OrdinalIgnoreCase));

        Assert.True(incidentText.Contains("First Signal", StringComparison.OrdinalIgnoreCase));
        Assert.True(incidentText.Contains("Narrowed Boundary", StringComparison.OrdinalIgnoreCase));
        Assert.True(incidentText.Contains("Logs, Metrics, And Traces", StringComparison.OrdinalIgnoreCase));
        Assert.True(incidentText.Contains("Immediate Containment", StringComparison.OrdinalIgnoreCase));
        Assert.True(incidentText.Contains("Durable Fix And Follow-Up Validation", StringComparison.OrdinalIgnoreCase));
        Assert.True(incidentText.Contains("Senior 004", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Deadlock_and_outbox_runbooks_keep_their_scenario_contracts_aligned()
    {
        var deadlockRunbookPath = ResolveRepositoryPath(
            "docs",
            "operations",
            "deadlock-response-runbook.md");

        var outboxRunbookPath = ResolveRepositoryPath(
            "docs",
            "operations",
            "outbox-delivery-runbook.md");

        var deadlockText = await File.ReadAllTextAsync(deadlockRunbookPath);
        var outboxText = await File.ReadAllTextAsync(outboxRunbookPath);

        Assert.True(deadlockText.Contains("Blocking And Graph Evidence", StringComparison.OrdinalIgnoreCase));
        Assert.True(deadlockText.Contains("victim", StringComparison.OrdinalIgnoreCase));
        Assert.True(deadlockText.Contains("Retry Versus Redesign", StringComparison.OrdinalIgnoreCase));
        Assert.True(deadlockText.Contains("consistent access order", StringComparison.OrdinalIgnoreCase));
        Assert.True(deadlockText.Contains("Senior 001", StringComparison.OrdinalIgnoreCase));

        Assert.True(outboxText.Contains("Idempotent Insert Contract", StringComparison.OrdinalIgnoreCase));
        Assert.True(outboxText.Contains("Dispatch Batch Discipline", StringComparison.OrdinalIgnoreCase));
        Assert.True(outboxText.Contains("Consistency Proof", StringComparison.OrdinalIgnoreCase));
        Assert.True(outboxText.Contains("duplicate", StringComparison.OrdinalIgnoreCase));
        Assert.True(outboxText.Contains("Senior 003", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Query_store_runbook_keeps_persisted_regression_checklist_aligned()
    {
        var runbookPath = ResolveRepositoryPath(
            "docs",
            "operations",
            "query-store-regression-runbook.md");

        var runbookText = await File.ReadAllTextAsync(runbookPath);

        Assert.True(runbookText.Contains("Scope The Query Store Window", StringComparison.OrdinalIgnoreCase));
        Assert.True(runbookText.Contains("Isolate The Query And Plan History", StringComparison.OrdinalIgnoreCase));
        Assert.True(runbookText.Contains("plan forcing", StringComparison.OrdinalIgnoreCase));
        Assert.True(runbookText.Contains("query_id", StringComparison.OrdinalIgnoreCase));
        Assert.True(runbookText.Contains("QueryStoreLabSmokeTests", StringComparison.OrdinalIgnoreCase));
        Assert.True(runbookText.Contains("Advanced 004", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Change_capture_security_and_rls_runbooks_keep_new_specialization_checklists_aligned()
    {
        var changeCaptureRunbookPath = ResolveRepositoryPath(
            "docs",
            "operations",
            "change-capture-reconciliation-runbook.md");

        var leastPrivilegeRunbookPath = ResolveRepositoryPath(
            "docs",
            "operations",
            "least-privilege-security-runbook.md");

        var rowLevelSecurityRunbookPath = ResolveRepositoryPath(
            "docs",
            "operations",
            "row-level-security-tenant-isolation-runbook.md");

        var changeCaptureText = await File.ReadAllTextAsync(changeCaptureRunbookPath);
        var leastPrivilegeText = await File.ReadAllTextAsync(leastPrivilegeRunbookPath);
        var rowLevelSecurityText = await File.ReadAllTextAsync(rowLevelSecurityRunbookPath);

        Assert.True(changeCaptureText.Contains("Provenance Boundary", StringComparison.OrdinalIgnoreCase));
        Assert.True(changeCaptureText.Contains("Reconciliation Proof", StringComparison.OrdinalIgnoreCase));
        Assert.True(changeCaptureText.Contains("Source", StringComparison.OrdinalIgnoreCase));
        Assert.True(changeCaptureText.Contains("CorrelationId", StringComparison.OrdinalIgnoreCase));
        Assert.True(changeCaptureText.Contains("Senior 010", StringComparison.OrdinalIgnoreCase));

        Assert.True(leastPrivilegeText.Contains("Runtime Versus Migration Authority", StringComparison.OrdinalIgnoreCase));
        Assert.True(leastPrivilegeText.Contains("Admin Surface Review", StringComparison.OrdinalIgnoreCase));
        Assert.True(leastPrivilegeText.Contains("sa", StringComparison.OrdinalIgnoreCase));
        Assert.True(leastPrivilegeText.Contains("InitializeLearningDatabaseAsync", StringComparison.OrdinalIgnoreCase));
        Assert.True(leastPrivilegeText.Contains("Senior 011", StringComparison.OrdinalIgnoreCase));

        Assert.True(rowLevelSecurityText.Contains("Tenant Context Gate", StringComparison.OrdinalIgnoreCase));
        Assert.True(rowLevelSecurityText.Contains("Policy Proof", StringComparison.OrdinalIgnoreCase));
        Assert.True(rowLevelSecurityText.Contains("Controlled Bypass Review", StringComparison.OrdinalIgnoreCase));
        Assert.True(rowLevelSecurityText.Contains("Disable And Recovery", StringComparison.OrdinalIgnoreCase));
        Assert.True(rowLevelSecurityText.Contains("TenantOrderIsolationPolicy", StringComparison.OrdinalIgnoreCase));
        Assert.True(rowLevelSecurityText.Contains("Senior 012", StringComparison.OrdinalIgnoreCase));
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
