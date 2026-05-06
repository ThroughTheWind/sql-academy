USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#cohort_summary', N'U') IS NOT NULL DROP TABLE #cohort_summary;

SELECT
    DATEFROMPARTS(YEAR(u.CreatedUtc), MONTH(u.CreatedUtc), 1) AS CohortMonth,
    COUNT(*) AS UserCount,
    COUNT(DISTINCT CASE WHEN o.Id IS NOT NULL THEN u.Id END) AS OrderingUserCount
INTO #cohort_summary
FROM academy.Users AS u
LEFT JOIN academy.Orders AS o ON o.UserId = u.Id
GROUP BY DATEFROMPARTS(YEAR(u.CreatedUtc), MONTH(u.CreatedUtc), 1);
GO

IF OBJECT_ID(N'tempdb..#stable_trade_page', N'U') IS NOT NULL DROP TABLE #stable_trade_page;

WITH NumberedTrades AS
(
    SELECT
        TradeId,
        UserId,
        TradedUtc,
        Quantity,
        ROW_NUMBER() OVER (ORDER BY TradedUtc DESC, TradeId DESC) AS RowNum
    FROM #TradeFeed
)
SELECT RowNum, TradeId, UserId, TradedUtc, Quantity
INTO #stable_trade_page
FROM NumberedTrades
WHERE RowNum BETWEEN 1 AND 3;
GO

IF OBJECT_ID(N'tempdb..#running_trade_quantity', N'U') IS NOT NULL DROP TABLE #running_trade_quantity;

SELECT
    TradeId,
    UserId,
    SUM(Quantity) OVER (PARTITION BY UserId ORDER BY TradedUtc, TradeId) AS RunningQuantity
INTO #running_trade_quantity
FROM #TradeFeed;
GO