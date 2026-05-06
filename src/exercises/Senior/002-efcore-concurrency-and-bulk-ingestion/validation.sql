USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#n_plus_one_fix_plan', N'U') IS NULL
BEGIN
    THROW 51000, '#n_plus_one_fix_plan was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#order_concurrency_check', N'U') IS NULL
BEGIN
    THROW 51000, '#order_concurrency_check was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#ingestion_pipeline_steps', N'U') IS NULL
BEGIN
    THROW 51000, '#ingestion_pipeline_steps was not created.', 1;
END;

WITH ExpectedNPlusOnePlan AS
(
    SELECT
        N'PostsReadPath' AS ReviewArea,
        N'PerRowChildLookup' AS CurrentRisk,
        N'SingleProjectionWithCounts' AS SaferShape,
        N'AvoidsExtraRoundTrips' AS PrimaryBenefit
)
IF EXISTS
(
    SELECT ReviewArea, CurrentRisk, SaferShape, PrimaryBenefit FROM #n_plus_one_fix_plan
    EXCEPT
    SELECT ReviewArea, CurrentRisk, SaferShape, PrimaryBenefit FROM ExpectedNPlusOnePlan
)
OR EXISTS
(
    SELECT ReviewArea, CurrentRisk, SaferShape, PrimaryBenefit FROM ExpectedNPlusOnePlan
    EXCEPT
    SELECT ReviewArea, CurrentRisk, SaferShape, PrimaryBenefit FROM #n_plus_one_fix_plan
)
BEGIN
    THROW 51000, '#n_plus_one_fix_plan does not describe the expected N+1 remediation shape.', 1;
END;

WITH ExpectedConcurrencyCheck AS
(
    SELECT
        o.OrderNumber,
        o.Status AS CurrentStatus,
        CONVERT(NVARCHAR(34), sys.fn_varbintohexstr(o.RowVersion)) AS RowVersionHex,
        N'RaiseConcurrencyConflict' AS ConflictOutcome
    FROM academy.Orders AS o
    WHERE o.OrderNumber = N'ORD-2025-0002'
)
IF EXISTS
(
    SELECT OrderNumber, CurrentStatus, RowVersionHex, ConflictOutcome FROM #order_concurrency_check
    EXCEPT
    SELECT OrderNumber, CurrentStatus, RowVersionHex, ConflictOutcome FROM ExpectedConcurrencyCheck
)
OR EXISTS
(
    SELECT OrderNumber, CurrentStatus, RowVersionHex, ConflictOutcome FROM ExpectedConcurrencyCheck
    EXCEPT
    SELECT OrderNumber, CurrentStatus, RowVersionHex, ConflictOutcome FROM #order_concurrency_check
)
BEGIN
    THROW 51000, '#order_concurrency_check does not match the expected optimistic-concurrency review.', 1;
END;

WITH ExpectedPipelineSteps AS
(
    SELECT 1 AS StepOrder, N'Land' AS StepName, N'#TradeImportStage' AS WorkingSet, N'Capture the raw feed without touching serving tables' AS Purpose
    UNION ALL
    SELECT 2, N'Validate', N'validated batch set', N'Reject bad symbols, sides, and required-value gaps'
    UNION ALL
    SELECT 3, N'Deduplicate', N'deduplicated batch set', N'Remove repeated business rows before publish'
    UNION ALL
    SELECT 4, N'Publish', N'academy.Trades', N'Insert only approved rows in a controlled final step'
)
IF EXISTS
(
    SELECT StepOrder, StepName, WorkingSet, Purpose FROM #ingestion_pipeline_steps
    EXCEPT
    SELECT StepOrder, StepName, WorkingSet, Purpose FROM ExpectedPipelineSteps
)
OR EXISTS
(
    SELECT StepOrder, StepName, WorkingSet, Purpose FROM ExpectedPipelineSteps
    EXCEPT
    SELECT StepOrder, StepName, WorkingSet, Purpose FROM #ingestion_pipeline_steps
)
BEGIN
    THROW 51000, '#ingestion_pipeline_steps does not separate landing, validation, deduplication, and publish steps clearly.', 1;
END;

PRINT N'Validation succeeded for Senior 002.';
GO