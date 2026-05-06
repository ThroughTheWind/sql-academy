using Serilog;
using SqlAcademy.Observability;
using SqlAcademy.Persistence;
using SqlAcademy.Persistence.Diagnostics;
using SqlAcademy.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((_, loggerConfiguration) =>
	loggerConfiguration.ApplySqlAcademyDefaults("SqlAcademy.Worker"));

builder.Services.AddSqlAcademyObservability(
	builder.Configuration,
	"SqlAcademy.Worker",
	PersistenceDiagnostics.ActivitySourceName);

builder.Services.AddSqlAcademyPersistence(builder.Configuration);
builder.Services.AddHostedService<DatabaseTelemetryWorker>();

var host = builder.Build();
await host.RunAsync();
