using Dapper;
using SqlAcademy.Persistence.Diagnostics;
using SqlAcademy.Persistence.Infrastructure;
using SqlAcademy.SharedKernel.Pagination;
using SqlAcademy.SharedKernel.Sorting;

namespace SqlAcademy.Persistence.Queries.Trades;

public sealed class TradeReadService(ISqlConnectionFactory connectionFactory)
{
    public async Task<PagedResult<TradeListItem>> GetTradesAsync(TradeQueryRequest request, CancellationToken cancellationToken)
    {
        using var activity = PersistenceDiagnostics.ActivitySource.StartActivity("dapper.trades.list");
        activity?.SetTag("trades.page_size", request.NormalizedPageSize);
        activity?.SetTag("trades.sort_by", request.SortBy);

        var sortColumn = request.SortBy.Trim().ToLowerInvariant() switch
        {
            "price" => "t.Price",
            "quantity" => "t.Quantity",
            "symbol" => "i.Symbol",
            _ => "t.TradedUtc",
        };

        var sortDirection = request.SortDirection == SortDirection.Asc ? "ASC" : "DESC";

        const string whereClause = """
WHERE (@UserId IS NULL OR t.UserId = @UserId)
  AND (@InstrumentSymbol IS NULL OR i.Symbol = @InstrumentSymbol)
""";

        var sql = $"""
SELECT
    t.Id,
    u.UserName,
    i.Symbol AS InstrumentSymbol,
    t.Side,
    t.Quantity,
    t.Price,
    t.TradedUtc
FROM academy.Trades AS t
INNER JOIN academy.Users AS u ON u.Id = t.UserId
INNER JOIN academy.Instruments AS i ON i.Id = t.InstrumentId
{whereClause}
ORDER BY {sortColumn} {sortDirection}, t.Id DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

SELECT COUNT_BIG(1)
FROM academy.Trades AS t
INNER JOIN academy.Instruments AS i ON i.Id = t.InstrumentId
{whereClause};
""";

        var parameters = new
        {
            request.UserId,
            InstrumentSymbol = string.IsNullOrWhiteSpace(request.InstrumentSymbol)
                ? null
                : request.InstrumentSymbol.Trim().ToUpperInvariant(),
            Offset = request.Skip,
            PageSize = request.NormalizedPageSize,
        };

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        using var reader = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        var items = (await reader.ReadAsync<TradeListItem>()).AsList();
        var totalCount = await reader.ReadSingleAsync<long>();

        return new PagedResult<TradeListItem>(items, request.NormalizedPageNumber, request.NormalizedPageSize, totalCount);
    }
}