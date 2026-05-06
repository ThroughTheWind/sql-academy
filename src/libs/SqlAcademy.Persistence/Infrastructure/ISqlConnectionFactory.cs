using Microsoft.Data.SqlClient;

namespace SqlAcademy.Persistence.Infrastructure;

public interface ISqlConnectionFactory
{
    Task<SqlConnection> OpenConnectionAsync(CancellationToken cancellationToken);
}