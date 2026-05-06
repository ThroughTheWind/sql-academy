USE LearningDb;
GO

-- Problem 1: the WHERE clause turns the LEFT JOIN into an INNER JOIN and silently drops posts without comments.
SELECT
    u.UserName,
    p.Title,
    c.Body
FROM academy.Users AS u
INNER JOIN academy.Posts AS p ON p.UserId = u.Id
LEFT JOIN academy.Comments AS c ON c.PostId = p.Id
WHERE c.Id IS NOT NULL
ORDER BY p.CreatedUtc DESC;
GO

-- Problem 2: post counts are inflated because each comment duplicates the joined post row.
SELECT
    u.UserName,
    COUNT(*) AS PostCount
FROM academy.Users AS u
INNER JOIN academy.Posts AS p ON p.UserId = u.Id
LEFT JOIN academy.Comments AS c ON c.PostId = p.Id
GROUP BY u.UserName
ORDER BY u.UserName;
GO