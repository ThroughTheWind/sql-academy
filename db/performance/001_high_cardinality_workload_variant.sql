:setvar DatabaseName "LearningDb"
:setvar SyntheticUserCount "64"
:setvar PostsPerUser "96"
:setvar MaxCommentsPerPost "6"
:setvar TradeDays "45"
:setvar TradesPerUserPerDay "12"

USE [$(DatabaseName)];
GO

SET NOCOUNT ON;
GO

DECLARE @SyntheticUserCount INT = TRY_CAST('$(SyntheticUserCount)' AS INT);
DECLARE @PostsPerUser INT = TRY_CAST('$(PostsPerUser)' AS INT);
DECLARE @MaxCommentsPerPost INT = TRY_CAST('$(MaxCommentsPerPost)' AS INT);
DECLARE @TradeDays INT = TRY_CAST('$(TradeDays)' AS INT);
DECLARE @TradesPerUserPerDay INT = TRY_CAST('$(TradesPerUserPerDay)' AS INT);

IF @SyntheticUserCount IS NULL OR @SyntheticUserCount < 1
BEGIN
    THROW 51000, 'SyntheticUserCount must be a positive integer.', 1;
END;

IF @PostsPerUser IS NULL OR @PostsPerUser < 1
BEGIN
    THROW 51001, 'PostsPerUser must be a positive integer.', 1;
END;

IF @MaxCommentsPerPost IS NULL OR @MaxCommentsPerPost < 1
BEGIN
    THROW 51002, 'MaxCommentsPerPost must be a positive integer.', 1;
END;

IF @TradeDays IS NULL OR @TradeDays < 1
BEGIN
    THROW 51003, 'TradeDays must be a positive integer.', 1;
END;

IF @TradesPerUserPerDay IS NULL OR @TradesPerUserPerDay < 1
BEGIN
    THROW 51004, 'TradesPerUserPerDay must be a positive integer.', 1;
END;

IF NOT EXISTS (SELECT 1 FROM academy.Users WHERE UserName = N'ada')
BEGIN
    THROW 51005, 'Baseline seed data is missing. Run the base seed scripts or infra/scripts/init-database.sh before applying this workload variant.', 1;
END;

DECLARE @MsftId INT = (SELECT Id FROM academy.Instruments WHERE Symbol = N'MSFT');
DECLARE @AaplId INT = (SELECT Id FROM academy.Instruments WHERE Symbol = N'AAPL');
DECLARE @EurUsdId INT = (SELECT Id FROM academy.Instruments WHERE Symbol = N'EURUSD');
DECLARE @ClId INT = (SELECT Id FROM academy.Instruments WHERE Symbol = N'CL');

IF @MsftId IS NULL OR @AaplId IS NULL OR @EurUsdId IS NULL OR @ClId IS NULL
BEGIN
    THROW 51006, 'Baseline instruments are missing. Run the reference seed before applying this workload variant.', 1;
END;

DECLARE @BasePostCreatedUtc DATETIME2(3) = '2025-02-01T08:00:00.000';
DECLARE @TradeStartUtc DATETIME2(3) = '2025-03-01T09:30:00.000';

PRINT CONCAT('Applying high-cardinality workload variant with ', @SyntheticUserCount, ' users, ', @PostsPerUser, ' posts per user, up to ', @MaxCommentsPerPost, ' comments per post, ', @TradeDays, ' trade days, and ', @TradesPerUserPerDay, ' trades per user per day.');

DROP TABLE IF EXISTS #SyntheticUsers;
CREATE TABLE #SyntheticUsers
(
    UserOrdinal INT NOT NULL PRIMARY KEY,
    UserId INT NOT NULL,
    UserName NVARCHAR(64) NOT NULL
);

;WITH UserNumbers AS
(
    SELECT TOP (@SyntheticUserCount)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
    FROM sys.all_objects AS a
    CROSS JOIN sys.all_objects AS b
)
INSERT INTO academy.Users (UserName, Email, CreatedUtc)
SELECT
    payload.UserName,
    payload.Email,
    DATEADD(MINUTE, UserNumbers.Number - 1, @BasePostCreatedUtc)
