USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#lesson01_user_directory', N'U') IS NULL
BEGIN
    THROW 51000, '#lesson01_user_directory was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#lesson01_recent_actionable_orders', N'U') IS NULL
BEGIN
    THROW 51000, '#lesson01_recent_actionable_orders was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#lesson01_candidate_user_decisions', N'U') IS NULL
BEGIN
    THROW 51000, '#lesson01_candidate_user_decisions was not created.', 1;
END;

WITH ExpectedUserDirectory AS
(
    SELECT
        ROW_NUMBER() OVER (ORDER BY u.CreatedUtc, u.Id) AS DisplayOrder,
        u.Id AS UserId,
        u.UserName,
        u.Email,
        u.CreatedUtc
    FROM academy.Users AS u
)
IF EXISTS
(
    SELECT DisplayOrder, UserId, UserName, Email, CreatedUtc FROM #lesson01_user_directory
    EXCEPT
    SELECT DisplayOrder, UserId, UserName, Email, CreatedUtc FROM ExpectedUserDirectory
)
OR EXISTS
(
    SELECT DisplayOrder, UserId, UserName, Email, CreatedUtc FROM ExpectedUserDirectory
    EXCEPT
    SELECT DisplayOrder, UserId, UserName, Email, CreatedUtc FROM #lesson01_user_directory
)
BEGIN
    THROW 51000, '#lesson01_user_directory does not match the expected ordered user directory.', 1;
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
),
ExpectedOrders AS
(
    SELECT TOP (2)
        ReviewRank,
        OrderNumber,
        Status,
        TotalAmount,
        UpdatedUtc
    FROM RankedOrders
    ORDER BY ReviewRank
)
IF EXISTS
(
    SELECT ReviewRank, OrderNumber, Status, TotalAmount, UpdatedUtc FROM #lesson01_recent_actionable_orders
    EXCEPT
    SELECT ReviewRank, OrderNumber, Status, TotalAmount, UpdatedUtc FROM ExpectedOrders
)
OR EXISTS
(
    SELECT ReviewRank, OrderNumber, Status, TotalAmount, UpdatedUtc FROM ExpectedOrders
    EXCEPT
    SELECT ReviewRank, OrderNumber, Status, TotalAmount, UpdatedUtc FROM #lesson01_recent_actionable_orders
)
BEGIN
    THROW 51000, '#lesson01_recent_actionable_orders does not match the expected filtered top-two order review.', 1;
END;

WITH ExpectedCandidateDecisions AS
(
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
    FROM #lesson01_candidate_users AS candidate
)
IF EXISTS
(
    SELECT CandidateRowId, DecisionCode FROM #lesson01_candidate_user_decisions
    EXCEPT
    SELECT CandidateRowId, DecisionCode FROM ExpectedCandidateDecisions
)
OR EXISTS
(
    SELECT CandidateRowId, DecisionCode FROM ExpectedCandidateDecisions
    EXCEPT
    SELECT CandidateRowId, DecisionCode FROM #lesson01_candidate_user_decisions
)
BEGIN
    THROW 51000, '#lesson01_candidate_user_decisions does not classify the staged rows correctly.', 1;
END;

PRINT N'Validation succeeded for Beginner 000.';
GO