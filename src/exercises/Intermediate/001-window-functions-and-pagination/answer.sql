USE LearningDb;
GO

-- Task 1: create #post_volume_ranks with:
-- DisplayOrder INT, UserId INT, UserName NVARCHAR(64), PostCount INT, PostRank INT
IF OBJECT_ID(N'tempdb..#post_volume_ranks', N'U') IS NOT NULL
BEGIN
    DROP TABLE #post_volume_ranks;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS DisplayOrder,
    CAST(NULL AS INT) AS UserId,
    CAST(NULL AS NVARCHAR(64)) AS UserName,
    CAST(NULL AS INT) AS PostCount,
    CAST(NULL AS INT) AS PostRank
INTO #post_volume_ranks;
GO

-- Task 2: create #running_trade_totals with:
-- TradeId INT, UserId INT, Quantity DECIMAL(18,4), RunningQuantity DECIMAL(18,4)
IF OBJECT_ID(N'tempdb..#running_trade_totals', N'U') IS NOT NULL
BEGIN
    DROP TABLE #running_trade_totals;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS TradeId,
    CAST(NULL AS INT) AS UserId,
    CAST(NULL AS DECIMAL(18, 4)) AS Quantity,
    CAST(NULL AS DECIMAL(18, 4)) AS RunningQuantity
INTO #running_trade_totals;
GO

-- Task 3: create #stable_trade_page with:
-- RowNumber INT, TradeId INT, UserId INT, TradedUtc DATETIME2(3)
IF OBJECT_ID(N'tempdb..#stable_trade_page', N'U') IS NOT NULL
BEGIN
    DROP TABLE #stable_trade_page;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS RowNumber,
    CAST(NULL AS INT) AS TradeId,
    CAST(NULL AS INT) AS UserId,
    CAST(NULL AS DATETIME2(3)) AS TradedUtc
INTO #stable_trade_page;
GO