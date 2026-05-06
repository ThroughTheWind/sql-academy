USE LearningDb;
GO

WITH RankedTrades AS
(
    SELECT
        t.Id,
        t.UserId,
        t.TradedUtc,
        ROW_NUMBER() OVER (ORDER BY t.TradedUtc DESC, t.Id DESC) AS RowNumber
    FROM academy.Trades AS t
)
SELECT *
FROM RankedTrades
WHERE RowNumber BETWEEN 1 AND 2
ORDER BY RowNumber;
GO

SELECT
    t.Id,
    t.UserId,
    t.Quantity,
    SUM(t.Quantity) OVER (PARTITION BY t.UserId ORDER BY t.TradedUtc, t.Id) AS RunningQuantity
FROM academy.Trades AS t
ORDER BY t.UserId, t.TradedUtc, t.Id;
GO