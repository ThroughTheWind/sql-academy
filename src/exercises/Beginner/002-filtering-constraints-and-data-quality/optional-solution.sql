USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#recent_high_value_orders', N'U') IS NOT NULL DROP TABLE #recent_high_value_orders;

SELECT
    o.OrderNumber,
    u.UserName,
    CAST(o.TotalAmount AS DECIMAL(18, 2)) AS TotalAmount
INTO #recent_high_value_orders
FROM academy.Orders AS o
INNER JOIN academy.Users AS u ON u.Id = o.UserId
WHERE o.CreatedUtc >= '2025-01-20T00:00:00.000'
  AND o.TotalAmount >= 500.00;
GO

IF OBJECT_ID(N'tempdb..#duplicate_import_emails', N'U') IS NOT NULL DROP TABLE #duplicate_import_emails;

SELECT
    LOWER(LTRIM(RTRIM(Email))) AS NormalizedEmail,
    COUNT(*) AS DuplicateCount
INTO #duplicate_import_emails
FROM #ImportedUsers
WHERE NULLIF(LTRIM(RTRIM(Email)), N'') IS NOT NULL
GROUP BY LOWER(LTRIM(RTRIM(Email)))
HAVING COUNT(*) > 1;
GO

IF OBJECT_ID(N'tempdb..#rejected_import_rows', N'U') IS NOT NULL DROP TABLE #rejected_import_rows;

SELECT
    RowId,
    CASE
        WHEN NULLIF(LTRIM(RTRIM(Email)), N'') IS NULL THEN N'MissingEmail'
        WHEN NULLIF(LTRIM(RTRIM(UserName)), N'') IS NULL THEN N'MissingUserName'
    END AS ReasonCode
INTO #rejected_import_rows
FROM #ImportedUsers
WHERE NULLIF(LTRIM(RTRIM(Email)), N'') IS NULL
   OR NULLIF(LTRIM(RTRIM(UserName)), N'') IS NULL;
GO