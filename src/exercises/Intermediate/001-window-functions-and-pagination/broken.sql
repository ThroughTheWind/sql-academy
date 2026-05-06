USE LearningDb;
GO

-- Problem 1: ROW_NUMBER without a deterministic tiebreaker creates unstable pages.
WITH RankedTrades AS
(
    SELECT
        t.Id,
        t.UserId,
        ROW_NUMBER() OVER (ORDER BY t.TradedUtc DESC) AS RowNumber
    FROM academy.Trades AS t
)
SELECT *
FROM RankedTrades
WHERE RowNumber BETWEEN 1 AND 2;
GO

-- Problem 2: the running total never resets per user because the partition is missing.
SELECT
    t.Id,
    t.UserId,
    t.Quantity,
    SUM(t.Quantity) OVER (ORDER BY t.TradedUtc, t.Id) AS RunningQuantity
FROM academy.Trades AS t;
GO