USE LearningDb;
GO

-- Problem 1: the query is not tagged, so the cache review cannot isolate the lab workload.
SELECT TOP (1)
    qs.total_worker_time,
    st.text
FROM sys.dm_exec_query_stats AS qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) AS st
ORDER BY qs.total_worker_time DESC;
GO

-- Problem 2: page counts are summed across all indexes, which can double-count the storage footprint.
SELECT
    OBJECT_NAME(ps.object_id) AS TableName,
    SUM(ps.used_page_count) AS UsedPageCount
FROM sys.dm_db_partition_stats AS ps
WHERE ps.object_id IN (OBJECT_ID(N'academy.Posts'), OBJECT_ID(N'academy.Comments'), OBJECT_ID(N'academy.Trades'))
GROUP BY ps.object_id;
GO

-- Problem 3: the top wait is treated as the verdict instead of a signal that needs workload context.
SELECT TOP (1)
    wait_type,
    wait_time_ms
FROM sys.dm_os_wait_stats
ORDER BY wait_time_ms DESC;
GO