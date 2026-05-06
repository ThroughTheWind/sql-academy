USE LearningDb;
GO

IF OBJECT_ID(N'tempdb..#lesson01_candidate_users', N'U') IS NOT NULL
BEGIN
    DROP TABLE #lesson01_candidate_users;
END;

CREATE TABLE #lesson01_candidate_users
(
    CandidateRowId INT NOT NULL PRIMARY KEY,
    UserName NVARCHAR(64) NULL,
    Email NVARCHAR(256) NULL
);
GO

INSERT INTO #lesson01_candidate_users (CandidateRowId, UserName, Email)
VALUES
    (1, N'edith', N'edith@sqlacademy.local'),
    (2, N'ada', N'ada-duplicate@sqlacademy.local'),
    (3, N'newhire', N'ada@sqlacademy.local'),
    (4, NULL, N'missing-user@sqlacademy.local'),
    (5, N'noemail', NULL);
GO