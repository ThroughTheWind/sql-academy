USE LearningDb;
GO

-- This is the SQL-equivalent shape to inspect when the API sorts by comment count
-- while also applying broad text search and large pages.
DECLARE @Search NVARCHAR(50) = N'a';

SELECT
    p.Id,
    p.Title,
    u.UserName,
    COUNT(c.Id) AS CommentCount,
    p.CreatedUtc
FROM academy.Posts AS p
INNER JOIN academy.Users AS u ON u.Id = p.UserId
LEFT JOIN academy.Comments AS c ON c.PostId = p.Id
WHERE p.Title LIKE CONCAT(N'%', @Search, N'%')
   OR p.Body LIKE CONCAT(N'%', @Search, N'%')
   OR u.UserName LIKE CONCAT(N'%', @Search, N'%')
GROUP BY p.Id, p.Title, u.UserName, p.CreatedUtc
ORDER BY COUNT(c.Id) DESC, p.Id DESC
OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY;
GO