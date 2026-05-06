USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#cohort_summary', N'U') IS NULL
BEGIN
    THROW 51000, '#cohort_summary was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#stable_trade_page', N'U') IS NULL
BEGIN
    THROW 51000, '#stable_trade_page was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#running_trade_quantity', N'U') IS NULL
BEGIN
    THROW 51000, '#running_trade_quantity was not created.', 1;
END;

WITH ExpectedCohortSummary AS
(
    SELECT CAST('2025-01-01' AS DATE) AS CohortMonth, 4 AS UserCount, 3 AS OrderingUserCount
)
IF EXISTS
(
    SELECT CohortMonth, UserCount, OrderingUserCount FROM #cohort_summary
    EXCEPT
    SELECT CohortMonth, UserCount, OrderingUserCount FROM ExpectedCohortSummary
)
OR EXISTS
(
    SELECT CohortMonth, UserCount, OrderingUserCount FROM ExpectedCohortSummary
    EXCEPT
    SELECT CohortMonth, UserCount, OrderingUserCount FROM #cohort_summary
)
BEGIN
    THROW 51000, '#cohort_summary is not correct for the seeded cohort data.', 1;
END;

WITH ExpectedStableTradePage AS
(
    SELECT 1 AS RowNum, 21 AS TradeId, 2 AS UserId, CAST('2025-01-20T09:15:00.000' AS DATETIME2(3)) AS TradedUtc, CAST(6.0000 AS DECIMAL(18, 4)) AS Quantity
    UNION ALL
    SELECT 2, 12, 1, CAST('2025-01-20T09:10:00.000' AS DATETIME2(3)), CAST(3.0000 AS DECIMAL(18, 4))
    UNION ALL
    SELECT 3, 20, 2, CAST('2025-01-20T09:05:00.000' AS DATETIME2(3)), CAST(4.0000 AS DECIMAL(18, 4))
)
IF EXISTS
(
    SELECT RowNum, TradeId, UserId, TradedUtc, Quantity FROM #stable_trade_page
    EXCEPT
    SELECT RowNum, TradeId, UserId, TradedUtc, Quantity FROM ExpectedStableTradePage
)
OR EXISTS
(
    SELECT RowNum, TradeId, UserId, TradedUtc, Quantity FROM ExpectedStableTradePage
    EXCEPT
    SELECT RowNum, TradeId, UserId, TradedUtc, Quantity FROM #stable_trade_page
)
BEGIN
    THROW 51000, '#stable_trade_page is not using the expected deterministic ordering.', 1;
END;

WITH ExpectedRunningQuantities AS
(
    SELECT 10 AS TradeId, 1 AS UserId, CAST(5.0000 AS DECIMAL(18, 4)) AS RunningQuantity
    UNION ALL
    SELECT 11, 1, CAST(12.0000 AS DECIMAL(18, 4))
    UNION ALL
    SELECT 12, 1, CAST(15.0000 AS DECIMAL(18, 4))
    UNION ALL
    SELECT 20, 2, CAST(4.0000 AS DECIMAL(18, 4))
    UNION ALL
    SELECT 21, 2, CAST(10.0000 AS DECIMAL(18, 4))
)
IF EXISTS
(
    SELECT TradeId, UserId, RunningQuantity FROM #running_trade_quantity
    EXCEPT
    SELECT TradeId, UserId, RunningQuantity FROM ExpectedRunningQuantities
)
OR EXISTS
(
    SELECT TradeId, UserId, RunningQuantity FROM ExpectedRunningQuantities
    EXCEPT
    SELECT TradeId, UserId, RunningQuantity FROM #running_trade_quantity
)
BEGIN
    THROW 51000, '#running_trade_quantity does not compute the expected user-level running totals.', 1;
END;

PRINT N'Validation succeeded for Intermediate 002.';
GO