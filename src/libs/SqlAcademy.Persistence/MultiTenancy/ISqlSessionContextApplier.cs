using Microsoft.Data.SqlClient;

namespace SqlAcademy.Persistence.MultiTenancy;

public interface ISqlSessionContextApplier
{
    void Apply(SqlConnection connection);

    Task ApplyAsync(SqlConnection connection, CancellationToken cancellationToken);
}