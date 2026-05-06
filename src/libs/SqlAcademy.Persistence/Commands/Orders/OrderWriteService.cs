using Microsoft.EntityFrameworkCore;
using SqlAcademy.Domain.Enums;
using SqlAcademy.Persistence.Database;

namespace SqlAcademy.Persistence.Commands.Orders;

public sealed record UpdateOrderStatusCommand(string OrderNumber, OrderStatus Status, string RowVersionHex);

public sealed record OrderStatusUpdateResult(string OrderNumber, string Status, string RowVersionHex, DateTime UpdatedUtc);

public sealed class OrderWriteService(LearningDbContext dbContext)
{
    public async Task<OrderStatusUpdateResult?> UpdateOrderStatusAsync(UpdateOrderStatusCommand command, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.OrderNumber);

        var order = await dbContext.Orders
            .SingleOrDefaultAsync(existingOrder => existingOrder.OrderNumber == command.OrderNumber, cancellationToken);

        if (order is null)
        {
            return null;
        }

        dbContext.Entry(order)
            .Property(existingOrder => existingOrder.RowVersion)
            .OriginalValue = ParseRowVersionHex(command.RowVersionHex);

        order.Status = command.Status;
        order.UpdatedUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new OrderStatusUpdateResult(
            order.OrderNumber,
            order.Status.ToString(),
            ToRowVersionHex(order.RowVersion),
            order.UpdatedUtc);
    }

    private static byte[] ParseRowVersionHex(string rowVersionHex)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rowVersionHex);

        var normalizedRowVersion = rowVersionHex.Trim();

        if (normalizedRowVersion.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            normalizedRowVersion = normalizedRowVersion[2..];
        }

        if (normalizedRowVersion.Length == 0 || normalizedRowVersion.Length % 2 != 0)
        {
            throw new ArgumentException("RowVersionHex must be a non-empty hexadecimal string.", nameof(rowVersionHex));
        }

        try
        {
            return Convert.FromHexString(normalizedRowVersion);
        }
        catch (FormatException exception)
        {
            throw new ArgumentException("RowVersionHex must be a valid hexadecimal string.", nameof(rowVersionHex), exception);
        }
    }

    private static string ToRowVersionHex(byte[] rowVersion)
    {
        return $"0x{Convert.ToHexString(rowVersion)}";
    }
}