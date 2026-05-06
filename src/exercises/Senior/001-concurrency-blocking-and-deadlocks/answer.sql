USE LearningDb;
GO

-- Task 1: create #blocking_cycle with:
-- WaitOrder INT, WaitingSession NVARCHAR(16), BlockingSession NVARCHAR(16), ResourceName NVARCHAR(64)
IF OBJECT_ID(N'tempdb..#blocking_cycle', N'U') IS NOT NULL
BEGIN
    DROP TABLE #blocking_cycle;
END;

SELECT TOP (0)
    CAST(NULL AS INT) AS WaitOrder,
    CAST(NULL AS NVARCHAR(16)) AS WaitingSession,
    CAST(NULL AS NVARCHAR(16)) AS BlockingSession,
    CAST(NULL AS NVARCHAR(64)) AS ResourceName
INTO #blocking_cycle;
GO

-- Task 2: create #lock_order_analysis with:
-- SessionLabel NVARCHAR(16), FirstResource NVARCHAR(64), SecondResource NVARCHAR(64)
IF OBJECT_ID(N'tempdb..#lock_order_analysis', N'U') IS NOT NULL
BEGIN
    DROP TABLE #lock_order_analysis;
END;

SELECT TOP (0)
    CAST(NULL AS NVARCHAR(16)) AS SessionLabel,
    CAST(NULL AS NVARCHAR(64)) AS FirstResource,
    CAST(NULL AS NVARCHAR(64)) AS SecondResource
INTO #lock_order_analysis;
GO

-- Task 3: create #deadlock_mitigation_summary with:
-- DiagnosticCode NVARCHAR(64), IsTrue BIT
IF OBJECT_ID(N'tempdb..#deadlock_mitigation_summary', N'U') IS NOT NULL
BEGIN
    DROP TABLE #deadlock_mitigation_summary;
END;

SELECT TOP (0)
    CAST(NULL AS NVARCHAR(64)) AS DiagnosticCode,
    CAST(NULL AS BIT) AS IsTrue
INTO #deadlock_mitigation_summary;
GO