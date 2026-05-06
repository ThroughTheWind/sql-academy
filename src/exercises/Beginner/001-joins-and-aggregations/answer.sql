USE LearningDb;
GO

-- Task 1 and 3: create #post_comment_summary with:
-- SortRank INT, PostId INT, UserName NVARCHAR(64), Title NVARCHAR(256), CommentCount INT, CreatedUtc DATETIME2(3)
IF OBJECT_ID(N'tempdb..#post_comment_summary', N'U') IS NOT NULL
BEGIN
    DROP TABLE #post_comment_summary;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS SortRank,
    CAST(NULL AS INT) AS PostId,
    CAST(NULL AS NVARCHAR(64)) AS UserName,
    CAST(NULL AS NVARCHAR(256)) AS Title,
    CAST(NULL AS INT) AS CommentCount,
    CAST(NULL AS DATETIME2(3)) AS CreatedUtc
INTO #post_comment_summary;
GO

-- Task 2: create #user_post_comment_totals with:
-- UserName NVARCHAR(64), PostCount INT, CommentCount INT
IF OBJECT_ID(N'tempdb..#user_post_comment_totals', N'U') IS NOT NULL
BEGIN
    DROP TABLE #user_post_comment_totals;
END;

SELECT TOP (0)
    CAST(NULL AS NVARCHAR(64)) AS UserName,
    CAST(NULL AS INT) AS PostCount,
    CAST(NULL AS INT) AS CommentCount
INTO #user_post_comment_totals;
GO