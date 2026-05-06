USE LearningDb;
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

-- Example reasoning notes:
-- 1. The tagged query appears in the plan cache with reuse evidence, so compilation is no longer just a guess.
-- 2. Used page counts turn future I/O discussions into something measurable instead of purely logical SQL discussion.
-- 3. The next step is to inspect memory-grant fields or waits as signals that either strengthen or weaken the current theory.