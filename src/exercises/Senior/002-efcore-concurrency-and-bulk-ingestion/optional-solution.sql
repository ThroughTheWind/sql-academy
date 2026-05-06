USE LearningDb;
GO

CREATE TABLE #TradeImportStage
(
    UserName NVARCHAR(64) NOT NULL,
    InstrumentSymbol NVARCHAR(24) NOT NULL,
    Side NVARCHAR(16) NOT NULL,
    Quantity DECIMAL(18,4) NOT NULL,
    Price DECIMAL(18,4) NOT NULL,
    TradedUtc DATETIME2(3) NOT NULL,
    BatchId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID()
);
GO

-- Example concurrency-aware read for a later update pipeline.
SELECT Id, OrderNumber, Status, RowVersion
FROM academy.Orders
WHERE Id = 2;
GO