using Dapper;
using SqlAcademy.Persistence.Diagnostics;
using SqlAcademy.Persistence.Infrastructure;
using SqlAcademy.SharedKernel.Pagination;
using SqlAcademy.SharedKernel.Sorting;

namespace SqlAcademy.Persistence.Queries.TenantOrders;

public sealed class TenantOrderReadService(ISqlConnectionFactory connectionFactory)
{
    public async Task<PagedResult<TenantOrderListItem>> GetTenantOrdersAsync(
        TenantOrderQueryRequest request,
        CancellationToken cancellationToken)
    {
        using var activity = PersistenceDiagnostics.ActivitySource.StartActivity("dapper.tenant-orders.list");
        activity?.SetTag("tenant_orders.page_size", request.NormalizedPageSize);
        activity?.SetTag("tenant_orders.sort_by", request.SortBy);

        var sortColumn = request.SortBy.Trim().ToLowerInvariant() switch
        {
            "ordernumber" => "o.OrderNumber",
            "totalamount" => "o.TotalAmount",
            _ => "o.CreatedUtc",
        };

        var sortDirection = request.SortDirection == SortDirection.Asc ? "ASC" : "DESC";

        var sql = $"""
SELECT
    o.Id,
    o.TenantId,
    o.OrderNumber,
    o.Description,
    o.TotalAmount,
    o.CreatedUtc
FROM academy.TenantOrders AS o
ORDER BY {sortColumn} {sortDirection}, o.Id DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

SELECT COUNT_BIG(1)
FROM academy.TenantOrders AS o;
""";

        var parameters = new
        {
            Offset = request.Skip,
            PageSize = request.NormalizedPageSize,
        };

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        using var reader = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        var items = (await reader.ReadAsync<TenantOrderListItem>()).AsList();
        var totalCount = await reader.ReadSingleAsync<long>();

        return new PagedResult<TenantOrderListItem>(items, request.NormalizedPageNumber, request.NormalizedPageSize, totalCount);
    }
}