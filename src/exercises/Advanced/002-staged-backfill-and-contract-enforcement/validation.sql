USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#backfill_batches', N'U') IS NULL
BEGIN
    THROW 51000, '#backfill_batches was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#contract_enforcement_check', N'U') IS NULL
BEGIN
    THROW 51000, '#contract_enforcement_check was not created.', 1;
END;

WITH ExpectedBackfilledRows AS
(
    SELECT 1 AS Id, N'LEGACY-ORD-2025-0001' AS ExternalReference
    UNION ALL
    SELECT 2, N'EXT-2025-0002'
    UNION ALL
    SELECT 3, N'LEGACY-ORD-2025-0003'
    UNION ALL
    SELECT 4, N'LEGACY-ORD-2025-0004'
)
IF EXISTS
(
    SELECT Id, ExternalReference FROM #OrderContractTarget
    EXCEPT
    SELECT Id, ExternalReference FROM ExpectedBackfilledRows
)
OR EXISTS
(
    SELECT Id, ExternalReference FROM ExpectedBackfilledRows
    EXCEPT
    SELECT Id, ExternalReference FROM #OrderContractTarget
)
BEGIN
    THROW 51000, '#OrderContractTarget was not backfilled as expected.', 1;
END;

WITH ExpectedBatches AS
(
    SELECT 1 AS BatchNumber, 1 AS OrderId, N'LEGACY-ORD-2025-0001' AS ExternalReference
    UNION ALL
    SELECT 1, 3, N'LEGACY-ORD-2025-0003'
    UNION ALL
    SELECT 2, 4, N'LEGACY-ORD-2025-0004'
)
IF EXISTS
(
    SELECT BatchNumber, OrderId, ExternalReference FROM #backfill_batches
    EXCEPT
    SELECT BatchNumber, OrderId, ExternalReference FROM ExpectedBatches
)
OR EXISTS
(
    SELECT BatchNumber, OrderId, ExternalReference FROM ExpectedBatches
    EXCEPT
    SELECT BatchNumber, OrderId, ExternalReference FROM #backfill_batches
)
BEGIN
    THROW 51000, '#backfill_batches is not deterministic or includes the wrong rows.', 1;
END;

WITH ExpectedCheck AS
(
    SELECT 0 AS NullExternalReferenceCount, 0 AS DuplicateExternalReferenceCount, CAST(1 AS BIT) AS CanEnforceNotNull
)
IF EXISTS
(
    SELECT NullExternalReferenceCount, DuplicateExternalReferenceCount, CanEnforceNotNull FROM #contract_enforcement_check
    EXCEPT
    SELECT NullExternalReferenceCount, DuplicateExternalReferenceCount, CanEnforceNotNull FROM ExpectedCheck
)
OR EXISTS
(
    SELECT NullExternalReferenceCount, DuplicateExternalReferenceCount, CanEnforceNotNull FROM ExpectedCheck
    EXCEPT
    SELECT NullExternalReferenceCount, DuplicateExternalReferenceCount, CanEnforceNotNull FROM #contract_enforcement_check
)
BEGIN
    THROW 51000, '#contract_enforcement_check does not prove the contract is ready to enforce.', 1;
END;

PRINT N'Validation succeeded for Advanced 002.';
GO