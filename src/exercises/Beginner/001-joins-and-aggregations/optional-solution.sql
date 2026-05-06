USE LearningDb;
GO

SELECT
    u.UserName,
    p.Title,
    COUNT(c.Id) AS CommentCount,
    p.CreatedUtc
FROM academy.Users AS u
INNER JOIN academy.Posts AS p ON p.UserId = u.Id
LEFT JOIN academy.Comments AS c ON c.PostId = p.Id
GROUP BY u.UserName, p.Title, p.CreatedUtc, p.Id
ORDER BY p.CreatedUtc DESC, p.Id DESC;
GO

SELECT
    u.UserName,
    COUNT(DISTINCT p.Id) AS PostCount,
    COUNT(c.Id) AS CommentCount
FROM academy.Users AS u
LEFT JOIN academy.Posts AS p ON p.UserId = u.Id
LEFT JOIN academy.Comments AS c ON c.PostId = p.Id
GROUP BY u.UserName
ORDER BY u.UserName;
GO