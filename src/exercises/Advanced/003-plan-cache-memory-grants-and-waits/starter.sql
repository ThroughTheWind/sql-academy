USE LearningDb;
GO

SET NOCOUNT ON;
GO

DECLARE @AdaId INT = (SELECT Id FROM academy.Users WHERE UserName = N'ada');

EXEC sp_executesql
    N'SELECT /* internals_lab_trade_filter */ COUNT_BIG(*) AS TradeCount FROM academy.Trades WHERE UserId = @UserId;',
    N'@UserId INT',
    @UserId = @AdaId;
GO

DECLARE @LinusId INT = (SELECT Id FROM academy.Users WHERE UserName = N'linus');

EXEC sp_executesql
    N'SELECT /* internals_lab_trade_filter */ COUNT_BIG(*) AS TradeCount FROM academy.Trades WHERE UserId = @UserId;',
    N'@UserId INT',
    @UserId = @LinusId;
GO

SELECT
    OBJECT_NAME(ps.object_id) AS TableName,
    SUM(ps.row_count) AS RowCount,
    SUM(ps.used_page_count) AS UsedPageCount
FROM sys.dm_db_partition_stats AS ps
WHERE ps.object_id IN (OBJECT_ID(N'academy.Posts'), OBJECT_ID(N'academy.Comments'), OBJECT_ID(N'academy.Trades'))
  AND ps.index_id IN (0, 1)
GROUP BY ps.object_id
ORDER BY TableName;
GO

SELECT TOP (20)
    qs.execution_count,
    qs.total_logical_reads,
    qs.total_worker_time,
    qs.last_grant_kb,
    qs.max_used_grant_kb,
    st.text AS SqlText
FROM sys.dm_exec_query_stats AS qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) AS st
WHERE st.text LIKE N'%internals_lab_trade_filter%'
ORDER BY qs.last_execution_time DESC;
GO

SELECT TOP (20)
    wait_type,
    waiting_tasks_count,
    wait_time_ms,
    signal_wait_time_ms
FROM sys.dm_os_wait_stats
WHERE wait_type NOT LIKE N'SLEEP%'
  AND wait_type NOT LIKE N'BROKER_%'
ORDER BY wait_time_ms DESC;
GO