FROM UserNumbers
CROSS APPLY
(
    VALUES
    (
        CONCAT(N'perf_user_', RIGHT(CONCAT(REPLICATE(N'0', 5), CONVERT(VARCHAR(10), UserNumbers.Number)), 5)),
        CONCAT(N'perf_user_', RIGHT(CONCAT(REPLICATE(N'0', 5), CONVERT(VARCHAR(10), UserNumbers.Number)), 5), N'@sqlacademy.local')
    )
) AS payload(UserName, Email)
WHERE NOT EXISTS
(
    SELECT 1
    FROM academy.Users AS existingUsers
    WHERE existingUsers.UserName = payload.UserName
);

;WITH UserNumbers AS
(
    SELECT TOP (@SyntheticUserCount)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
    FROM sys.all_objects AS a
    CROSS JOIN sys.all_objects AS b
)
INSERT INTO #SyntheticUsers (UserOrdinal, UserId, UserName)
SELECT
    UserNumbers.Number,
    users.Id,
    users.UserName
FROM UserNumbers
INNER JOIN academy.Users AS users
    ON users.UserName = CONCAT(N'perf_user_', RIGHT(CONCAT(REPLICATE(N'0', 5), CONVERT(VARCHAR(10), UserNumbers.Number)), 5));

DROP TABLE IF EXISTS #SyntheticPosts;
CREATE TABLE #SyntheticPosts
(
    UserOrdinal INT NOT NULL,
    PostOrdinal INT NOT NULL,
    PostId INT NOT NULL,
    PostCreatedUtc DATETIME2(3) NOT NULL,
    CONSTRAINT PK_SyntheticPosts PRIMARY KEY (UserOrdinal, PostOrdinal)
);

;WITH PostNumbers AS
(
    SELECT TOP (@PostsPerUser)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
    FROM sys.all_objects AS a
)
INSERT INTO academy.Posts (UserId, Title, Body, CreatedUtc)
SELECT
    syntheticUsers.UserId,
    payload.Title,
    payload.Body,
    payload.CreatedUtc
FROM #SyntheticUsers AS syntheticUsers
CROSS JOIN PostNumbers
CROSS APPLY
(
    VALUES
    (
        CONCAT(
            N'Performance workload post ',
            RIGHT(CONCAT(REPLICATE(N'0', 5), CONVERT(VARCHAR(10), syntheticUsers.UserOrdinal)), 5),
            N'-',
            RIGHT(CONCAT(REPLICATE(N'0', 4), CONVERT(VARCHAR(10), PostNumbers.Number)), 4)),
        CONCAT(
            N'Synthetic workload row for indexing, statistics, plan cache, Query Store, and EF Core tracking comparisons. Topic=',
            CASE (PostNumbers.Number - 1) % 4
                WHEN 0 THEN N'plan stability'
                WHEN 1 THEN N'statistics skew'
                WHEN 2 THEN N'pagination drift'
                ELSE N'generated SQL'
            END,
            N'; user bucket=',
            CONVERT(VARCHAR(10), syntheticUsers.UserOrdinal % 8),
            N'; post bucket=',
            CONVERT(VARCHAR(10), PostNumbers.Number % 12)),
        DATEADD(MINUTE, ((syntheticUsers.UserOrdinal - 1) * @PostsPerUser) + PostNumbers.Number - 1, @BasePostCreatedUtc)
    )
) AS payload(Title, Body, CreatedUtc)
WHERE NOT EXISTS
(
    SELECT 1
    FROM academy.Posts AS existingPosts
    WHERE existingPosts.Title = payload.Title
);

