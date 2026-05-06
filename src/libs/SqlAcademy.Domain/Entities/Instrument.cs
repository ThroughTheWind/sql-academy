using SqlAcademy.Domain.Enums;

namespace SqlAcademy.Domain.Entities;

public sealed class Instrument
{
    public int Id { get; set; }

    public required string Symbol { get; set; }

    public required string Name { get; set; }

    public AssetClass AssetClass { get; set; }

    public decimal TickSize { get; set; }

    public decimal LotSize { get; set; }

    public DateTime CreatedUtc { get; set; }

    public List<Trade> Trades { get; } = [];
}