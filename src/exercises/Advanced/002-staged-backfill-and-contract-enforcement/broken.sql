USE LearningDb;
GO

-- Problem 1: this overwrites existing references instead of only filling gaps.
UPDATE #OrderContractTarget
SET ExternalReference = CONCAT(N'LEGACY-', OrderNumber);
GO

-- Problem 2: this creates one huge batch instead of a controlled staged plan.
SELECT
    1 AS BatchNumber,
    Id AS OrderId,
    ExternalReference
FROM #OrderContractTarget
WHERE NeedsBackfill = 1;
GO

-- Problem 3: this checks for nulls only and ignores duplicate references.
SELECT
    COUNT(*) AS NullExternalReferenceCount,
    CAST(CASE WHEN COUNT(*) = 0 THEN 1 ELSE 0 END AS BIT) AS CanEnforceNotNull
FROM #OrderContractTarget
WHERE ExternalReference IS NULL;
GO