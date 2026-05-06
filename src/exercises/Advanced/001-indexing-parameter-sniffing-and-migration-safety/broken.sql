USE LearningDb;
GO

-- Problem 1: an index change without workload reasoning.
CREATE INDEX IX_Trades_Everything
ON academy.Trades (UserId, InstrumentId, TradedUtc, Quantity, Price, Side);
GO

-- Problem 2: a migration that may rewrite the whole table under load.
ALTER TABLE academy.Posts
ADD Summary NVARCHAR(512) NOT NULL;
GO

-- Problem 3: the procedure is executed for different selectivities with no plan review.
EXEC academy.GetTradesByUser @UserId = 1;
EXEC academy.GetTradesByUser @UserId = 999999;
GO