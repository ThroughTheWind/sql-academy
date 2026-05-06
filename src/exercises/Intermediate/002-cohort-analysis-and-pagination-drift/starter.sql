USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#TradeFeed', N'U') IS NOT NULL
BEGIN
    DROP TABLE #TradeFeed;
END;
GO

CREATE TABLE #TradeFeed
(
    TradeId INT NOT NULL PRIMARY KEY,
    UserId INT NOT NULL,
    TradedUtc DATETIME2(3) NOT NULL,
    Quantity DECIMAL(18, 4) NOT NULL
);
GO

INSERT INTO #TradeFeed (TradeId, UserId, TradedUtc, Quantity)
VALUES
    (10, 1, '2025-01-20T09:00:00.000', 5.0000),
    (11, 1, '2025-01-20T09:00:00.000', 7.0000),
    (12, 1, '2025-01-20T09:10:00.000', 3.0000),
    (20, 2, '2025-01-20T09:05:00.000', 4.0000),
    (21, 2, '2025-01-20T09:15:00.000', 6.0000);
GO