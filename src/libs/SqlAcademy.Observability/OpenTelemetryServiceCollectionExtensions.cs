using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace SqlAcademy.Observability;

public static class OpenTelemetryServiceCollectionExtensions
{
    public static IServiceCollection AddSqlAcademyObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName,
        params string[] activitySources)
    {
        var endpoint = configuration["OpenTelemetry:Endpoint"];
        var environmentName = configuration["ASPNETCORE_ENVIRONMENT"]
            ?? configuration["DOTNET_ENVIRONMENT"]
            ?? "Development";

        var openTelemetry = services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName)
                .AddAttributes([
                    new KeyValuePair<string, object>("deployment.environment", environmentName),
                ]));

        openTelemetry.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();

            if (activitySources.Length > 0)
            {
                tracing.AddSource(activitySources);
            }

            if (!string.IsNullOrWhiteSpace(endpoint))
            {
                tracing.AddOtlpExporter(options => options.Endpoint = new Uri(endpoint));
            }
        });

        openTelemetry.WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddHttpClientInstrumentation();
            metrics.AddRuntimeInstrumentation();

            if (!string.IsNullOrWhiteSpace(endpoint))
            {
                metrics.AddOtlpExporter(options => options.Endpoint = new Uri(endpoint));
            }
        });

        return services;
    }
}