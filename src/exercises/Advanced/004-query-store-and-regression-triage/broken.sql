USE LearningDb;
GO

SET NOCOUNT ON;
GO

-- Problem 1: this ignores the tagged lab query and just grabs whatever currently looks expensive.
SELECT TOP (20)
    *
FROM sys.query_store_runtime_stats
ORDER BY avg_duration DESC;
GO

-- Problem 2: this reviews plans without the query text that explains whether they belong to the workload you are investigating.
SELECT TOP (20)
    *
FROM sys.query_store_plan
ORDER BY plan_id DESC;
GO

-- Problem 3: this jumps to plan forcing language without first isolating one query, one plan history, and one rollback rule.