USE LearningDb;
GO

SET NOCOUNT ON;
GO

ALTER DATABASE CURRENT SET QUERY_STORE = ON;
GO

ALTER DATABASE CURRENT SET QUERY_STORE
(
    OPERATION_MODE = READ_WRITE,
    QUERY_CAPTURE_MODE = ALL,
    INTERVAL_LENGTH_MINUTES = 1,
    MAX_STORAGE_SIZE_MB = 256
);
GO

ALTER DATABASE CURRENT SET QUERY_STORE CLEAR;
GO

DECLARE @AdaId INT = (SELECT Id FROM academy.Users WHERE UserName = N'ada');
DECLARE @GraceId INT = (SELECT Id FROM academy.Users WHERE UserName = N'grace');
DECLARE @Search NVARCHAR(100) = N'window';

EXEC sp_executesql
    N'SELECT /* query_store_lab_trade_lookup */ COUNT_BIG(*) AS TradeCount FROM academy.Trades WHERE UserId = @UserId;',
    N'@UserId INT',
    @UserId = @AdaId;
GO

EXEC sp_executesql
    N'SELECT /* query_store_lab_trade_lookup */ COUNT_BIG(*) AS TradeCount FROM academy.Trades WHERE UserId = @UserId;',
    N'@UserId INT',
    @UserId = @GraceId;
GO

EXEC sp_executesql
    N'
SELECT /* query_store_lab_post_search */ TOP (10)
    p.Id,
    COUNT(c.Id) AS CommentCount
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c ON c.PostId = p.Id
WHERE p.Title LIKE N''%'' + @Search + N''%''
   OR p.Body LIKE N''%'' + @Search + N''%''
GROUP BY p.Id
ORDER BY CommentCount DESC, p.Id DESC;',
    N'@Search NVARCHAR(100)',
    @Search = @Search;
GO

EXEC sys.sp_query_store_flush_db;
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
WHERE qt.query_sql_text LIKE N'%query_store_lab_%'
ORDER BY rs.last_execution_time DESC, q.query_id DESC, p.plan_id DESC;
GO