USE LearningDb;
GO

SELECT
    u.UserName,
    COUNT(p.Id) AS PostCount,
    DENSE_RANK() OVER (ORDER BY COUNT(p.Id) DESC) AS PostRank
FROM academy.Users AS u
LEFT JOIN academy.Posts AS p ON p.UserId = u.Id
GROUP BY u.UserName;
GO

SELECT
    t.Id,
    t.UserId,
    t.TradedUtc,
    t.Quantity,
    SUM(t.Quantity) OVER (PARTITION BY t.UserId ORDER BY t.TradedUtc, t.Id) AS RunningQuantity
FROM academy.Trades AS t
ORDER BY t.UserId, t.TradedUtc, t.Id;
GO