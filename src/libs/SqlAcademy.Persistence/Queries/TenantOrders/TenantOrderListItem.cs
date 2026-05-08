namespace SqlAcademy.Persistence.Queries.TenantOrders;

public sealed record TenantOrderListItem(
    int Id,
    int TenantId,
    string OrderNumber,
    string Description,
    decimal TotalAmount,
    DateTime CreatedUtc);