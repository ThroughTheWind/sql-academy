USE LearningDb;
GO

-- Problem 1: projection is too broad and the result order is implicit.
SELECT *
FROM academy.Users;
GO

-- Problem 2: TOP is applied without filtering to the actionable statuses first.
SELECT TOP (2)
    OrderNumber,
    Status,
    TotalAmount,
    UpdatedUtc
FROM academy.Orders
ORDER BY UpdatedUtc DESC;
GO

-- Problem 3: duplicate usernames are checked, but missing values and duplicate emails are ignored.
SELECT
    CandidateRowId,
    CASE
        WHEN EXISTS
        (
            SELECT 1
            FROM academy.Users AS existingUser
            WHERE existingUser.UserName = candidate.UserName
        ) THEN N'DuplicateUserName'
        ELSE N'ReadyToInsert'
    END AS DecisionCode
FROM #lesson01_candidate_users AS candidate;
GO