using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlAcademy.Domain.Enums;
using SqlAcademy.Persistence.Commands.Orders;

namespace SqlAcademy.Api.Controllers.V1;

[ApiController]
[Route("api/v1/orders")]
public sealed class OrdersController(OrderWriteService orderWriteService) : ControllerBase
{
    [HttpPut("{orderNumber}/status")]
    [ProducesResponseType(typeof(OrderStatusUpdateResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<OrderStatusUpdateResult>> UpdateOrderStatus(
        string orderNumber,
        [FromBody] UpdateOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Status))
        {
            return BadRequest(CreateProblemDetails(StatusCodes.Status400BadRequest, "Invalid request", "Status is required."));
        }

        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
        {
            return BadRequest(CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Invalid request",
                $"Status '{request.Status}' is not supported."));
        }

        try
        {
            var result = await orderWriteService.UpdateOrderStatusAsync(
                new UpdateOrderStatusCommand(orderNumber, status, request.RowVersionHex),
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(CreateProblemDetails(
                StatusCodes.Status409Conflict,
                "Concurrency conflict",
                $"Order '{orderNumber}' was changed by another writer. Refresh the rowversion and retry."));
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
}

public sealed record UpdateOrderStatusRequest(string Status, string RowVersionHex);