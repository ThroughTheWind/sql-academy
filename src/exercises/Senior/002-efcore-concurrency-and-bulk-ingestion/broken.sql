USE LearningDb;
GO

-- Problem 1: no staging or validation boundary before writing directly to the OLTP table.
INSERT INTO academy.Trades (UserId, InstrumentId, Side, Quantity, Price, TradedUtc)
SELECT 1, 1, N'Buy', 1000.0000, 422.1000, SYSUTCDATETIME()
UNION ALL
SELECT 1, 1, N'Buy', 1000.0000, 422.1000, SYSUTCDATETIME();
GO

-- Problem 2: the rowversion is ignored entirely during an update review.
UPDATE academy.Orders
SET Status = N'Filled',
    UpdatedUtc = SYSUTCDATETIME()
WHERE Id = 2;
GO