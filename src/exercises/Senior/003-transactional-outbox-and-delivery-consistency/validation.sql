USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#dispatch_batch', N'U') IS NULL
BEGIN
    THROW 51000, '#dispatch_batch was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#delivery_consistency_check', N'U') IS NULL
BEGIN
    THROW 51000, '#delivery_consistency_check was not created.', 1;
END;

WITH ExpectedPendingMessages AS
(
    SELECT 1 AS AggregateId, N'{"orderNumber":"ORD-2025-1001","status":"ReadyToShip"}' AS Payload, CAST('2025-02-01T10:00:00.000' AS DATETIME2(3)) AS CreatedUtc
    UNION ALL
    SELECT 2, N'{"orderNumber":"ORD-2025-1002","status":"ReadyToShip"}', CAST('2025-02-01T10:05:00.000' AS DATETIME2(3))
    UNION ALL
    SELECT 4, N'{"orderNumber":"ORD-2025-1004","status":"ReadyToShip"}', CAST('2025-02-01T10:20:00.000' AS DATETIME2(3))
)
IF EXISTS
(
    SELECT AggregateId, Payload, CreatedUtc
    FROM #Outbox
    WHERE EventType = N'OrderReadyToShip' AND Status = N'Pending'
    EXCEPT
    SELECT AggregateId, Payload, CreatedUtc FROM ExpectedPendingMessages
)
OR EXISTS
(
    SELECT AggregateId, Payload, CreatedUtc FROM ExpectedPendingMessages
    EXCEPT
    SELECT AggregateId, Payload, CreatedUtc
    FROM #Outbox
    WHERE EventType = N'OrderReadyToShip' AND Status = N'Pending'
)
BEGIN
    THROW 51000, '#Outbox does not contain the expected idempotent pending messages.', 1;
END;

WITH ExpectedDispatchBatch AS
(
    SELECT 1 AS DispatchSequence, 101 AS OutboxId, 1 AS AggregateId, CAST('2025-02-01T10:00:00.000' AS DATETIME2(3)) AS CreatedUtc
    UNION ALL
    SELECT 2, 102, 2, CAST('2025-02-01T10:05:00.000' AS DATETIME2(3))
)
IF EXISTS
(
    SELECT DispatchSequence, OutboxId, AggregateId, CreatedUtc FROM #dispatch_batch
    EXCEPT
    SELECT DispatchSequence, OutboxId, AggregateId, CreatedUtc FROM ExpectedDispatchBatch
)
OR EXISTS
(
    SELECT DispatchSequence, OutboxId, AggregateId, CreatedUtc FROM ExpectedDispatchBatch
    EXCEPT
    SELECT DispatchSequence, OutboxId, AggregateId, CreatedUtc FROM #dispatch_batch
)
BEGIN
    THROW 51000, '#dispatch_batch is not deterministic or is missing the correct messages.', 1;
END;

WITH ExpectedConsistencyCheck AS
(
    SELECT 3 AS ReadyOrderCount, 3 AS PendingMessageCount, 0 AS OrdersMissingPendingMessage, 0 AS DuplicatePendingMessageCount
)
IF EXISTS
(
    SELECT ReadyOrderCount, PendingMessageCount, OrdersMissingPendingMessage, DuplicatePendingMessageCount FROM #delivery_consistency_check
    EXCEPT
    SELECT ReadyOrderCount, PendingMessageCount, OrdersMissingPendingMessage, DuplicatePendingMessageCount FROM ExpectedConsistencyCheck
)
OR EXISTS
(
    SELECT ReadyOrderCount, PendingMessageCount, OrdersMissingPendingMessage, DuplicatePendingMessageCount FROM ExpectedConsistencyCheck
    EXCEPT
    SELECT ReadyOrderCount, PendingMessageCount, OrdersMissingPendingMessage, DuplicatePendingMessageCount FROM #delivery_consistency_check
)
BEGIN
    THROW 51000, '#delivery_consistency_check does not prove the expected outbox state.', 1;
END;

PRINT N'Validation succeeded for Senior 003.';
GO