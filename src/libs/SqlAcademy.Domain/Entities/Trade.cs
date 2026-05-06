using SqlAcademy.Domain.Enums;

namespace SqlAcademy.Domain.Entities;

public sealed class Trade
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int InstrumentId { get; set; }

    public TradeSide Side { get; set; }

    public decimal Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime TradedUtc { get; set; }

    public User User { get; set; } = null!;

    public Instrument Instrument { get; set; } = null!;
}