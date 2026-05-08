using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;
using Serilog;
using SqlAcademy.Api.Infrastructure;
using SqlAcademy.Observability;
using SqlAcademy.Persistence;
using SqlAcademy.Persistence.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Keep the startup explicit so learners can see exactly where observability,
// persistence, health checks, and HTTP behavior enter the application.
builder.Services.AddSerilog((_, loggerConfiguration) =>
    loggerConfiguration.ApplySqlAcademyDefaults("SqlAcademy.Api"));

builder.Services.AddSqlAcademyObservability(
    builder.Configuration,
    "SqlAcademy.Api",
    PersistenceDiagnostics.ActivitySourceName);

builder.Services.AddSqlAcademyPersistence(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Applying migrations and seeds on startup keeps local runs deterministic and
// ensures the Docker stack and test host both begin from the same baseline.
await app.Services.InitializeLearningDatabaseAsync(app.Lifetime.ApplicationStopping);

app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseHttpMetrics();
app.UseMiddleware<TenantSessionContextMiddleware>();

app.MapOpenApi();
app.MapControllers();
app.MapMetrics("/metrics");
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false,
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = _ => true,
});

app.Run();

public partial class Program;
