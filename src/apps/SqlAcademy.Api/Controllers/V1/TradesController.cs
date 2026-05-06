using Microsoft.AspNetCore.Mvc;
using SqlAcademy.Persistence.Commands.Trades;
using SqlAcademy.Persistence.Queries.Trades;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Api.Controllers.V1;

[ApiController]
[Route("api/v1/trades")]
public sealed class TradesController(
    TradeReadService tradeReadService,
    TradeImportService tradeImportService,
    TradeImportBatchReadService tradeImportBatchReadService) : ControllerBase
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

    [HttpGet("import-batches")]
    [ProducesResponseType(typeof(PagedResult<TradeImportBatchListItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TradeImportBatchListItem>>> GetImportBatches(
        [FromQuery] TradeImportBatchQueryRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await tradeImportBatchReadService.GetBatchesAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("import-batches/{batchId:int}")]
    [ProducesResponseType(typeof(TradeImportBatchDetails), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TradeImportBatchDetails>> GetImportBatch(
        int batchId,
        CancellationToken cancellationToken)
    {
        var result = await tradeImportBatchReadService.GetBatchAsync(batchId, cancellationToken);

        if (result is null)
        {
            return NotFound(CreateProblemDetails(StatusCodes.Status404NotFound, "Batch not found", $"Trade import batch '{batchId}' was not found."));
        }

        return Ok(result);
    }

    [HttpGet("import-batches/{batchId:int}/rows")]
    [ProducesResponseType(typeof(PagedResult<TradeImportBatchRowListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagedResult<TradeImportBatchRowListItem>>> GetImportBatchRows(
        int batchId,
        [FromQuery] TradeImportBatchRowQueryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await tradeImportBatchReadService.GetBatchRowsAsync(batchId, request, cancellationToken);

        if (result is null)
        {
            return NotFound(CreateProblemDetails(StatusCodes.Status404NotFound, "Batch not found", $"Trade import batch '{batchId}' was not found."));
        }

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
                .ToArray(),
                request.DryRun),
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

public sealed record ImportTradesRequest(bool DryRun, IReadOnlyList<ImportTradeRowRequest> Trades);

public sealed record ImportTradeRowRequest(
    string UserName,
    string InstrumentSymbol,
    string Side,
    decimal Quantity,
    decimal Price,
    DateTime TradedUtc);