using Microsoft.AspNetCore.Mvc;
using SqlAcademy.Persistence.Queries.TenantOrders;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Api.Controllers.V1;

[ApiController]
[Route("api/v1/tenant-orders")]
public sealed class TenantOrdersController(TenantOrderReadService tenantOrderReadService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TenantOrderListItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TenantOrderListItem>>> GetTenantOrders(
        [FromQuery] TenantOrderQueryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await tenantOrderReadService.GetTenantOrdersAsync(request, cancellationToken);
        return Ok(result);
    }
}