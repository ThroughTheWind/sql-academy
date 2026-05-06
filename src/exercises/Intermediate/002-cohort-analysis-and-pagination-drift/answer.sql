USE LearningDb;
GO

-- Task 1: create #cohort_summary with:
-- CohortMonth DATE, UserCount INT, OrderingUserCount INT
IF OBJECT_ID(N'tempdb..#cohort_summary', N'U') IS NOT NULL
BEGIN
    DROP TABLE #cohort_summary;
END;

SELECT TOP (0)
    CAST(NULL AS DATE) AS CohortMonth,
    CAST(NULL AS INT) AS UserCount,
    CAST(NULL AS INT) AS OrderingUserCount
INTO #cohort_summary;
GO

-- Task 2: create #stable_trade_page with:
-- RowNum INT, TradeId INT, UserId INT, TradedUtc DATETIME2(3), Quantity DECIMAL(18,4)
IF OBJECT_ID(N'tempdb..#stable_trade_page', N'U') IS NOT NULL
BEGIN
    DROP TABLE #stable_trade_page;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS RowNum,
    CAST(NULL AS INT) AS TradeId,
    CAST(NULL AS INT) AS UserId,
    CAST(NULL AS DATETIME2(3)) AS TradedUtc,
    CAST(NULL AS DECIMAL(18, 4)) AS Quantity
INTO #stable_trade_page;
GO

-- Task 3: create #running_trade_quantity with:
-- TradeId INT, UserId INT, RunningQuantity DECIMAL(18,4)
IF OBJECT_ID(N'tempdb..#running_trade_quantity', N'U') IS NOT NULL
BEGIN
    DROP TABLE #running_trade_quantity;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS TradeId,
    CAST(NULL AS INT) AS UserId,
    CAST(NULL AS DECIMAL(18, 4)) AS RunningQuantity
INTO #running_trade_quantity;
GO