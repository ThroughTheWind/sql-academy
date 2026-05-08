namespace SqlAcademy.Domain.Entities;

public sealed class TenantOrder
{
    public int Id { get; set; }

    public int TenantId { get; set; }

    public required string OrderNumber { get; set; }

    public required string Description { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedUtc { get; set; }
}