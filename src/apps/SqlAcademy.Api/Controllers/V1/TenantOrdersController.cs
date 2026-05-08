using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlAcademy.Persistence.Commands.TenantOrders;
using SqlAcademy.Persistence.Queries.TenantOrders;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Api.Controllers.V1;

[ApiController]
[Route("api/v1/tenant-orders")]
public sealed class TenantOrdersController(
    TenantOrderReadService tenantOrderReadService,
    TenantOrderWriteService tenantOrderWriteService) : ControllerBase
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

    [HttpPost]
    [ProducesResponseType(typeof(TenantOrderWriteResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TenantOrderWriteResult>> CreateTenantOrder(
        [FromBody] CreateTenantOrderRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.OrderNumber))
        {
            return BadRequest(CreateProblemDetails(StatusCodes.Status400BadRequest, "Invalid request", "OrderNumber is required."));
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return BadRequest(CreateProblemDetails(StatusCodes.Status400BadRequest, "Invalid request", "Description is required."));
        }

        if (request.TenantId <= 0)
        {
            return BadRequest(CreateProblemDetails(StatusCodes.Status400BadRequest, "Invalid request", "TenantId must be greater than zero."));
        }

        if (request.TotalAmount <= 0)
        {
            return BadRequest(CreateProblemDetails(StatusCodes.Status400BadRequest, "Invalid request", "TotalAmount must be greater than zero."));
        }

        try
        {
            var result = await tenantOrderWriteService.CreateTenantOrderAsync(
                new CreateTenantOrderCommand(request.TenantId, request.OrderNumber, request.Description, request.TotalAmount),
                cancellationToken);

            return Created($"/api/v1/tenant-orders/{result.Id}", result);
        }
        catch (DbUpdateException exception) when (IsBlockedByRowLevelSecurity(exception))
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                CreateProblemDetails(
                    StatusCodes.Status403Forbidden,
                    "Write blocked by security policy",
                    "The SQL Server block predicate rejected the write because the tenant context did not authorize the requested row."));
        }
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

    private static bool IsBlockedByRowLevelSecurity(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current.Message.Contains("block predicate", StringComparison.OrdinalIgnoreCase) ||
                current.Message.Contains("security policy", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}

public sealed record CreateTenantOrderRequest(int TenantId, string OrderNumber, string Description, decimal TotalAmount);