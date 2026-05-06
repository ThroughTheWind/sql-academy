USE LearningDb;
GO

-- Task 1: backfill missing ExternalReference values in #OrderContractTarget.
-- Preserve existing values such as EXT-2025-0002.

-- Task 2: create #backfill_batches with:
-- BatchNumber INT, OrderId INT, ExternalReference NVARCHAR(64)
IF OBJECT_ID(N'tempdb..#backfill_batches', N'U') IS NOT NULL
BEGIN
    DROP TABLE #backfill_batches;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS BatchNumber,
    CAST(NULL AS INT) AS OrderId,
    CAST(NULL AS NVARCHAR(64)) AS ExternalReference
INTO #backfill_batches;
GO

-- Task 3: create #contract_enforcement_check with:
-- NullExternalReferenceCount INT, DuplicateExternalReferenceCount INT, CanEnforceNotNull BIT
IF OBJECT_ID(N'tempdb..#contract_enforcement_check', N'U') IS NOT NULL
BEGIN
    DROP TABLE #contract_enforcement_check;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS NullExternalReferenceCount,
    CAST(NULL AS INT) AS DuplicateExternalReferenceCount,
    CAST(NULL AS BIT) AS CanEnforceNotNull
INTO #contract_enforcement_check;
GO