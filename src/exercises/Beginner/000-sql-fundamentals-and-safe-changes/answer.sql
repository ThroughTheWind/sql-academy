USE LearningDb;
GO

-- Task 1: create #lesson01_user_directory with:
-- DisplayOrder INT, UserId INT, UserName NVARCHAR(64), Email NVARCHAR(256), CreatedUtc DATETIME2(3)
IF OBJECT_ID(N'tempdb..#lesson01_user_directory', N'U') IS NOT NULL
BEGIN
    DROP TABLE #lesson01_user_directory;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS DisplayOrder,
    CAST(NULL AS INT) AS UserId,
    CAST(NULL AS NVARCHAR(64)) AS UserName,
    CAST(NULL AS NVARCHAR(256)) AS Email,
    CAST(NULL AS DATETIME2(3)) AS CreatedUtc
INTO #lesson01_user_directory;
GO

-- Task 2: create #lesson01_recent_actionable_orders with:
-- ReviewRank INT, OrderNumber NVARCHAR(32), Status NVARCHAR(32), TotalAmount DECIMAL(18,2), UpdatedUtc DATETIME2(3)
IF OBJECT_ID(N'tempdb..#lesson01_recent_actionable_orders', N'U') IS NOT NULL
BEGIN
    DROP TABLE #lesson01_recent_actionable_orders;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS ReviewRank,
    CAST(NULL AS NVARCHAR(32)) AS OrderNumber,
    CAST(NULL AS NVARCHAR(32)) AS Status,
    CAST(NULL AS DECIMAL(18, 2)) AS TotalAmount,
    CAST(NULL AS DATETIME2(3)) AS UpdatedUtc
INTO #lesson01_recent_actionable_orders;
GO

-- Task 3: create #lesson01_candidate_user_decisions with:
-- CandidateRowId INT, DecisionCode NVARCHAR(32)
IF OBJECT_ID(N'tempdb..#lesson01_candidate_user_decisions', N'U') IS NOT NULL
BEGIN
    DROP TABLE #lesson01_candidate_user_decisions;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS CandidateRowId,
    CAST(NULL AS NVARCHAR(32)) AS DecisionCode
INTO #lesson01_candidate_user_decisions;
GO