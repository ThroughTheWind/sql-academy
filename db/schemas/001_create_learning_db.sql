:setvar DatabaseName "LearningDb"

IF DB_ID('$(DatabaseName)') IS NULL
BEGIN
    EXEC('CREATE DATABASE [$(DatabaseName)]');
END
GO

USE [$(DatabaseName)];
GO

IF SCHEMA_ID(N'academy') IS NULL
BEGIN
    EXEC(N'CREATE SCHEMA academy AUTHORIZATION dbo');
END
GO

IF OBJECT_ID(N'academy.Users', N'U') IS NULL
BEGIN
    CREATE TABLE academy.Users
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
        UserName NVARCHAR(64) NOT NULL,
        Email NVARCHAR(256) NOT NULL,
        CreatedUtc DATETIME2(3) NOT NULL CONSTRAINT DF_Users_CreatedUtc DEFAULT SYSUTCDATETIME()
    );

    CREATE UNIQUE INDEX UX_Users_UserName ON academy.Users (UserName);
    CREATE UNIQUE INDEX UX_Users_Email ON academy.Users (Email);
END
GO

IF OBJECT_ID(N'academy.Instruments', N'U') IS NULL
BEGIN
    CREATE TABLE academy.Instruments
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Instruments PRIMARY KEY,
        Symbol NVARCHAR(24) NOT NULL,
        Name NVARCHAR(256) NOT NULL,
        AssetClass NVARCHAR(32) NOT NULL,
        TickSize DECIMAL(18,4) NOT NULL,
        LotSize DECIMAL(18,4) NOT NULL,
        CreatedUtc DATETIME2(3) NOT NULL CONSTRAINT DF_Instruments_CreatedUtc DEFAULT SYSUTCDATETIME()
    );

    CREATE UNIQUE INDEX UX_Instruments_Symbol ON academy.Instruments (Symbol);
END
GO

IF OBJECT_ID(N'academy.Posts', N'U') IS NULL
BEGIN
    CREATE TABLE academy.Posts
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Posts PRIMARY KEY,
        UserId INT NOT NULL,
        Title NVARCHAR(256) NOT NULL,
        Body NVARCHAR(4000) NOT NULL,
        CreatedUtc DATETIME2(3) NOT NULL CONSTRAINT DF_Posts_CreatedUtc DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Posts_Users_UserId FOREIGN KEY (UserId) REFERENCES academy.Users (Id)
    );

    CREATE INDEX IX_Posts_UserId_CreatedUtc ON academy.Posts (UserId, CreatedUtc DESC);
    CREATE INDEX IX_Posts_Title ON academy.Posts (Title);
END
GO

IF OBJECT_ID(N'academy.Comments', N'U') IS NULL
BEGIN
    CREATE TABLE academy.Comments
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Comments PRIMARY KEY,
        PostId INT NOT NULL,
        UserId INT NOT NULL,
        Body NVARCHAR(2000) NOT NULL,
        CreatedUtc DATETIME2(3) NOT NULL CONSTRAINT DF_Comments_CreatedUtc DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Comments_Posts_PostId FOREIGN KEY (PostId) REFERENCES academy.Posts (Id) ON DELETE CASCADE,
        CONSTRAINT FK_Comments_Users_UserId FOREIGN KEY (UserId) REFERENCES academy.Users (Id)
    );

    CREATE INDEX IX_Comments_PostId_CreatedUtc ON academy.Comments (PostId, CreatedUtc DESC);
    CREATE INDEX IX_Comments_UserId_CreatedUtc ON academy.Comments (UserId, CreatedUtc DESC);
END
GO

IF OBJECT_ID(N'academy.Orders', N'U') IS NULL
BEGIN
    CREATE TABLE academy.Orders
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Orders PRIMARY KEY,
        UserId INT NOT NULL,
        OrderNumber NVARCHAR(32) NOT NULL,
        Status NVARCHAR(32) NOT NULL,
        TotalAmount DECIMAL(18,2) NOT NULL,
        CreatedUtc DATETIME2(3) NOT NULL CONSTRAINT DF_Orders_CreatedUtc DEFAULT SYSUTCDATETIME(),
        UpdatedUtc DATETIME2(3) NOT NULL CONSTRAINT DF_Orders_UpdatedUtc DEFAULT SYSUTCDATETIME(),
        RowVersion ROWVERSION NOT NULL,
        CONSTRAINT FK_Orders_Users_UserId FOREIGN KEY (UserId) REFERENCES academy.Users (Id)
    );

    CREATE UNIQUE INDEX UX_Orders_OrderNumber ON academy.Orders (OrderNumber);
    CREATE INDEX IX_Orders_UserId_CreatedUtc ON academy.Orders (UserId, CreatedUtc DESC);
END
GO

IF OBJECT_ID(N'academy.Trades', N'U') IS NULL
BEGIN
    CREATE TABLE academy.Trades
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Trades PRIMARY KEY,
        UserId INT NOT NULL,
        InstrumentId INT NOT NULL,
        Side NVARCHAR(16) NOT NULL,
        Quantity DECIMAL(18,4) NOT NULL,
        Price DECIMAL(18,4) NOT NULL,
        TradedUtc DATETIME2(3) NOT NULL,
        CONSTRAINT FK_Trades_Users_UserId FOREIGN KEY (UserId) REFERENCES academy.Users (Id),
        CONSTRAINT FK_Trades_Instruments_InstrumentId FOREIGN KEY (InstrumentId) REFERENCES academy.Instruments (Id)
    );

    CREATE INDEX IX_Trades_UserId_TradedUtc ON academy.Trades (UserId, TradedUtc DESC);
    CREATE INDEX IX_Trades_InstrumentId_TradedUtc ON academy.Trades (InstrumentId, TradedUtc DESC);
END
GO