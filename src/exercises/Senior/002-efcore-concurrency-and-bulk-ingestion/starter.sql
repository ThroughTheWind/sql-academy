USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#TradeImportStage', N'U') IS NOT NULL
BEGIN
    DROP TABLE #TradeImportStage;
END;

CREATE TABLE #TradeImportStage
(
    BatchRowId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserName NVARCHAR(64) NOT NULL,
    InstrumentSymbol NVARCHAR(24) NOT NULL,
    Side NVARCHAR(16) NOT NULL,
    Quantity DECIMAL(18,4) NOT NULL,
    Price DECIMAL(18,4) NOT NULL,
    TradedUtc DATETIME2(3) NOT NULL
);
GO

INSERT INTO #TradeImportStage (UserName, InstrumentSymbol, Side, Quantity, Price, TradedUtc)
VALUES
    (N'ada', N'MSFT', N'Buy', 1000.0000, 422.1000, '2025-02-03T09:00:00.000'),
    (N'ada', N'MSFT', N'Buy', 1000.0000, 422.1000, '2025-02-03T09:00:00.000'),
    (N'grace', N'AAPL', N'Sell', 25.0000, 187.2000, '2025-02-03T09:05:00.000'),
    (N'linus', N'EURUSD', N'Sell', 25000.0000, 1.0842, '2025-02-03T09:10:00.000');
GO

SELECT
    Id,
    OrderNumber,
    Status,
    CONVERT(NVARCHAR(34), sys.fn_varbintohexstr(RowVersion)) AS RowVersionHex,
    UpdatedUtc
FROM academy.Orders
WHERE OrderNumber = N'ORD-2025-0002';
GO

SELECT *
FROM #TradeImportStage
ORDER BY BatchRowId;
GO