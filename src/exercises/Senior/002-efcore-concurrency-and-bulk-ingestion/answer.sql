USE LearningDb;
GO

-- Task 1: create #n_plus_one_fix_plan with:
-- ReviewArea NVARCHAR(64), CurrentRisk NVARCHAR(64), SaferShape NVARCHAR(96), PrimaryBenefit NVARCHAR(96)
IF OBJECT_ID(N'tempdb..#n_plus_one_fix_plan', N'U') IS NOT NULL
BEGIN
    DROP TABLE #n_plus_one_fix_plan;
END;

SELECT TOP (0)
    CAST(NULL AS NVARCHAR(64)) AS ReviewArea,
    CAST(NULL AS NVARCHAR(64)) AS CurrentRisk,
    CAST(NULL AS NVARCHAR(96)) AS SaferShape,
    CAST(NULL AS NVARCHAR(96)) AS PrimaryBenefit
INTO #n_plus_one_fix_plan;
GO

-- Task 2: create #order_concurrency_check with:
-- OrderNumber NVARCHAR(32), CurrentStatus NVARCHAR(32), RowVersionHex NVARCHAR(34), ConflictOutcome NVARCHAR(64)
IF OBJECT_ID(N'tempdb..#order_concurrency_check', N'U') IS NOT NULL
BEGIN
    DROP TABLE #order_concurrency_check;
END;

SELECT TOP (0)
    CAST(NULL AS NVARCHAR(32)) AS OrderNumber,
    CAST(NULL AS NVARCHAR(32)) AS CurrentStatus,
    CAST(NULL AS NVARCHAR(34)) AS RowVersionHex,
    CAST(NULL AS NVARCHAR(64)) AS ConflictOutcome
INTO #order_concurrency_check;
GO

-- Task 3: create #ingestion_pipeline_steps with:
-- StepOrder INT, StepName NVARCHAR(32), WorkingSet NVARCHAR(64), Purpose NVARCHAR(128)
IF OBJECT_ID(N'tempdb..#ingestion_pipeline_steps', N'U') IS NOT NULL
BEGIN
    DROP TABLE #ingestion_pipeline_steps;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS StepOrder,
    CAST(NULL AS NVARCHAR(32)) AS StepName,
    CAST(NULL AS NVARCHAR(64)) AS WorkingSet,
    CAST(NULL AS NVARCHAR(128)) AS Purpose
INTO #ingestion_pipeline_steps;
GO