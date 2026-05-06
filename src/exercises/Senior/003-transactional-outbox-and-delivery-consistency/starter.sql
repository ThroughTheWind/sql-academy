USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#OrdersReadyForDispatch', N'U') IS NOT NULL
BEGIN
    DROP TABLE #OrdersReadyForDispatch;
END;

IF OBJECT_ID(N'tempdb..#Outbox', N'U') IS NOT NULL
BEGIN
    DROP TABLE #Outbox;
END;
GO

CREATE TABLE #OrdersReadyForDispatch
(
    OrderId INT NOT NULL PRIMARY KEY,
    OrderNumber NVARCHAR(32) NOT NULL,
    Status NVARCHAR(32) NOT NULL,
    CreatedUtc DATETIME2(3) NOT NULL
);

CREATE TABLE #Outbox
(
    OutboxId INT IDENTITY(100, 1) NOT NULL PRIMARY KEY,
    AggregateType NVARCHAR(64) NOT NULL,
    AggregateId INT NOT NULL,
    EventType NVARCHAR(64) NOT NULL,
    Payload NVARCHAR(400) NOT NULL,
    Status NVARCHAR(32) NOT NULL,
    CreatedUtc DATETIME2(3) NOT NULL,
    DispatchedUtc DATETIME2(3) NULL
);
GO

INSERT INTO #OrdersReadyForDispatch (OrderId, OrderNumber, Status, CreatedUtc)
VALUES
    (1, N'ORD-2025-1001', N'ReadyToShip', '2025-02-01T10:00:00.000'),
    (2, N'ORD-2025-1002', N'ReadyToShip', '2025-02-01T10:05:00.000'),
    (3, N'ORD-2025-1003', N'Pending', '2025-02-01T10:10:00.000'),
    (4, N'ORD-2025-1004', N'ReadyToShip', '2025-02-01T10:15:00.000');

INSERT INTO #Outbox (AggregateType, AggregateId, EventType, Payload, Status, CreatedUtc, DispatchedUtc)
VALUES
    (N'Order', 4, N'OrderReadyToShip', N'{"orderNumber":"ORD-2025-1004","status":"ReadyToShip"}', N'Pending', '2025-02-01T10:20:00.000', NULL);
GO