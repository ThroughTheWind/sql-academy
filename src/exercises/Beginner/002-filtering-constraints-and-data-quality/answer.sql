USE LearningDb;
GO

-- Task 1: create #recent_high_value_orders with:
-- OrderNumber NVARCHAR(32), UserName NVARCHAR(64), TotalAmount DECIMAL(18,2)
IF OBJECT_ID(N'tempdb..#recent_high_value_orders', N'U') IS NOT NULL
BEGIN
    DROP TABLE #recent_high_value_orders;
END;

SELECT TOP (0)
    CAST(NULL AS NVARCHAR(32)) AS OrderNumber,
    CAST(NULL AS NVARCHAR(64)) AS UserName,
    CAST(NULL AS DECIMAL(18, 2)) AS TotalAmount
INTO #recent_high_value_orders;
GO

-- Task 2: create #duplicate_import_emails with:
-- NormalizedEmail NVARCHAR(256), DuplicateCount INT
IF OBJECT_ID(N'tempdb..#duplicate_import_emails', N'U') IS NOT NULL
BEGIN
    DROP TABLE #duplicate_import_emails;
END;

SELECT TOP (0)
    CAST(NULL AS NVARCHAR(256)) AS NormalizedEmail,
    CAST(NULL AS INT) AS DuplicateCount
INTO #duplicate_import_emails;
GO

-- Task 3: create #rejected_import_rows with:
-- RowId INT, ReasonCode NVARCHAR(32)
IF OBJECT_ID(N'tempdb..#rejected_import_rows', N'U') IS NOT NULL
BEGIN
    DROP TABLE #rejected_import_rows;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS RowId,
    CAST(NULL AS NVARCHAR(32)) AS ReasonCode
INTO #rejected_import_rows;
GO