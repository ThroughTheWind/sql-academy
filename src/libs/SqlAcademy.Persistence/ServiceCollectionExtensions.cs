using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlAcademy.Persistence.Database;
using SqlAcademy.Persistence.Infrastructure;
using SqlAcademy.Persistence.Initialization;
using SqlAcademy.Persistence.Queries.Posts;
using SqlAcademy.Persistence.Queries.Trades;

namespace SqlAcademy.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlAcademyPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LearningDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ConnectionStrings:LearningDb must be configured before the application starts.");
        }

        services.AddDbContext<LearningDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly("SqlAcademy.Migrations");
                sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                sqlServerOptions.CommandTimeout(30);
            });

            options.EnableDetailedErrors();
        });

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        services.AddScoped<LearningDbInitializer>();
        services.AddScoped<PostReadService>();
        services.AddScoped<TradeReadService>();
        services.AddHealthChecks()
            .AddDbContextCheck<LearningDbContext>("learning-db");

        return services;
    }

    public static async Task InitializeLearningDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken)
    {
        await using var scope = services.CreateAsyncScope();
        var initializer = scope.ServiceProvider.GetRequiredService<LearningDbInitializer>();
        await initializer.InitializeAsync(cancellationToken);
    }
}