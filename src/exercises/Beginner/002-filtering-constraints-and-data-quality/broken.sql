USE LearningDb;
GO

-- Problem 1: this predicate forgets the date requirement and would return older high-value rows if they existed.
SELECT
    o.OrderNumber,
    u.UserName,
    o.TotalAmount
FROM academy.Orders AS o
INNER JOIN academy.Users AS u ON u.Id = o.UserId
WHERE o.TotalAmount >= 500.00;
GO

-- Problem 2: this duplicate check ignores whitespace and case differences, so it misses real duplicates.
SELECT
    Email,
    COUNT(*) AS DuplicateCount
FROM #ImportedUsers
GROUP BY Email
HAVING COUNT(*) > 1;
GO

-- Problem 3: this filter only catches NULLs and misses blank user names.
SELECT RowId
FROM #ImportedUsers
WHERE Email IS NULL OR UserName IS NULL;
GO