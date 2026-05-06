USE LearningDb;
GO

-- Task 1: insert missing pending outbox messages for ready-to-ship orders.
-- Keep the insert idempotent and use the order CreatedUtc as the message CreatedUtc.

-- Task 2: create #dispatch_batch with:
-- DispatchSequence INT, OutboxId INT, AggregateId INT, CreatedUtc DATETIME2(3)
IF OBJECT_ID(N'tempdb..#dispatch_batch', N'U') IS NOT NULL
BEGIN
    DROP TABLE #dispatch_batch;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS DispatchSequence,
    CAST(NULL AS INT) AS OutboxId,
    CAST(NULL AS INT) AS AggregateId,
    CAST(NULL AS DATETIME2(3)) AS CreatedUtc
INTO #dispatch_batch;
GO

-- Task 3: create #delivery_consistency_check with:
-- ReadyOrderCount INT, PendingMessageCount INT, OrdersMissingPendingMessage INT, DuplicatePendingMessageCount INT
IF OBJECT_ID(N'tempdb..#delivery_consistency_check', N'U') IS NOT NULL
BEGIN
    DROP TABLE #delivery_consistency_check;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS ReadyOrderCount,
    CAST(NULL AS INT) AS PendingMessageCount,
    CAST(NULL AS INT) AS OrdersMissingPendingMessage,
    CAST(NULL AS INT) AS DuplicatePendingMessageCount
INTO #delivery_consistency_check;
GO