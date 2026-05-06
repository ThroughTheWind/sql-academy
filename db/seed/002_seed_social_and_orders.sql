:setvar DatabaseName "LearningDb"

USE [$(DatabaseName)];
GO

DECLARE @AdaId INT = (SELECT Id FROM academy.Users WHERE UserName = N'ada');
DECLARE @GraceId INT = (SELECT Id FROM academy.Users WHERE UserName = N'grace');
DECLARE @LinusId INT = (SELECT Id FROM academy.Users WHERE UserName = N'linus');
DECLARE @MargaretId INT = (SELECT Id FROM academy.Users WHERE UserName = N'margaret');

DECLARE @MsftId INT = (SELECT Id FROM academy.Instruments WHERE Symbol = N'MSFT');
DECLARE @AaplId INT = (SELECT Id FROM academy.Instruments WHERE Symbol = N'AAPL');
DECLARE @EurUsdId INT = (SELECT Id FROM academy.Instruments WHERE Symbol = N'EURUSD');
DECLARE @ClId INT = (SELECT Id FROM academy.Instruments WHERE Symbol = N'CL');

INSERT INTO academy.Posts (UserId, Title, Body, CreatedUtc)
SELECT @AdaId, N'Understanding clustered indexes', N'Clustered indexes define the physical ordering of rows and strongly influence seek and scan tradeoffs.', '2025-01-16T08:30:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Posts WHERE Title = N'Understanding clustered indexes');

INSERT INTO academy.Posts (UserId, Title, Body, CreatedUtc)
SELECT @GraceId, N'When to prefer window functions', N'Window functions let you rank, partition, and aggregate without losing row-level detail.', '2025-01-17T08:30:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Posts WHERE Title = N'When to prefer window functions');

INSERT INTO academy.Posts (UserId, Title, Body, CreatedUtc)
SELECT @LinusId, N'Concurrency surprises in OLTP systems', N'Blocking chains usually begin with one innocent transaction held open too long.', '2025-01-18T08:30:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Posts WHERE Title = N'Concurrency surprises in OLTP systems');

INSERT INTO academy.Posts (UserId, Title, Body, CreatedUtc)
SELECT @MargaretId, N'Operational playbooks for SQL releases', N'Practice safe migrations, smoke tests, metrics checks, and rollback drills.', '2025-01-19T08:30:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Posts WHERE Title = N'Operational playbooks for SQL releases');
GO

DECLARE @ClusteredIndexesPostId INT = (SELECT Id FROM academy.Posts WHERE Title = N'Understanding clustered indexes');
DECLARE @WindowFunctionsPostId INT = (SELECT Id FROM academy.Posts WHERE Title = N'When to prefer window functions');
DECLARE @ConcurrencyPostId INT = (SELECT Id FROM academy.Posts WHERE Title = N'Concurrency surprises in OLTP systems');

INSERT INTO academy.Comments (PostId, UserId, Body, CreatedUtc)
SELECT @ClusteredIndexesPostId, @GraceId, N'Include fill factor and fragmentation maintenance tradeoffs in the review notes.', '2025-01-16T10:30:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Comments WHERE PostId = @ClusteredIndexesPostId AND UserId = @GraceId);

INSERT INTO academy.Comments (PostId, UserId, Body, CreatedUtc)
SELECT @ClusteredIndexesPostId, @LinusId, N'Also highlight lookup amplification on wide rows.', '2025-01-16T11:30:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Comments WHERE PostId = @ClusteredIndexesPostId AND UserId = @LinusId);

INSERT INTO academy.Comments (PostId, UserId, Body, CreatedUtc)
SELECT @WindowFunctionsPostId, @AdaId, N'A running total example usually makes the concept click quickly.', '2025-01-17T09:15:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Comments WHERE PostId = @WindowFunctionsPostId AND UserId = @AdaId);

INSERT INTO academy.Comments (PostId, UserId, Body, CreatedUtc)
SELECT @ConcurrencyPostId, @MargaretId, N'This topic should end with a deadlock graph walkthrough.', '2025-01-18T09:45:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Comments WHERE PostId = @ConcurrencyPostId AND UserId = @MargaretId);
GO

INSERT INTO academy.Orders (UserId, OrderNumber, Status, TotalAmount, CreatedUtc, UpdatedUtc)
SELECT @AdaId, N'ORD-2025-0001', N'Filled', 512.40, '2025-01-20T08:45:00.000', '2025-01-20T08:50:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Orders WHERE OrderNumber = N'ORD-2025-0001');

INSERT INTO academy.Orders (UserId, OrderNumber, Status, TotalAmount, CreatedUtc, UpdatedUtc)
SELECT @GraceId, N'ORD-2025-0002', N'Submitted', 980.10, '2025-01-20T10:45:00.000', '2025-01-20T10:47:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Orders WHERE OrderNumber = N'ORD-2025-0002');

INSERT INTO academy.Orders (UserId, OrderNumber, Status, TotalAmount, CreatedUtc, UpdatedUtc)
SELECT @LinusId, N'ORD-2025-0003', N'Pending', 143.00, '2025-01-21T09:00:00.000', '2025-01-21T09:01:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Orders WHERE OrderNumber = N'ORD-2025-0003');
GO

INSERT INTO academy.Trades (UserId, InstrumentId, Side, Quantity, Price, TradedUtc)
SELECT @AdaId, @MsftId, N'Buy', 100.0000, 420.5000, '2025-01-20T09:15:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Trades WHERE UserId = @AdaId AND InstrumentId = @MsftId AND TradedUtc = '2025-01-20T09:15:00.000');

INSERT INTO academy.Trades (UserId, InstrumentId, Side, Quantity, Price, TradedUtc)
SELECT @GraceId, @AaplId, N'Buy', 25.0000, 187.2000, '2025-01-20T09:25:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Trades WHERE UserId = @GraceId AND InstrumentId = @AaplId AND TradedUtc = '2025-01-20T09:25:00.000');

INSERT INTO academy.Trades (UserId, InstrumentId, Side, Quantity, Price, TradedUtc)
SELECT @LinusId, @EurUsdId, N'Sell', 25000.0000, 1.0842, '2025-01-21T09:10:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Trades WHERE UserId = @LinusId AND InstrumentId = @EurUsdId AND TradedUtc = '2025-01-21T09:10:00.000');

INSERT INTO academy.Trades (UserId, InstrumentId, Side, Quantity, Price, TradedUtc)
SELECT @MargaretId, @ClId, N'Buy', 2.0000, 77.8500, '2025-01-21T09:45:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Trades WHERE UserId = @MargaretId AND InstrumentId = @ClId AND TradedUtc = '2025-01-21T09:45:00.000');
GO