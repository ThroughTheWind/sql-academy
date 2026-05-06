USE LearningDb;
GO

-- Session A
BEGIN TRANSACTION;

UPDATE academy.Orders
SET Status = N'Submitted',
    UpdatedUtc = SYSUTCDATETIME()
WHERE OrderNumber = N'ORD-2025-0002';

-- Keep the transaction open while Session B starts.
SELECT * FROM academy.Orders WHERE OrderNumber = N'ORD-2025-0002';

ROLLBACK TRANSACTION;
GO

-- Session B
BEGIN TRANSACTION;

UPDATE academy.Trades
SET Price = Price + 0.0100
WHERE Id = 1;

ROLLBACK TRANSACTION;
GO