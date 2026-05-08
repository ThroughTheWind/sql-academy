using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlAcademy.Persistence.Commands.Orders;
using SqlAcademy.Persistence.Commands.TenantOrders;
using SqlAcademy.Persistence.Commands.Trades;
using SqlAcademy.Persistence.Database;
using SqlAcademy.Persistence.Infrastructure;
using SqlAcademy.Persistence.Initialization;
using SqlAcademy.Persistence.MultiTenancy;
using SqlAcademy.Persistence.Queries.Posts;
using SqlAcademy.Persistence.Queries.TenantOrders;
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

        services.AddScoped<ISqlSessionContextAccessor, SqlSessionContextAccessor>();
        services.AddScoped<ISqlSessionContextApplier, SqlSessionContextApplier>();
        services.AddScoped<SqlSessionContextConnectionInterceptor>();

        services.AddDbContext<LearningDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly("SqlAcademy.Migrations");
                sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                sqlServerOptions.CommandTimeout(30);
            });

            options.EnableDetailedErrors();
            options.AddInterceptors(serviceProvider.GetRequiredService<SqlSessionContextConnectionInterceptor>());
        });

        services.AddScoped<ISqlConnectionFactory>(serviceProvider =>
            new SqlConnectionFactory(
                connectionString,
                serviceProvider.GetRequiredService<ISqlSessionContextApplier>()));
        services.AddScoped<LearningDbInitializer>();
        services.AddScoped<OrderWriteService>();
        services.AddScoped<TenantOrderWriteService>();
        services.AddScoped<TradeImportService>();
        services.AddScoped<PostReadService>();
        services.AddScoped<TenantOrderReadService>();
        services.AddScoped<TradeImportBatchReadService>();
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