:setvar DatabaseName "LearningDb"

USE [$(DatabaseName)];
GO

INSERT INTO academy.Users (UserName, Email, CreatedUtc)
SELECT N'ada', N'ada@sqlacademy.local', '2025-01-15T08:30:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Users WHERE UserName = N'ada');

INSERT INTO academy.Users (UserName, Email, CreatedUtc)
SELECT N'grace', N'grace@sqlacademy.local', '2025-01-15T08:35:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Users WHERE UserName = N'grace');

INSERT INTO academy.Users (UserName, Email, CreatedUtc)
SELECT N'linus', N'linus@sqlacademy.local', '2025-01-15T08:40:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Users WHERE UserName = N'linus');

INSERT INTO academy.Users (UserName, Email, CreatedUtc)
SELECT N'margaret', N'margaret@sqlacademy.local', '2025-01-15T08:45:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Users WHERE UserName = N'margaret');
GO

INSERT INTO academy.Instruments (Symbol, Name, AssetClass, TickSize, LotSize, CreatedUtc)
SELECT N'MSFT', N'Microsoft Corporation', N'Equity', 0.0100, 1.0000, '2025-01-15T08:30:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Instruments WHERE Symbol = N'MSFT');

INSERT INTO academy.Instruments (Symbol, Name, AssetClass, TickSize, LotSize, CreatedUtc)
SELECT N'AAPL', N'Apple Inc.', N'Equity', 0.0100, 1.0000, '2025-01-15T08:31:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Instruments WHERE Symbol = N'AAPL');

INSERT INTO academy.Instruments (Symbol, Name, AssetClass, TickSize, LotSize, CreatedUtc)
SELECT N'EURUSD', N'Euro / US Dollar', N'ForeignExchange', 0.0001, 1000.0000, '2025-01-15T08:32:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Instruments WHERE Symbol = N'EURUSD');

INSERT INTO academy.Instruments (Symbol, Name, AssetClass, TickSize, LotSize, CreatedUtc)
SELECT N'CL', N'WTI Crude Oil', N'Commodity', 0.0100, 1000.0000, '2025-01-15T08:33:00.000'
WHERE NOT EXISTS (SELECT 1 FROM academy.Instruments WHERE Symbol = N'CL');
GO