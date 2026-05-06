using Microsoft.Data.SqlClient;

namespace SqlAcademy.Persistence.Infrastructure;

public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
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
        return connection;
    }
}