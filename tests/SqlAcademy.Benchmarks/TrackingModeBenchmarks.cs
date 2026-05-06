using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using SqlAcademy.Persistence.Database;

namespace SqlAcademy.Benchmarks;

[MemoryDiagnoser]
public class TrackingModeBenchmarks
{
    private DbContextOptions<LearningDbContext> _options = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var connectionString = Environment.GetEnvironmentVariable("SQLACADEMY_BENCHMARK_CONNECTIONSTRING")
            ?? "Server=localhost,14333;Database=LearningDb;User ID=sa;Password=SqlAcademy_dev_2026!;Encrypt=False;TrustServerCertificate=True;ConnectRetryCount=3;ConnectRetryInterval=10";

        _options = new DbContextOptionsBuilder<LearningDbContext>()
            .UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly("SqlAcademy.Migrations");
                sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            })
            .Options;
    }

    [Benchmark(Baseline = true)]
    public async Task<int> Query_with_tracking()
    {
        await using var dbContext = new LearningDbContext(_options);
        return await dbContext.Posts.AsTracking().CountAsync();
    }

    [Benchmark]
    public async Task<int> Query_without_tracking()
    {
        await using var dbContext = new LearningDbContext(_options);
        return await dbContext.Posts.AsNoTracking().CountAsync();
    }
}