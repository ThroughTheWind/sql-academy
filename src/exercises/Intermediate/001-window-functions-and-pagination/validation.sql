USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#post_volume_ranks', N'U') IS NULL
BEGIN
    THROW 51000, '#post_volume_ranks was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#running_trade_totals', N'U') IS NULL
BEGIN
    THROW 51000, '#running_trade_totals was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#stable_trade_page', N'U') IS NULL
BEGIN
    THROW 51000, '#stable_trade_page was not created.', 1;
END;

WITH PostCounts AS
(
    SELECT
        u.Id AS UserId,
        u.UserName,
        COUNT(p.Id) AS PostCount
    FROM academy.Users AS u
    LEFT JOIN academy.Posts AS p ON p.UserId = u.Id
    GROUP BY u.Id, u.UserName
),
ExpectedPostRanks AS
(
    SELECT
        ROW_NUMBER() OVER (ORDER BY PostCount DESC, UserId ASC) AS DisplayOrder,
        UserId,
        UserName,
        PostCount,
        DENSE_RANK() OVER (ORDER BY PostCount DESC) AS PostRank
    FROM PostCounts
)
IF EXISTS
(
    SELECT DisplayOrder, UserId, UserName, PostCount, PostRank FROM #post_volume_ranks
    EXCEPT
    SELECT DisplayOrder, UserId, UserName, PostCount, PostRank FROM ExpectedPostRanks
)
OR EXISTS
(
    SELECT DisplayOrder, UserId, UserName, PostCount, PostRank FROM ExpectedPostRanks
    EXCEPT
    SELECT DisplayOrder, UserId, UserName, PostCount, PostRank FROM #post_volume_ranks
)
BEGIN
    THROW 51000, '#post_volume_ranks does not match the expected ranking output.', 1;
END;

WITH ExpectedRunningTotals AS
(
    SELECT
        t.Id AS TradeId,
        t.UserId,
        t.Quantity,
        SUM(t.Quantity) OVER (PARTITION BY t.UserId ORDER BY t.TradedUtc, t.Id) AS RunningQuantity
    FROM academy.Trades AS t
)
IF EXISTS
(
    SELECT TradeId, UserId, Quantity, RunningQuantity FROM #running_trade_totals
    EXCEPT
    SELECT TradeId, UserId, Quantity, RunningQuantity FROM ExpectedRunningTotals
)
OR EXISTS
(
    SELECT TradeId, UserId, Quantity, RunningQuantity FROM ExpectedRunningTotals
    EXCEPT
    SELECT TradeId, UserId, Quantity, RunningQuantity FROM #running_trade_totals
)
BEGIN
    THROW 51000, '#running_trade_totals does not match the expected running totals.', 1;
END;

WITH ExpectedTradePage AS
(
    SELECT
        ROW_NUMBER() OVER (ORDER BY t.TradedUtc DESC, t.Id DESC) AS RowNumber,
        t.Id AS TradeId,
        t.UserId,
        t.TradedUtc
    FROM academy.Trades AS t
),
ExpectedFirstPage AS
(
    SELECT RowNumber, TradeId, UserId, TradedUtc
    FROM ExpectedTradePage
    WHERE RowNumber BETWEEN 1 AND 2
)
IF EXISTS
(
    SELECT RowNumber, TradeId, UserId, TradedUtc FROM #stable_trade_page
    EXCEPT
    SELECT RowNumber, TradeId, UserId, TradedUtc FROM ExpectedFirstPage
)
OR EXISTS
(
    SELECT RowNumber, TradeId, UserId, TradedUtc FROM ExpectedFirstPage
    EXCEPT
    SELECT RowNumber, TradeId, UserId, TradedUtc FROM #stable_trade_page
)
BEGIN
    THROW 51000, '#stable_trade_page does not match the expected deterministic trade page.', 1;
END;

PRINT N'Validation succeeded for Intermediate 001.';
GO