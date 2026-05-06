USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#recent_high_value_orders', N'U') IS NULL
BEGIN
    THROW 51000, '#recent_high_value_orders was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#duplicate_import_emails', N'U') IS NULL
BEGIN
    THROW 51000, '#duplicate_import_emails was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#rejected_import_rows', N'U') IS NULL
BEGIN
    THROW 51000, '#rejected_import_rows was not created.', 1;
END;

WITH ExpectedRecentOrders AS
(
    SELECT N'ORD-2025-0001' AS OrderNumber, N'ada' AS UserName, CAST(512.40 AS DECIMAL(18, 2)) AS TotalAmount
    UNION ALL
    SELECT N'ORD-2025-0002', N'grace', CAST(980.10 AS DECIMAL(18, 2))
)
IF EXISTS
(
    SELECT OrderNumber, UserName, TotalAmount FROM #recent_high_value_orders
    EXCEPT
    SELECT OrderNumber, UserName, TotalAmount FROM ExpectedRecentOrders
)
OR EXISTS
(
    SELECT OrderNumber, UserName, TotalAmount FROM ExpectedRecentOrders
    EXCEPT
    SELECT OrderNumber, UserName, TotalAmount FROM #recent_high_value_orders
)
BEGIN
    THROW 51000, '#recent_high_value_orders does not match the expected filtered result.', 1;
END;

WITH ExpectedDuplicateEmails AS
(
    SELECT N'ada@sqlacademy.local' AS NormalizedEmail, 2 AS DuplicateCount
    UNION ALL
    SELECT N'margaret@sqlacademy.local', 2
)
IF EXISTS
(
    SELECT NormalizedEmail, DuplicateCount FROM #duplicate_import_emails
    EXCEPT
    SELECT NormalizedEmail, DuplicateCount FROM ExpectedDuplicateEmails
)
OR EXISTS
(
    SELECT NormalizedEmail, DuplicateCount FROM ExpectedDuplicateEmails
    EXCEPT
    SELECT NormalizedEmail, DuplicateCount FROM #duplicate_import_emails
)
BEGIN
    THROW 51000, '#duplicate_import_emails does not normalize or count duplicates correctly.', 1;
END;

WITH ExpectedRejectedRows AS
(
    SELECT 3 AS RowId, N'MissingEmail' AS ReasonCode
    UNION ALL
    SELECT 4, N'MissingUserName'
)
IF EXISTS
(
    SELECT RowId, ReasonCode FROM #rejected_import_rows
    EXCEPT
    SELECT RowId, ReasonCode FROM ExpectedRejectedRows
)
OR EXISTS
(
    SELECT RowId, ReasonCode FROM ExpectedRejectedRows
    EXCEPT
    SELECT RowId, ReasonCode FROM #rejected_import_rows
)
BEGIN
    THROW 51000, '#rejected_import_rows does not flag the expected invalid staging rows.', 1;
END;

PRINT N'Validation succeeded for Beginner 002.';
GO