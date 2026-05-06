namespace SqlAcademy.Persistence.Queries.Trades;

public sealed record TradeListItem(
    int Id,
    string UserName,
    string InstrumentSymbol,
    string Side,
    decimal Quantity,
    decimal Price,
    DateTime TradedUtc);