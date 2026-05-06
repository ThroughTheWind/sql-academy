using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Respawn;
using SqlAcademy.Persistence.Database;
using SqlAcademy.Persistence.Initialization;
using Testcontainers.MsSql;

namespace SqlAcademy.PerformanceTests.Testing;

public sealed class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("SqlAcademy_perf_2026!")
        .Build();

    private Respawner? _respawner;

    public string ConnectionString
    {
        get
        {
            var builder = new SqlConnectionStringBuilder(_container.GetConnectionString())
            {
                InitialCatalog = "LearningDb",
                Encrypt = false,
                TrustServerCertificate = true,
            };

            return builder.ConnectionString;
        }
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await EnsureSchemaAndSeedAsync();

        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = ["academy"],
        });
    }

    public async Task ResetAsync()
    {
        if (_respawner is null)
        {
            throw new InvalidOperationException("The SQL Server performance fixture was not initialized before reset.");
        }

        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await _respawner.ResetAsync(connection);
        await EnsureSeedOnlyAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public LearningDbContext CreateDbContext(bool trackingEnabled = true)
    {
        var options = new DbContextOptionsBuilder<LearningDbContext>()
            .UseSqlServer(ConnectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly("SqlAcademy.Migrations");
                sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

        if (!trackingEnabled)
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        return new LearningDbContext(options.Options);
    }

    private async Task EnsureSchemaAndSeedAsync()
    {
        await using var dbContext = CreateDbContext();
        await dbContext.Database.MigrateAsync();
        await LearningDbSeed.SeedAsync(dbContext, NullLogger.Instance, CancellationToken.None);
    }

    private async Task EnsureSeedOnlyAsync()
    {
        await using var dbContext = CreateDbContext();
        await LearningDbSeed.SeedAsync(dbContext, NullLogger.Instance, CancellationToken.None);
    }
}