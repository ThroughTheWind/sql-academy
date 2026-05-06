USE LearningDb;
GO

SELECT TOP (10)
    Id,
    OrderNumber,
    Status,
    RowVersion,
    UpdatedUtc
FROM academy.Orders
ORDER BY UpdatedUtc DESC;
GO

-- Sketch a staging table for bulk ingestion review.
CREATE TABLE #TradeImportStage
(
    UserName NVARCHAR(64) NOT NULL,
    InstrumentSymbol NVARCHAR(24) NOT NULL,
    Side NVARCHAR(16) NOT NULL,
    Quantity DECIMAL(18,4) NOT NULL,
    Price DECIMAL(18,4) NOT NULL,
    TradedUtc DATETIME2(3) NOT NULL
);
GO