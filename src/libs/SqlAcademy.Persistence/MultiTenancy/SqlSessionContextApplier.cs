using Microsoft.Data.SqlClient;

namespace SqlAcademy.Persistence.MultiTenancy;

public sealed class SqlSessionContextApplier(ISqlSessionContextAccessor sessionContextAccessor) : ISqlSessionContextApplier
{
    public void Apply(SqlConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        using var command = CreateCommand(connection);
        command.ExecuteNonQuery();
    }

    public async Task ApplyAsync(SqlConnection connection, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(connection);

        using var command = CreateCommand(connection);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private SqlCommand CreateCommand(SqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"""
    EXEC sys.sp_set_session_context @key = N'{SqlSessionContextKeys.TenantId}', @value = @tenantId;
    EXEC sys.sp_set_session_context @key = N'{SqlSessionContextKeys.RlsBypass}', @value = @rlsBypass;
    """;

        command.Parameters.AddWithValue("@tenantId", sessionContextAccessor.TenantId.HasValue
            ? sessionContextAccessor.TenantId.Value
            : DBNull.Value);
        command.Parameters.AddWithValue("@rlsBypass", sessionContextAccessor.BypassRowLevelSecurity ? 1 : 0);

        return command;
    }
}