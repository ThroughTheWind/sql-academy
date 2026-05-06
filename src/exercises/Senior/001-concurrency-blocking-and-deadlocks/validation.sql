USE LearningDb;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'tempdb..#blocking_cycle', N'U') IS NULL
BEGIN
    THROW 51000, '#blocking_cycle was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#lock_order_analysis', N'U') IS NULL
BEGIN
    THROW 51000, '#lock_order_analysis was not created.', 1;
END;

IF OBJECT_ID(N'tempdb..#deadlock_mitigation_summary', N'U') IS NULL
BEGIN
    THROW 51000, '#deadlock_mitigation_summary was not created.', 1;
END;

WITH ExpectedBlockingCycle AS
(
    SELECT 1 AS WaitOrder, N'SessionB' AS WaitingSession, N'SessionA' AS BlockingSession, N'academy.Orders' AS ResourceName
    UNION ALL
    SELECT 2, N'SessionA', N'SessionB', N'academy.Trades'
)
IF EXISTS
(
    SELECT WaitOrder, WaitingSession, BlockingSession, ResourceName FROM #blocking_cycle
    EXCEPT
    SELECT WaitOrder, WaitingSession, BlockingSession, ResourceName FROM ExpectedBlockingCycle
)
OR EXISTS
(
    SELECT WaitOrder, WaitingSession, BlockingSession, ResourceName FROM ExpectedBlockingCycle
    EXCEPT
    SELECT WaitOrder, WaitingSession, BlockingSession, ResourceName FROM #blocking_cycle
)
BEGIN
    THROW 51000, '#blocking_cycle does not capture the expected deadlock wait cycle.', 1;
END;

WITH ExpectedLockOrder AS
(
    SELECT N'SessionA' AS SessionLabel, N'academy.Orders' AS FirstResource, N'academy.Trades' AS SecondResource
    UNION ALL
    SELECT N'SessionB', N'academy.Trades', N'academy.Orders'
)
IF EXISTS
(
    SELECT SessionLabel, FirstResource, SecondResource FROM #lock_order_analysis
    EXCEPT
    SELECT SessionLabel, FirstResource, SecondResource FROM ExpectedLockOrder
)
OR EXISTS
(
    SELECT SessionLabel, FirstResource, SecondResource FROM ExpectedLockOrder
    EXCEPT
    SELECT SessionLabel, FirstResource, SecondResource FROM #lock_order_analysis
)
BEGIN
    THROW 51000, '#lock_order_analysis does not show the inconsistent access order that creates risk.', 1;
END;

WITH ExpectedMitigationSummary AS
(
    SELECT N'VictimChoiceDependsOnPriorityOrCost' AS DiagnosticCode, CAST(1 AS BIT) AS IsTrue
    UNION ALL
    SELECT N'UseConsistentAccessOrder', CAST(1 AS BIT)
    UNION ALL
    SELECT N'RetryBelongsInApplicationLayer', CAST(1 AS BIT)
)
IF EXISTS
(
    SELECT DiagnosticCode, IsTrue FROM #deadlock_mitigation_summary
    EXCEPT
    SELECT DiagnosticCode, IsTrue FROM ExpectedMitigationSummary
)
OR EXISTS
(
    SELECT DiagnosticCode, IsTrue FROM ExpectedMitigationSummary
    EXCEPT
    SELECT DiagnosticCode, IsTrue FROM #deadlock_mitigation_summary
)
BEGIN
    THROW 51000, '#deadlock_mitigation_summary does not reflect the expected victim and mitigation reasoning.', 1;
END;

PRINT N'Validation succeeded for Senior 001.';
GO