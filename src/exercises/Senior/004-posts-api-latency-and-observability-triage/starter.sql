USE LearningDb;
GO

DECLARE @Search NVARCHAR(50) = N'a';

SELECT TOP (10)
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
ORDER BY COUNT(c.Id) DESC, p.Id DESC;
GO

SELECT TOP (10)
    p.Id,
    p.Title,
    u.UserName,
    p.CreatedUtc
FROM academy.Posts AS p
INNER JOIN academy.Users AS u ON u.Id = p.UserId
WHERE p.Title LIKE CONCAT(N'%', @Search, N'%')
   OR p.Body LIKE CONCAT(N'%', @Search, N'%')
   OR u.UserName LIKE CONCAT(N'%', @Search, N'%')
ORDER BY p.CreatedUtc DESC, p.Id DESC;
GO

SELECT
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    COL_NAME(ic.object_id, ic.column_id) AS ColumnName,
    ic.key_ordinal,
    ic.is_included_column
FROM sys.indexes AS i
INNER JOIN sys.index_columns AS ic
    ON ic.object_id = i.object_id
   AND ic.index_id = i.index_id
WHERE i.object_id IN (OBJECT_ID(N'academy.Posts'), OBJECT_ID(N'academy.Comments'))
  AND i.name IS NOT NULL
ORDER BY TableName, IndexName, ic.is_included_column, ic.key_ordinal, ic.index_column_id;
GO