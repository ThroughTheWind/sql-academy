USE LearningDb;
GO

-- Problem 1: this cohort summary counts order rows, not ordering users.
SELECT
    DATEFROMPARTS(YEAR(u.CreatedUtc), MONTH(u.CreatedUtc), 1) AS CohortMonth,
    COUNT(*) AS UserCount,
    COUNT(o.Id) AS OrderingUserCount
FROM academy.Users AS u
LEFT JOIN academy.Orders AS o ON o.UserId = u.Id
GROUP BY DATEFROMPARTS(YEAR(u.CreatedUtc), MONTH(u.CreatedUtc), 1);
GO

-- Problem 2: missing a tiebreaker makes page boundaries unstable.
WITH NumberedTrades AS
(
    SELECT
        TradeId,
        UserId,
        TradedUtc,
        Quantity,
        ROW_NUMBER() OVER (ORDER BY TradedUtc DESC) AS RowNum
    FROM #TradeFeed
)
SELECT *
FROM NumberedTrades
WHERE RowNum BETWEEN 1 AND 3;
GO

-- Problem 3: missing PARTITION BY mixes users into one cumulative total.
SELECT
    TradeId,
    UserId,
    SUM(Quantity) OVER (ORDER BY TradedUtc, TradeId) AS RunningQuantity
FROM #TradeFeed;
GO