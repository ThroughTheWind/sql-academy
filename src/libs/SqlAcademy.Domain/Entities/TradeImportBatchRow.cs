namespace SqlAcademy.Domain.Entities;

public sealed class TradeImportBatchRow
{
    public int Id { get; set; }

    public int TradeImportBatchId { get; set; }

    public int RowNumber { get; set; }

    public string Outcome { get; set; } = string.Empty;

    public string Stage { get; set; } = string.Empty;

    public string? Code { get; set; }

    public string? Reason { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string InstrumentSymbol { get; set; } = string.Empty;

    public string Side { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime TradedUtc { get; set; }

    public int? TradeId { get; set; }

    public TradeImportBatch Batch { get; set; } = null!;
}