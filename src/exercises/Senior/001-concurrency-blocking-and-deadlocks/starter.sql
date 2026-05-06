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

IF OBJECT_ID(N'tempdb..#deadlock_steps', N'U') IS NOT NULL
BEGIN
    DROP TABLE #deadlock_steps;
END;
GO

-- The validation workflow runs in one SQL session, so this fixture captures the
-- lock acquisition order from the manual two-session scenario above.
CREATE TABLE #deadlock_steps
(
    SessionLabel NVARCHAR(16) NOT NULL,
    StepNumber INT NOT NULL,
    ResourceName NVARCHAR(64) NOT NULL,
    OperationName NVARCHAR(16) NOT NULL
);
GO

INSERT INTO #deadlock_steps (SessionLabel, StepNumber, ResourceName, OperationName)
VALUES
    (N'SessionA', 1, N'academy.Orders', N'UPDATE'),
    (N'SessionA', 2, N'academy.Trades', N'UPDATE'),
    (N'SessionB', 1, N'academy.Trades', N'UPDATE'),
    (N'SessionB', 2, N'academy.Orders', N'UPDATE');
GO