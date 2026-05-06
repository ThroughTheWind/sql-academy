using Microsoft.AspNetCore.Mvc;
using SqlAcademy.Persistence.Commands.Trades;
using SqlAcademy.Persistence.Queries.Trades;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Api.Controllers.V1;

[ApiController]
[Route("api/v1/trades")]
public sealed class TradesController(TradeReadService tradeReadService, TradeImportService tradeImportService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TradeListItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TradeListItem>>> GetTrades(
        [FromQuery] TradeQueryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await tradeReadService.GetTradesAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("import")]
    [ProducesResponseType(typeof(TradeImportResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TradeImportResult>> ImportTrades(
        [FromBody] ImportTradesRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Trades is null || request.Trades.Count == 0)
        {
            return BadRequest(CreateProblemDetails(StatusCodes.Status400BadRequest, "Invalid request", "Trades is required and must contain at least one row."));
        }

        var result = await tradeImportService.ImportTradesAsync(
            new ImportTradesCommand(request.Trades
                .Select(trade => new TradeImportRow(
                    trade.UserName,
                    trade.InstrumentSymbol,
                    trade.Side,
                    trade.Quantity,
                    trade.Price,
                    trade.TradedUtc))
                .ToArray()),
            cancellationToken);

        return Ok(result);
    }

    private ProblemDetails CreateProblemDetails(int statusCode, string title, string detail)
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = HttpContext.Request.Path,
        };
    }
}

public sealed record ImportTradesRequest(IReadOnlyList<ImportTradeRowRequest> Trades);

public sealed record ImportTradeRowRequest(
    string UserName,
    string InstrumentSymbol,
    string Side,
    decimal Quantity,
    decimal Price,
    DateTime TradedUtc);