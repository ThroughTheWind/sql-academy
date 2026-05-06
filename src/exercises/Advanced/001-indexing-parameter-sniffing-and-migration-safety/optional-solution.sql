USE LearningDb;
GO

CREATE INDEX IX_Trades_UserId_TradedUtc_Include
ON academy.Trades (UserId, TradedUtc DESC)
INCLUDE (InstrumentId, Quantity, Price, Side);
GO

ALTER TABLE academy.Posts
ADD Summary NVARCHAR(512) NULL;
GO

UPDATE academy.Posts
SET Summary = LEFT(Body, 512)
WHERE Summary IS NULL;
GO

ALTER TABLE academy.Posts
ALTER COLUMN Summary NVARCHAR(512) NOT NULL;
GO