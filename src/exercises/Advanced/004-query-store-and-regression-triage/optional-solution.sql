USE LearningDb;
GO

SET NOCOUNT ON;
GO

SELECT
    q.query_id,
    p.plan_id,
    p.is_forced_plan,
    rs.count_executions,
    rs.avg_duration,
    rs.avg_cpu_time,
    rs.avg_logical_io_reads,
    rs.last_execution_time,
    qt.query_sql_text
FROM sys.query_store_query_text AS qt
INNER JOIN sys.query_store_query AS q ON q.query_text_id = qt.query_text_id
INNER JOIN sys.query_store_plan AS p ON p.query_id = q.query_id
INNER JOIN sys.query_store_runtime_stats AS rs ON rs.plan_id = p.plan_id
WHERE qt.query_sql_text LIKE N'%query_store_lab_trade_lookup%'
ORDER BY rs.last_execution_time DESC, p.plan_id DESC;
GO

SELECT
    COUNT(DISTINCT p.plan_id) AS PlanCount,
    SUM(rs.count_executions) AS TotalExecutions,
    MAX(CASE WHEN p.is_forced_plan = 1 THEN 1 ELSE 0 END) AS HasForcedPlan
FROM sys.query_store_query_text AS qt
INNER JOIN sys.query_store_query AS q ON q.query_text_id = qt.query_text_id
INNER JOIN sys.query_store_plan AS p ON p.query_id = q.query_id
INNER JOIN sys.query_store_runtime_stats AS rs ON rs.plan_id = p.plan_id
WHERE qt.query_sql_text LIKE N'%query_store_lab_trade_lookup%';
GO

-- One defensible containment rule:
-- only consider forcing a plan when you have isolated one tagged query,
-- confirmed that one known-good plan is materially safer than the alternative,
-- and written down the signal that would make you remove the force.