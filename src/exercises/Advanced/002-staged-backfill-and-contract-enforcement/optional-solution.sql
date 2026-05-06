USE LearningDb;
GO

UPDATE #OrderContractTarget
SET ExternalReference = CONCAT(N'LEGACY-', OrderNumber)
WHERE NeedsBackfill = 1
  AND ExternalReference IS NULL;
GO

IF OBJECT_ID(N'tempdb..#backfill_batches', N'U') IS NOT NULL DROP TABLE #backfill_batches;

WITH OrderedBackfills AS
(
    SELECT
        Id,
        ExternalReference,
        ((ROW_NUMBER() OVER (ORDER BY Id) - 1) / 2) + 1 AS BatchNumber
    FROM #OrderContractTarget
    WHERE NeedsBackfill = 1
)
SELECT
    BatchNumber,
    Id AS OrderId,
    ExternalReference
INTO #backfill_batches
FROM OrderedBackfills;
GO

IF OBJECT_ID(N'tempdb..#contract_enforcement_check', N'U') IS NOT NULL DROP TABLE #contract_enforcement_check;

SELECT
    SUM(CASE WHEN ExternalReference IS NULL THEN 1 ELSE 0 END) AS NullExternalReferenceCount,
    COUNT(*) - COUNT(DISTINCT ExternalReference) AS DuplicateExternalReferenceCount,
    CAST(CASE
        WHEN SUM(CASE WHEN ExternalReference IS NULL THEN 1 ELSE 0 END) = 0
         AND COUNT(*) = COUNT(DISTINCT ExternalReference) THEN 1
        ELSE 0
    END AS BIT) AS CanEnforceNotNull
INTO #contract_enforcement_check
FROM #OrderContractTarget;
GO