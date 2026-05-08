using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using SqlAcademy.PerformanceTests.Testing;

namespace SqlAcademy.PerformanceTests;

[Collection(PerformanceTestCollection.Name)]
public sealed class PlanCacheLabSmokeTests(SqlServerFixture databaseFixture) : IAsyncLifetime
{
    [Fact]
    public async Task Advanced_003_starter_script_captures_tagged_query_and_storage_snapshot()
    {
        var scriptPath = ResolveRepositoryPath(
            "src",
            "exercises",
            "Advanced",
            "003-plan-cache-memory-grants-and-waits",
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
    COUNT_BIG(DISTINCT CASE WHEN st.text LIKE N'%internals_lab_trade_filter%' THEN qs.plan_handle END) AS TaggedPlanCount,
    MAX(CASE WHEN st.text LIKE N'%internals_lab_trade_filter%' THEN qs.execution_count END) AS TaggedExecutionCount,
    COUNT_BIG(DISTINCT CASE WHEN ps.object_id = OBJECT_ID(N'academy.Posts') AND ps.index_id IN (0, 1) THEN ps.object_id END) AS PostsSnapshotCount,
    COUNT_BIG(DISTINCT CASE WHEN ps.object_id = OBJECT_ID(N'academy.Comments') AND ps.index_id IN (0, 1) THEN ps.object_id END) AS CommentsSnapshotCount,
    COUNT_BIG(DISTINCT CASE WHEN ps.object_id = OBJECT_ID(N'academy.Trades') AND ps.index_id IN (0, 1) THEN ps.object_id END) AS TradesSnapshotCount
FROM sys.dm_exec_query_stats AS qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) AS st
CROSS JOIN sys.dm_db_partition_stats AS ps;
""";

        await using var verificationCommand = new SqlCommand(verificationSql, connection)
        {
            CommandTimeout = 30,
        };

        await using var reader = await verificationCommand.ExecuteReaderAsync();
        Assert.True(await reader.ReadAsync());

        var taggedPlanCount = reader.GetInt64(0);
        var taggedExecutionCount = reader.IsDBNull(1) ? 0L : reader.GetInt64(1);
        var postsSnapshotCount = reader.GetInt64(2);
        var commentsSnapshotCount = reader.GetInt64(3);
        var tradesSnapshotCount = reader.GetInt64(4);

        Assert.True(taggedPlanCount >= 1, "Expected the starter script to leave the tagged plan-cache query visible.");
        Assert.True(taggedExecutionCount >= 2, "Expected the tagged query to run for more than one parameter value.");
        Assert.Equal(1, postsSnapshotCount);
        Assert.Equal(1, commentsSnapshotCount);
        Assert.Equal(1, tradesSnapshotCount);
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