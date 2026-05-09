# SQL-First Day One

Use this guide when your first goal is a single successful query against `LearningDb`, not a full tour of the platform.

This is the lightest day-one route currently supported by the repository.

It still uses the same Docker-first environment as the main course, but it now supports a reduced-startup database-first path so you do not need the full API and observability stack before your first successful query.

## Use This Guide When

- you already know your way around a terminal and a SQL client
- you want to prove the database workflow first before reading the full learner route
- the main [How To Start](how-to-start.md) guide feels like too much context for your first hour

## Default Local Connection

Use [SQL Client Connection Guide](sql-client-connection-guide.md) if you want client-specific instructions for Azure Data Studio, SSMS, or `sqlcmd`.

If you keep the default `.env` values, use these connection details in your SQL client:

| Setting | Default Value |
| --- | --- |
| Server | `localhost,14333` |
| Database | `LearningDb` |
| User | `sa` |
| Password | `SqlAcademy_dev_2026!` |

If you changed `.env`, use your local values instead.

## First Goal

Do not try to "learn the repo" on day one.

The goal is only this:

1. start the local stack successfully
2. connect to SQL Server with your client
3. run one query that returns seeded data
4. inspect the six seeded tables without changing anything

## Startup Options

Use the smallest option that still proves the point you care about.

### Option A: Database-Only First Session

Use this when the only goal is a successful query against `LearningDb`.

In VS Code, run the task `sqlacademy: start database-first path`.

If you prefer the terminal directly, use:

```powershell
powershell -ExecutionPolicy Bypass -File infra/scripts/start-database-first.ps1
```

This task or script starts only SQL Server and the database bootstrap step.

### Option B: Full Learning Stack

Use this when you also want API readiness, telemetry, and the rest of the local platform.

```bash
docker compose up --build
```

## Minimal Day-One Loop

1. Review [../../.env.example](../../.env.example) so you know the default local ports and password.
2. Start the reduced database-first path with the task `sqlacademy: start database-first path` or `powershell -ExecutionPolicy Bypass -File infra/scripts/start-database-first.ps1`.
3. Connect to `LearningDb` with the default local SQL Server connection.
4. Run the three read-only queries below.
5. Stop for the day if those queries make sense. Treat that as a successful first session.
6. Only after that, use `docker compose up --build` when you want the full API and observability platform.

Do not open Grafana, Prometheus, EF Core code, or the performance labs until this database workflow already feels predictable.

## First Read-Only Queries

Use [Schema Quick Reference](schema-quick-reference.md) alongside these queries.

Use [SQL Syntax And Query Patterns](../sql/sql-syntax-and-query-patterns.md) if `SELECT`, `ORDER BY`, or the purpose of these operators is still fuzzier than the table layout itself.

```sql
SELECT COUNT(*) AS UserCount
FROM academy.Users;

SELECT Id, UserName, Email, CreatedUtc
FROM academy.Users
ORDER BY Id;

SELECT Id, Title, CreatedUtc
FROM academy.Posts
ORDER BY CreatedUtc DESC, Id DESC;
```

## What Success Looks Like

- `UserCount` returns `4`
- the user list returns `ada`, `grace`, `linus`, and `margaret`
- the posts list returns four seeded posts, newest first
- you can explain the difference between `academy.Users`, `academy.Posts`, and `academy.Comments` without guessing

## If You Get Stuck

- go back to [Phase 0: Local Setup](../phases/phase-0-local-setup.md) if the main problem is Docker, ports, or readiness
- use [SQL Client Connection Guide](sql-client-connection-guide.md) if your SQL client connection details are the only missing step
- use [Local Setup Troubleshooting](local-setup-troubleshooting.md) for Docker, port, readiness, and login failures
- use [Schema Quick Reference](schema-quick-reference.md) if the table relationships are still fuzzy
- use [SQL Syntax And Query Patterns](../sql/sql-syntax-and-query-patterns.md) if the query operators themselves are the part that still feels unfamiliar
- use [Learning Glossary](glossary.md) if terms such as readiness, projection, predicate, or deterministic order are slowing you down
- stop after the first successful `SELECT COUNT(*) FROM academy.Users` if everything else feels noisy; that still counts as progress

## What To Read Next

Once the first read-only session feels boring, continue in this order:

1. [How To Start](how-to-start.md)
2. [Phase 0: Local Setup](../phases/phase-0-local-setup.md)
3. [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md)
4. [Beginner 000: SQL Fundamentals And Safe Changes](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)