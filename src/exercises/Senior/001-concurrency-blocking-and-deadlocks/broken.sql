USE LearningDb;
GO

-- Session A
BEGIN TRANSACTION;

UPDATE academy.Orders
SET UpdatedUtc = SYSUTCDATETIME()
WHERE Id = 1;

UPDATE academy.Trades
SET Price = Price + 0.0100
WHERE Id = 1;

COMMIT TRANSACTION;
GO

-- Session B
BEGIN TRANSACTION;

UPDATE academy.Trades
SET Price = Price + 0.0200
WHERE Id = 1;

UPDATE academy.Orders
SET UpdatedUtc = SYSUTCDATETIME()
WHERE Id = 1;

COMMIT TRANSACTION;
GO