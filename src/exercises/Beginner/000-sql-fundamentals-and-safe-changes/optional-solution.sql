USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#lesson01_user_directory', N'U') IS NOT NULL
BEGIN
    DROP TABLE #lesson01_user_directory;
END;

SELECT
    ROW_NUMBER() OVER (ORDER BY u.CreatedUtc, u.Id) AS DisplayOrder,
    u.Id AS UserId,
    u.UserName,
    u.Email,
    u.CreatedUtc
INTO #lesson01_user_directory
FROM academy.Users AS u;
GO

IF OBJECT_ID(N'tempdb..#lesson01_recent_actionable_orders', N'U') IS NOT NULL
BEGIN
    DROP TABLE #lesson01_recent_actionable_orders;
END;

WITH RankedOrders AS
(
    SELECT
        ROW_NUMBER() OVER (ORDER BY o.UpdatedUtc DESC, o.Id DESC) AS ReviewRank,
        o.OrderNumber,
        o.Status,
        o.TotalAmount,
        o.UpdatedUtc
    FROM academy.Orders AS o
    WHERE o.Status <> N'Filled'
)
SELECT TOP (2)
    ReviewRank,
    OrderNumber,
    Status,
    TotalAmount,
    UpdatedUtc
INTO #lesson01_recent_actionable_orders
FROM RankedOrders
ORDER BY ReviewRank;
GO

IF OBJECT_ID(N'tempdb..#lesson01_candidate_user_decisions', N'U') IS NOT NULL
BEGIN
    DROP TABLE #lesson01_candidate_user_decisions;
END;

SELECT
    candidate.CandidateRowId,
    CASE
        WHEN NULLIF(LTRIM(RTRIM(candidate.UserName)), N'') IS NULL THEN N'MissingUserName'
        WHEN NULLIF(LTRIM(RTRIM(candidate.Email)), N'') IS NULL THEN N'MissingEmail'
        WHEN EXISTS
        (
            SELECT 1
            FROM academy.Users AS existingUser
            WHERE existingUser.UserName = candidate.UserName
        ) THEN N'DuplicateUserName'
        WHEN EXISTS
        (
            SELECT 1
            FROM academy.Users AS existingUser
            WHERE existingUser.Email = candidate.Email
        ) THEN N'DuplicateEmail'
        ELSE N'ReadyToInsert'
    END AS DecisionCode
INTO #lesson01_candidate_user_decisions
FROM #lesson01_candidate_users AS candidate;
GO