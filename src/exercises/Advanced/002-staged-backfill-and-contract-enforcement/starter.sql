USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#OrderContractTarget', N'U') IS NOT NULL
BEGIN
    DROP TABLE #OrderContractTarget;
END;
GO

CREATE TABLE #OrderContractTarget
(
    Id INT NOT NULL PRIMARY KEY,
    OrderNumber NVARCHAR(32) NOT NULL,
    ExternalReference NVARCHAR(64) NULL,
    NeedsBackfill BIT NOT NULL,
    CreatedUtc DATETIME2(3) NOT NULL
);
GO

INSERT INTO #OrderContractTarget (Id, OrderNumber, ExternalReference, NeedsBackfill, CreatedUtc)
VALUES
    (1, N'ORD-2025-0001', NULL, 1, '2025-01-20T08:45:00.000'),
    (2, N'ORD-2025-0002', N'EXT-2025-0002', 0, '2025-01-20T10:45:00.000'),
    (3, N'ORD-2025-0003', NULL, 1, '2025-01-21T09:00:00.000'),
    (4, N'ORD-2025-0004', NULL, 1, '2025-01-21T11:00:00.000');
GO