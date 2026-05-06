using Microsoft.AspNetCore.Mvc;
using SqlAcademy.Persistence.Queries.Trades;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Api.Controllers.V1;

[ApiController]
[Route("api/v1/trades")]
public sealed class TradesController(TradeReadService tradeReadService) : ControllerBase
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
}