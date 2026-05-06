using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SqlAcademy.Persistence.Database;

namespace SqlAcademy.Migrations;

public sealed class DesignTimeLearningDbContextFactory : IDesignTimeDbContextFactory<LearningDbContext>
{
    public LearningDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var candidatePaths = new[]
        {
            Path.GetFullPath(Path.Combine(basePath, "src", "apps", "SqlAcademy.Api")),
            Path.GetFullPath(Path.Combine(basePath, "..", "..", "apps", "SqlAcademy.Api")),
        };

        var apiProjectPath = candidatePaths.FirstOrDefault(Directory.Exists)
            ?? throw new DirectoryNotFoundException("Could not locate the SqlAcademy.Api project for design-time migration creation.");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("LearningDb")
            ?? "Server=localhost,14333;Database=LearningDb;User ID=sa;Password=SqlAcademy_dev_2026!;Encrypt=False;TrustServerCertificate=True;ConnectRetryCount=3;ConnectRetryInterval=10";

        var optionsBuilder = new DbContextOptionsBuilder<LearningDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
        {
            sqlServerOptions.MigrationsAssembly(typeof(DesignTimeLearningDbContextFactory).Assembly.FullName);
            sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });

        return new LearningDbContext(optionsBuilder.Options);
    }
}