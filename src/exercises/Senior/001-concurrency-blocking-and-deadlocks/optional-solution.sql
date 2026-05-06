USE LearningDb;
GO

-- One mitigation is to keep a consistent access order and reduce the time the transaction stays open.
BEGIN TRANSACTION;

UPDATE academy.Orders
SET UpdatedUtc = SYSUTCDATETIME()
WHERE Id = 1;

UPDATE academy.Trades
SET Price = Price + 0.0100
WHERE Id = 1;

COMMIT TRANSACTION;
GO