using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SqlAcademy.Persistence.MultiTenancy;

public sealed class SqlSessionContextConnectionInterceptor(ISqlSessionContextApplier sessionContextApplier) : DbConnectionInterceptor
{
    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        ApplySessionContext(connection);
        base.ConnectionOpened(connection, eventData);
    }

    public override async Task ConnectionOpenedAsync(
        DbConnection connection,
        ConnectionEndEventData eventData,
        CancellationToken cancellationToken = default)
    {
        await ApplySessionContextAsync(connection, cancellationToken);
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    private void ApplySessionContext(DbConnection connection)
    {
        if (connection is SqlConnection sqlConnection)
        {
            sessionContextApplier.Apply(sqlConnection);
        }
    }

    private Task ApplySessionContextAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        if (connection is SqlConnection sqlConnection)
        {
            return sessionContextApplier.ApplyAsync(sqlConnection, cancellationToken);
        }

        return Task.CompletedTask;
    }
}