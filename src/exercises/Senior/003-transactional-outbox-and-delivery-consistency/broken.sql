USE LearningDb;
GO

-- Problem 1: this inserts a new outbox message every time and ignores idempotency.
INSERT INTO #Outbox (AggregateType, AggregateId, EventType, Payload, Status, CreatedUtc, DispatchedUtc)
SELECT
    N'Order',
    OrderId,
    N'OrderReadyToShip',
    CONCAT(N'{"orderNumber":"', OrderNumber, N'","status":"', Status, N'"}'),
    N'Pending',
    SYSUTCDATETIME(),
    NULL
FROM #OrdersReadyForDispatch
WHERE Status = N'ReadyToShip';
GO

-- Problem 2: this chooses dispatch work without a deterministic ordering.
SELECT TOP (2)
    OutboxId,
    AggregateId,
    CreatedUtc
FROM #Outbox
WHERE Status = N'Pending';
GO

-- Problem 3: this checks only message count and does not validate missing or duplicate messages per order.
SELECT COUNT(*) AS PendingMessageCount
FROM #Outbox
WHERE Status = N'Pending';
GO