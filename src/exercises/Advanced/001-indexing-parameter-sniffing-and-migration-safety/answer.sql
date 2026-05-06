USE LearningDb;
GO

DROP INDEX IF EXISTS IX_Trades_UserId_TradedUtc_Include ON academy.Trades;
GO

-- Task 1: create the recommended covering index for academy.GetTradesByUser.
CREATE INDEX IX_Trades_UserId_TradedUtc_Include
ON academy.Trades (UserId, TradedUtc DESC)
INCLUDE (InstrumentId, Quantity, Price, Side);
GO

-- Task 2: create #parameter_sensitivity_summary with:
-- ParameterPattern NVARCHAR(64), RiskSummary NVARCHAR(96), MitigationCode NVARCHAR(64)
IF OBJECT_ID(N'tempdb..#parameter_sensitivity_summary', N'U') IS NOT NULL
BEGIN
    DROP TABLE #parameter_sensitivity_summary;
END;

SELECT TOP (0)
    CAST(NULL AS NVARCHAR(64)) AS ParameterPattern,
    CAST(NULL AS NVARCHAR(96)) AS RiskSummary,
    CAST(NULL AS NVARCHAR(64)) AS MitigationCode
INTO #parameter_sensitivity_summary;
GO

-- Task 3: create #safe_post_summary_rollout with:
-- StepNumber INT, StepCode NVARCHAR(64), StepCategory NVARCHAR(32)
IF OBJECT_ID(N'tempdb..#safe_post_summary_rollout', N'U') IS NOT NULL
BEGIN
    DROP TABLE #safe_post_summary_rollout;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS StepNumber,
    CAST(NULL AS NVARCHAR(64)) AS StepCode,
    CAST(NULL AS NVARCHAR(32)) AS StepCategory
INTO #safe_post_summary_rollout;
GO