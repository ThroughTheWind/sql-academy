USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#post_comment_summary', N'U') IS NULL
BEGIN
    THROW 51000, '#post_comment_summary was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#user_post_comment_totals', N'U') IS NULL
BEGIN
    THROW 51000, '#user_post_comment_totals was not created.', 1;
END;

WITH ExpectedPostSummary AS
(
    SELECT
        ROW_NUMBER() OVER (ORDER BY p.CreatedUtc DESC, p.Id DESC) AS SortRank,
        p.Id AS PostId,
        u.UserName,
        p.Title,
        COUNT(c.Id) AS CommentCount,
        p.CreatedUtc
    FROM academy.Users AS u
    INNER JOIN academy.Posts AS p ON p.UserId = u.Id
    LEFT JOIN academy.Comments AS c ON c.PostId = p.Id
    GROUP BY p.Id, u.UserName, p.Title, p.CreatedUtc
)
IF EXISTS
(
    SELECT SortRank, PostId, UserName, Title, CommentCount, CreatedUtc FROM #post_comment_summary
    EXCEPT
    SELECT SortRank, PostId, UserName, Title, CommentCount, CreatedUtc FROM ExpectedPostSummary
)
OR EXISTS
(
    SELECT SortRank, PostId, UserName, Title, CommentCount, CreatedUtc FROM ExpectedPostSummary
    EXCEPT
    SELECT SortRank, PostId, UserName, Title, CommentCount, CreatedUtc FROM #post_comment_summary
)
BEGIN
    THROW 51000, '#post_comment_summary does not match the expected post-level join and aggregation result.', 1;
END;

WITH ExpectedUserTotals AS
(
    SELECT
        u.UserName,
        COUNT(DISTINCT p.Id) AS PostCount,
        COUNT(c.Id) AS CommentCount
    FROM academy.Users AS u
    LEFT JOIN academy.Posts AS p ON p.UserId = u.Id
    LEFT JOIN academy.Comments AS c ON c.PostId = p.Id
    GROUP BY u.UserName
)
IF EXISTS
(
    SELECT UserName, PostCount, CommentCount FROM #user_post_comment_totals
    EXCEPT
    SELECT UserName, PostCount, CommentCount FROM ExpectedUserTotals
)
OR EXISTS
(
    SELECT UserName, PostCount, CommentCount FROM ExpectedUserTotals
    EXCEPT
    SELECT UserName, PostCount, CommentCount FROM #user_post_comment_totals
)
BEGIN
    THROW 51000, '#user_post_comment_totals does not match the expected per-user aggregate counts.', 1;
END;

PRINT N'Validation succeeded for Beginner 001.';
GO