;WITH PostNumbers AS
(
    SELECT TOP (@PostsPerUser)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
    FROM sys.all_objects AS a
)
INSERT INTO #SyntheticPosts (UserOrdinal, PostOrdinal, PostId, PostCreatedUtc)
SELECT
    syntheticUsers.UserOrdinal,
    PostNumbers.Number,
    posts.Id,
    posts.CreatedUtc
FROM #SyntheticUsers AS syntheticUsers
CROSS JOIN PostNumbers
CROSS APPLY
(
    VALUES
    (
        CONCAT(
            N'Performance workload post ',
            RIGHT(CONCAT(REPLICATE(N'0', 5), CONVERT(VARCHAR(10), syntheticUsers.UserOrdinal)), 5),
            N'-',
            RIGHT(CONCAT(REPLICATE(N'0', 4), CONVERT(VARCHAR(10), PostNumbers.Number)), 4))
    )
) AS payload(Title)
INNER JOIN academy.Posts AS posts
    ON posts.Title = payload.Title;

;WITH CommentNumbers AS
(
    SELECT TOP (@MaxCommentsPerPost)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
    FROM sys.all_objects AS a
)
INSERT INTO academy.Comments (PostId, UserId, Body, CreatedUtc)
SELECT
    syntheticPosts.PostId,
    commentAuthors.UserId,
    payload.Body,
    payload.CreatedUtc
FROM #SyntheticPosts AS syntheticPosts
CROSS JOIN CommentNumbers
INNER JOIN #SyntheticUsers AS commentAuthors
    ON commentAuthors.UserOrdinal = ((syntheticPosts.UserOrdinal + CommentNumbers.Number - 2) % @SyntheticUserCount) + 1
CROSS APPLY
(
    VALUES (((syntheticPosts.PostOrdinal + syntheticPosts.UserOrdinal) % @MaxCommentsPerPost) + 1)
) AS limits(CommentTarget)
CROSS APPLY
(
    VALUES
    (
        CONCAT(
            N'Performance workload comment ',
            RIGHT(CONCAT(REPLICATE(N'0', 5), CONVERT(VARCHAR(10), syntheticPosts.UserOrdinal)), 5),
            N'-',
            RIGHT(CONCAT(REPLICATE(N'0', 4), CONVERT(VARCHAR(10), syntheticPosts.PostOrdinal)), 4),
            N'-',
            RIGHT(CONCAT(REPLICATE(N'0', 2), CONVERT(VARCHAR(10), CommentNumbers.Number)), 2)),
        DATEADD(MINUTE, CommentNumbers.Number * 3, syntheticPosts.PostCreatedUtc)
    )
) AS payload(Body, CreatedUtc)
WHERE CommentNumbers.Number <= limits.CommentTarget
  AND NOT EXISTS
  (
      SELECT 1
      FROM academy.Comments AS existingComments
      WHERE existingComments.PostId = syntheticPosts.PostId
        AND existingComments.Body = payload.Body
  );

;WITH TradeDayNumbers AS
(
    SELECT TOP (@TradeDays)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
    FROM sys.all_objects AS a
),
TradeSlotNumbers AS
(
    SELECT TOP (@TradesPerUserPerDay)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Number
    FROM sys.all_objects AS a
)
INSERT INTO academy.Trades (UserId, InstrumentId, Side, Quantity, Price, TradedUtc)
SELECT
    syntheticUsers.UserId,
    tradeBase.InstrumentId,
    tradeBase.Side,
    tradeShape.Quantity,
    tradeShape.Price,
    tradeBase.TradedUtc
