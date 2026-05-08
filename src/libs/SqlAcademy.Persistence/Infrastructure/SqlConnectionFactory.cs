using Microsoft.Data.SqlClient;
using SqlAcademy.Persistence.MultiTenancy;

namespace SqlAcademy.Persistence.Infrastructure;

public sealed class SqlConnectionFactory(string connectionString, ISqlSessionContextApplier sessionContextApplier) : ISqlConnectionFactory
{
    public async Task<SqlConnection> OpenConnectionAsync(CancellationToken cancellationToken)
    {
        // Normalize the connection string so both EF Core and Dapper benefit from
        // the same SQL Server retry semantics and local development defaults.
        var builder = new SqlConnectionStringBuilder(connectionString)
        {
            Encrypt = false,
            TrustServerCertificate = true,
            ConnectRetryCount = 3,
            ConnectRetryInterval = 10,
        };

        var connection = new SqlConnection(builder.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await sessionContextApplier.ApplyAsync(connection, cancellationToken);
        return connection;
    }
}