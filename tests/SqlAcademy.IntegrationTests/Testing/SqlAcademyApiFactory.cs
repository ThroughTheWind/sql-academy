using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace SqlAcademy.IntegrationTests.Testing;

public sealed class SqlAcademyApiFactory(SqlServerFixture databaseFixture) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LearningDb"] = databaseFixture.ConnectionString,
                ["OpenTelemetry:Endpoint"] = "http://localhost:4317",
            });
        });
    }
}