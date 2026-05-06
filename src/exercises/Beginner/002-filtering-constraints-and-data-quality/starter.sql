USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#ImportedUsers', N'U') IS NOT NULL
BEGIN
    DROP TABLE #ImportedUsers;
END;
GO

CREATE TABLE #ImportedUsers
(
    RowId INT NOT NULL PRIMARY KEY,
    UserName NVARCHAR(64) NULL,
    Email NVARCHAR(256) NULL
);
GO

INSERT INTO #ImportedUsers (RowId, UserName, Email)
VALUES
    (1, N'ada', N'ada@sqlacademy.local'),
    (2, N' ada ', N'ada@sqlacademy.local '),
    (3, N'grace', NULL),
    (4, N'', N'ops@sqlacademy.local'),
    (5, N'Margaret', N'margaret@sqlacademy.local'),
    (6, N'margaret', N' MARGARET@sqlacademy.local ');
GO