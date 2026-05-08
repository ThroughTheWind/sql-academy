namespace SqlAcademy.PerformanceTests;

public sealed class BackupRestoreRecoveryDrillAssetTests
{
    [Fact]
    public async Task Senior_007_pack_combines_recovery_targets_backup_chain_and_restore_verification()
    {
        var readmePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "007-backup-restore-and-recovery-verification",
            "README.md");

        var templatePath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "007-backup-restore-and-recovery-verification",
            "investigation-template.md");

        var inventoryPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "007-backup-restore-and-recovery-verification",
            "backup-inventory.md");

        var verificationPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "007-backup-restore-and-recovery-verification",
            "restore-verification.md");

        var solutionPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Senior",
            "007-backup-restore-and-recovery-verification",
            "optional-solution.md");

        var readmeText = await File.ReadAllTextAsync(readmePath);
        var templateText = await File.ReadAllTextAsync(templatePath);
        var inventoryText = await File.ReadAllTextAsync(inventoryPath);
        var verificationText = await File.ReadAllTextAsync(verificationPath);
        var solutionText = await File.ReadAllTextAsync(solutionPath);

        Assert.True(readmeText.Contains("investigation pack", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("backup-inventory.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("restore-verification.md", StringComparison.OrdinalIgnoreCase));
        Assert.True(readmeText.Contains("RPO", StringComparison.OrdinalIgnoreCase));

        Assert.True(templateText.Contains("Recovery Objective", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Backup Chain Review", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Restore Drill Decision", StringComparison.OrdinalIgnoreCase));
        Assert.True(templateText.Contains("Post-Restore Validation", StringComparison.OrdinalIgnoreCase));

        Assert.True(inventoryText.Contains("target RPO = 15 minutes", StringComparison.OrdinalIgnoreCase));
        Assert.True(inventoryText.Contains("log", StringComparison.OrdinalIgnoreCase));

        Assert.True(verificationText.Contains("RESTORE VERIFYONLY", StringComparison.OrdinalIgnoreCase));
        Assert.True(verificationText.Contains("DBCC CHECKDB", StringComparison.OrdinalIgnoreCase));
        Assert.True(verificationText.Contains("/health/ready", StringComparison.OrdinalIgnoreCase));

        Assert.True(solutionText.Contains("Block the release", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("order write", StringComparison.OrdinalIgnoreCase));
        Assert.True(solutionText.Contains("restore duration", StringComparison.OrdinalIgnoreCase));
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
