USE LearningDb;
GO

INSERT INTO #Outbox (AggregateType, AggregateId, EventType, Payload, Status, CreatedUtc, DispatchedUtc)
SELECT
    N'Order',
    o.OrderId,
    N'OrderReadyToShip',
    CONCAT(N'{"orderNumber":"', o.OrderNumber, N'","status":"', o.Status, N'"}'),
    N'Pending',
    o.CreatedUtc,
    NULL
FROM #OrdersReadyForDispatch AS o
WHERE o.Status = N'ReadyToShip'
  AND NOT EXISTS
  (
      SELECT 1
      FROM #Outbox AS ob
      WHERE ob.AggregateType = N'Order'
        AND ob.AggregateId = o.OrderId
        AND ob.EventType = N'OrderReadyToShip'
        AND ob.Status = N'Pending'
  );
GO

IF OBJECT_ID(N'tempdb..#dispatch_batch', N'U') IS NOT NULL DROP TABLE #dispatch_batch;

WITH OrderedPendingMessages AS
(
    SELECT
        OutboxId,
        AggregateId,
        CreatedUtc,
        ROW_NUMBER() OVER (ORDER BY CreatedUtc, OutboxId) AS DispatchSequence
    FROM #Outbox
    WHERE Status = N'Pending'
)
SELECT DispatchSequence, OutboxId, AggregateId, CreatedUtc
INTO #dispatch_batch
FROM OrderedPendingMessages
WHERE DispatchSequence <= 2;
GO

IF OBJECT_ID(N'tempdb..#delivery_consistency_check', N'U') IS NOT NULL DROP TABLE #delivery_consistency_check;

WITH ReadyOrders AS
(
    SELECT OrderId
    FROM #OrdersReadyForDispatch
    WHERE Status = N'ReadyToShip'
),
PendingMessages AS
(
    SELECT AggregateId
    FROM #Outbox
    WHERE AggregateType = N'Order'
      AND EventType = N'OrderReadyToShip'
      AND Status = N'Pending'
)
SELECT
    (SELECT COUNT(*) FROM ReadyOrders) AS ReadyOrderCount,
    (SELECT COUNT(*) FROM PendingMessages) AS PendingMessageCount,
    (SELECT COUNT(*) FROM ReadyOrders AS ro WHERE NOT EXISTS (SELECT 1 FROM PendingMessages AS pm WHERE pm.AggregateId = ro.OrderId)) AS OrdersMissingPendingMessage,
    (SELECT COUNT(*)
     FROM (
         SELECT AggregateId
         FROM PendingMessages
         GROUP BY AggregateId
         HAVING COUNT(*) > 1
     ) AS duplicates) AS DuplicatePendingMessageCount
INTO #delivery_consistency_check;
GO