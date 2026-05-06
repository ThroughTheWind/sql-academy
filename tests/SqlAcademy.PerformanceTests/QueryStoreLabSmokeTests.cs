using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using SqlAcademy.PerformanceTests.Testing;

namespace SqlAcademy.PerformanceTests;

[Collection(PerformanceTestCollection.Name)]
public sealed class QueryStoreLabSmokeTests(SqlServerFixture databaseFixture) : IAsyncLifetime
{
    [Fact]
    public async Task Advanced_004_starter_script_captures_tagged_queries_in_query_store()
    {
        var scriptPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Advanced",
            "004-query-store-and-regression-triage",
            "starter.sql");

        var scriptText = await File.ReadAllTextAsync(scriptPath);
        var batches = Regex.Split(scriptText, @"^\s*GO\s*(?:--.*)?$", RegexOptions.Multiline | RegexOptions.IgnoreCase)
            .Select(batch => batch.Trim())
            .Where(batch => !string.IsNullOrWhiteSpace(batch))
            .ToArray();

        await using var connection = new SqlConnection(databaseFixture.ConnectionString);
        await connection.OpenAsync();

        foreach (var batch in batches)
        {
            await using var batchCommand = new SqlCommand(batch, connection)
            {
                CommandTimeout = 30,
            };

            await batchCommand.ExecuteNonQueryAsync();
        }

        const string verificationSql = """
SELECT
    options.actual_state_desc,
    COUNT(DISTINCT CASE WHEN qt.query_sql_text LIKE N'%query_store_lab_trade_lookup%' THEN q.query_id END) AS TradeQueryCount,
    COUNT(DISTINCT CASE WHEN qt.query_sql_text LIKE N'%query_store_lab_post_search%' THEN q.query_id END) AS PostQueryCount
FROM sys.database_query_store_options AS options
LEFT JOIN sys.query_store_query_text AS qt ON 1 = 1
LEFT JOIN sys.query_store_query AS q ON q.query_text_id = qt.query_text_id
GROUP BY options.actual_state_desc;
""";

        await using var verificationCommand = new SqlCommand(verificationSql, connection)
        {
            CommandTimeout = 30,
        };

        await using var reader = await verificationCommand.ExecuteReaderAsync();
        Assert.True(await reader.ReadAsync());

        var actualState = reader.GetString(0);
        var tradeQueryCount = reader.GetInt32(1);
        var postQueryCount = reader.GetInt32(2);

        Assert.Equal("READ_WRITE", actualState);
        Assert.True(tradeQueryCount >= 1, "Expected Query Store to capture the tagged trade query.");
        Assert.True(postQueryCount >= 1, "Expected Query Store to capture the tagged post query.");
    }

    public async Task InitializeAsync()
    {
        await databaseFixture.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private static string ResolveRepositoryPath(params string[] relativeSegments)
    {
        var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);

        while (currentDirectory is not null)
        {
            var candidateSolutionPath = Path.Combine(currentDirectory.FullName, "SqlAcademy.slnx");

            if (File.Exists(candidateSolutionPath))
            {
                return Path.Combine([currentDirectory.FullName, ..relativeSegments]);
            }

            currentDirectory = currentDirectory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate the repository root from the performance test output directory.");
    }
}