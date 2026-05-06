USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#n_plus_one_fix_plan', N'U') IS NOT NULL
BEGIN
    DROP TABLE #n_plus_one_fix_plan;
END;

SELECT
    N'PostsReadPath' AS ReviewArea,
    N'PerRowChildLookup' AS CurrentRisk,
    N'SingleProjectionWithCounts' AS SaferShape,
    N'AvoidsExtraRoundTrips' AS PrimaryBenefit
INTO #n_plus_one_fix_plan;
GO

IF OBJECT_ID(N'tempdb..#order_concurrency_check', N'U') IS NOT NULL
BEGIN
    DROP TABLE #order_concurrency_check;
END;

SELECT
    o.OrderNumber,
    o.Status AS CurrentStatus,
    CONVERT(NVARCHAR(34), sys.fn_varbintohexstr(o.RowVersion)) AS RowVersionHex,
    N'RaiseConcurrencyConflict' AS ConflictOutcome
INTO #order_concurrency_check
FROM academy.Orders AS o
WHERE o.OrderNumber = N'ORD-2025-0002';
GO

IF OBJECT_ID(N'tempdb..#ingestion_pipeline_steps', N'U') IS NOT NULL
BEGIN
    DROP TABLE #ingestion_pipeline_steps;
END;

SELECT 1 AS StepOrder, N'Land' AS StepName, N'#TradeImportStage' AS WorkingSet, N'Capture the raw feed without touching serving tables' AS Purpose
INTO #ingestion_pipeline_steps
UNION ALL
SELECT 2, N'Validate', N'validated batch set', N'Reject bad symbols, sides, and required-value gaps'
UNION ALL
SELECT 3, N'Deduplicate', N'deduplicated batch set', N'Remove repeated business rows before publish'
UNION ALL
SELECT 4, N'Publish', N'academy.Trades', N'Insert only approved rows in a controlled final step';
GO