using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Respawn;
using SqlAcademy.Persistence.Database;
using SqlAcademy.Persistence.Initialization;
using SqlAcademy.Persistence.MultiTenancy;
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

    public async Task<LearningDbContext> CreateTenantScopedDbContextAsync(
        int? tenantId,
        bool trackingEnabled = true,
        bool bypassRowLevelSecurity = false)
    {
        var dbContext = CreateDbContext(trackingEnabled);
        await dbContext.Database.OpenConnectionAsync();
        await ApplySessionContextAsync(
            (SqlConnection)dbContext.Database.GetDbConnection(),
            tenantId,
            bypassRowLevelSecurity);

        return dbContext;
    }

    private async Task EnsureSchemaAndSeedAsync()
    {
        await using var dbContext = CreateDbContext();
        await dbContext.Database.OpenConnectionAsync();
        await ApplySessionContextAsync((SqlConnection)dbContext.Database.GetDbConnection(), null, bypassRowLevelSecurity: true);
        await dbContext.Database.MigrateAsync();
        await LearningDbSeed.SeedAsync(dbContext, NullLogger.Instance, CancellationToken.None);
    }

    private async Task EnsureSeedOnlyAsync()
    {
        await using var dbContext = CreateDbContext();
        await dbContext.Database.OpenConnectionAsync();
        await ApplySessionContextAsync((SqlConnection)dbContext.Database.GetDbConnection(), null, bypassRowLevelSecurity: true);
        await LearningDbSeed.SeedAsync(dbContext, NullLogger.Instance, CancellationToken.None);
    }

    private static async Task ApplySessionContextAsync(SqlConnection connection, int? tenantId, bool bypassRowLevelSecurity)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = $"""
EXEC sys.sp_set_session_context @key = N'{SqlSessionContextKeys.TenantId}', @value = @tenantId;
EXEC sys.sp_set_session_context @key = N'{SqlSessionContextKeys.RlsBypass}', @value = @rlsBypass;
""";
        command.Parameters.AddWithValue("@tenantId", tenantId.HasValue ? tenantId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@rlsBypass", bypassRowLevelSecurity ? 1 : 0);
        await command.ExecuteNonQueryAsync();
    }
}