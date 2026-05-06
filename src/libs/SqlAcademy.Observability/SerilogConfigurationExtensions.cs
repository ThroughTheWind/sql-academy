using Serilog;
using Serilog.Events;

namespace SqlAcademy.Observability;

public static class SerilogConfigurationExtensions
{
    public static LoggerConfiguration ApplySqlAcademyDefaults(this LoggerConfiguration loggerConfiguration, string serviceName)
    {
        return loggerConfiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProcessId()
            .Enrich.WithProperty("Service", serviceName)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Service} {SourceContext} {Message:lj}{NewLine}{Exception}");
    }
}