FROM #SyntheticUsers AS syntheticUsers
CROSS JOIN TradeDayNumbers
CROSS JOIN TradeSlotNumbers
CROSS APPLY
(
    VALUES
    (
        CASE
            WHEN ((TradeSlotNumbers.Number - 1) % 12) < 8 THEN @MsftId
            WHEN ((TradeSlotNumbers.Number - 1) % 12) < 10 THEN @AaplId
            WHEN ((TradeSlotNumbers.Number - 1) % 12) = 10 THEN @EurUsdId
            ELSE @ClId
        END,
        CASE
            WHEN (syntheticUsers.UserOrdinal + TradeDayNumbers.Number + TradeSlotNumbers.Number) % 4 = 0 THEN N'Sell'
            ELSE N'Buy'
        END,
        DATEADD(
            MINUTE,
            ((TradeDayNumbers.Number - 1) * 1440) + ((TradeSlotNumbers.Number - 1) * 30) + ((syntheticUsers.UserOrdinal - 1) % 17),
            @TradeStartUtc)
    )
) AS tradeBase(InstrumentId, Side, TradedUtc)
CROSS APPLY
(
    VALUES
    (
        CASE tradeBase.InstrumentId
            WHEN @EurUsdId THEN CAST(10000 + ((syntheticUsers.UserOrdinal * 9 + TradeDayNumbers.Number * 7 + TradeSlotNumbers.Number * 5) % 40) * 1000 AS DECIMAL(18,4))
            WHEN @ClId THEN CAST(1 + ((syntheticUsers.UserOrdinal + TradeDayNumbers.Number + TradeSlotNumbers.Number) % 9) AS DECIMAL(18,4))
            ELSE CAST(25 + ((syntheticUsers.UserOrdinal * 17 + TradeDayNumbers.Number * 3 + TradeSlotNumbers.Number * 11) % 600) AS DECIMAL(18,4))
        END,
        CAST(
            CASE tradeBase.InstrumentId
                WHEN @MsftId THEN 380.0000 + (((syntheticUsers.UserOrdinal + TradeDayNumbers.Number + TradeSlotNumbers.Number) % 90) * 0.4700)
                WHEN @AaplId THEN 165.0000 + (((syntheticUsers.UserOrdinal + TradeDayNumbers.Number + TradeSlotNumbers.Number) % 70) * 0.3100)
                WHEN @EurUsdId THEN 1.0500 + (((syntheticUsers.UserOrdinal * 5 + TradeDayNumbers.Number + TradeSlotNumbers.Number) % 180) * 0.0004)
                ELSE 68.0000 + (((syntheticUsers.UserOrdinal + TradeDayNumbers.Number + TradeSlotNumbers.Number) % 45) * 0.2200)
            END
            AS DECIMAL(18,4))
    )
) AS tradeShape(Quantity, Price)
WHERE NOT EXISTS
(
    SELECT 1
    FROM academy.Trades AS existingTrades
    WHERE existingTrades.UserId = syntheticUsers.UserId
      AND existingTrades.InstrumentId = tradeBase.InstrumentId
      AND existingTrades.TradedUtc = tradeBase.TradedUtc
);

EXEC sys.sp_updatestats;

SELECT
    summary.TableName,
    summary.TotalRows,
    summary.SyntheticRows
FROM
(
    SELECT
        N'academy.Users' AS TableName,
        COUNT_BIG(*) AS TotalRows,
        SUM(CASE WHEN users.UserName LIKE N'perf_user_%' THEN 1 ELSE 0 END) AS SyntheticRows
    FROM academy.Users AS users

    UNION ALL

    SELECT
        N'academy.Posts',
        COUNT_BIG(*),
        SUM(CASE WHEN posts.Title LIKE N'Performance workload post %' THEN 1 ELSE 0 END)
    FROM academy.Posts AS posts

    UNION ALL

    SELECT
        N'academy.Comments',
        COUNT_BIG(*),
        SUM(CASE WHEN comments.Body LIKE N'Performance workload comment %' THEN 1 ELSE 0 END)
    FROM academy.Comments AS comments

    UNION ALL

    SELECT
        N'academy.Trades',
        COUNT_BIG(*),
        SUM(CASE WHEN EXISTS (SELECT 1 FROM #SyntheticUsers AS syntheticUsers WHERE syntheticUsers.UserId = trades.UserId) THEN 1 ELSE 0 END)
    FROM academy.Trades AS trades
) AS summary
ORDER BY summary.TableName;