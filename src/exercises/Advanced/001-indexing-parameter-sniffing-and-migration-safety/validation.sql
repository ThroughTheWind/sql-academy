USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#parameter_sensitivity_summary', N'U') IS NULL
BEGIN
    THROW 51000, '#parameter_sensitivity_summary was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#safe_post_summary_rollout', N'U') IS NULL
BEGIN
    THROW 51000, '#safe_post_summary_rollout was not created.', 1;
END;

DECLARE @tradeIndexId INT;

SELECT @tradeIndexId = i.index_id
FROM sys.indexes AS i
INNER JOIN sys.tables AS t ON t.object_id = i.object_id
INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id
WHERE s.name = N'academy'
  AND t.name = N'Trades'
  AND i.name = N'IX_Trades_UserId_TradedUtc_Include';

IF @tradeIndexId IS NULL
BEGIN
    THROW 51000, 'IX_Trades_UserId_TradedUtc_Include was not created on academy.Trades.', 1;
END;

WITH ExpectedKeyColumns AS
(
    SELECT 1 AS KeyOrdinal, N'UserId' AS ColumnName, CAST(0 AS BIT) AS IsDescending
    UNION ALL
    SELECT 2, N'TradedUtc', CAST(1 AS BIT)
),
ActualKeyColumns AS
(
    SELECT
        ic.key_ordinal AS KeyOrdinal,
        c.name AS ColumnName,
        CAST(ic.is_descending_key AS BIT) AS IsDescending
    FROM sys.index_columns AS ic
    INNER JOIN sys.columns AS c
        ON c.object_id = ic.object_id
       AND c.column_id = ic.column_id
    WHERE ic.object_id = OBJECT_ID(N'academy.Trades')
      AND ic.index_id = @tradeIndexId
      AND ic.is_included_column = 0
)
IF EXISTS
(
    SELECT KeyOrdinal, ColumnName, IsDescending FROM ActualKeyColumns
    EXCEPT
    SELECT KeyOrdinal, ColumnName, IsDescending FROM ExpectedKeyColumns
)
OR EXISTS
(
    SELECT KeyOrdinal, ColumnName, IsDescending FROM ExpectedKeyColumns
    EXCEPT
    SELECT KeyOrdinal, ColumnName, IsDescending FROM ActualKeyColumns
)
BEGIN
    THROW 51000, 'The created trade index keys do not match the expected workload-driven shape.', 1;
END;

WITH ExpectedIncludeColumns AS
(
    SELECT N'InstrumentId' AS ColumnName
    UNION ALL
    SELECT N'Quantity'
    UNION ALL
    SELECT N'Price'
    UNION ALL
    SELECT N'Side'
),
ActualIncludeColumns AS
(
    SELECT c.name AS ColumnName
    FROM sys.index_columns AS ic
    INNER JOIN sys.columns AS c
        ON c.object_id = ic.object_id
       AND c.column_id = ic.column_id
    WHERE ic.object_id = OBJECT_ID(N'academy.Trades')
      AND ic.index_id = @tradeIndexId
      AND ic.is_included_column = 1
)
IF EXISTS
(
    SELECT ColumnName FROM ActualIncludeColumns
    EXCEPT
    SELECT ColumnName FROM ExpectedIncludeColumns
)
OR EXISTS
(
    SELECT ColumnName FROM ExpectedIncludeColumns
    EXCEPT
    SELECT ColumnName FROM ActualIncludeColumns
)
BEGIN
    THROW 51000, 'The created trade index include columns do not match the expected covering shape.', 1;
END;

WITH ExpectedSensitivitySummary AS
(
    SELECT
        N'DifferentUserIdSelectivity' AS ParameterPattern,
        N'OneCachedPlanCanMisfitAnother' AS RiskSummary,
        N'ReviewRepresentativePlans' AS MitigationCode
)
IF EXISTS
(
    SELECT ParameterPattern, RiskSummary, MitigationCode FROM #parameter_sensitivity_summary
    EXCEPT
    SELECT ParameterPattern, RiskSummary, MitigationCode FROM ExpectedSensitivitySummary
)
OR EXISTS
(
    SELECT ParameterPattern, RiskSummary, MitigationCode FROM ExpectedSensitivitySummary
    EXCEPT
    SELECT ParameterPattern, RiskSummary, MitigationCode FROM #parameter_sensitivity_summary
)
BEGIN
    THROW 51000, '#parameter_sensitivity_summary does not describe the expected plan-sensitivity diagnosis.', 1;
END;

WITH ExpectedRollout AS
(
    SELECT 1 AS StepNumber, N'AddSummaryNullable' AS StepCode, N'Additive' AS StepCategory
    UNION ALL
    SELECT 2, N'BackfillExistingRows', N'Backfill'
    UNION ALL
    SELECT 3, N'EnforceNotNull', N'Enforcement'
)
IF EXISTS
(
    SELECT StepNumber, StepCode, StepCategory FROM #safe_post_summary_rollout
    EXCEPT
    SELECT StepNumber, StepCode, StepCategory FROM ExpectedRollout
)
OR EXISTS
(
    SELECT StepNumber, StepCode, StepCategory FROM ExpectedRollout
    EXCEPT
    SELECT StepNumber, StepCode, StepCategory FROM #safe_post_summary_rollout
)
BEGIN
    THROW 51000, '#safe_post_summary_rollout does not match the expected staged migration plan.', 1;
END;

PRINT N'Validation succeeded for Advanced 001.';
GO