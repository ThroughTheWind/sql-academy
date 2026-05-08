using SqlAcademy.Domain.Entities;
using SqlAcademy.Persistence.Database;

namespace SqlAcademy.Persistence.Commands.TenantOrders;

public sealed record CreateTenantOrderCommand(int TenantId, string OrderNumber, string Description, decimal TotalAmount);

public sealed record TenantOrderWriteResult(int Id, int TenantId, string OrderNumber, string Description, decimal TotalAmount, DateTime CreatedUtc);

public sealed class TenantOrderWriteService(LearningDbContext dbContext)
{
    public async Task<TenantOrderWriteResult> CreateTenantOrderAsync(
        CreateTenantOrderCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.OrderNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Description);

        if (command.TenantId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(command.TenantId), "TenantId must be greater than zero.");
        }

        if (command.TotalAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(command.TotalAmount), "TotalAmount must be greater than zero.");
        }

        var tenantOrder = new TenantOrder
        {
            TenantId = command.TenantId,
            OrderNumber = command.OrderNumber.Trim(),
            Description = command.Description.Trim(),
            TotalAmount = command.TotalAmount,
            CreatedUtc = DateTime.UtcNow,
        };

        await dbContext.TenantOrders.AddAsync(tenantOrder, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new TenantOrderWriteResult(
            tenantOrder.Id,
            tenantOrder.TenantId,
            tenantOrder.OrderNumber,
            tenantOrder.Description,
            tenantOrder.TotalAmount,
            tenantOrder.CreatedUtc);
    }
}