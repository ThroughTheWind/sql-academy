using SqlAcademy.Domain.Enums;

namespace SqlAcademy.Domain.Entities;

public sealed class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public required string OrderNumber { get; set; }

    public OrderStatus Status { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime UpdatedUtc { get; set; }

    public byte[] RowVersion { get; set; } = [];

    public User User { get; set; } = null!;
}