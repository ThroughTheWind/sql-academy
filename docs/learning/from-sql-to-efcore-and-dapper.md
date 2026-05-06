# From SQL To EF Core And Dapper

Use this guide right before [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md).

The goal is to make the application track feel like a continuation of the SQL track rather than a separate branch.

## What Carries Forward From The SQL Lessons

The abstractions change in Phase 8, but the important questions do not.

Keep asking:

- what is the base rowset?
- which filters reduce that rowset?
- which columns are actually needed?
- is the sort order deterministic?
- is pagination happening in the database or in memory?
- where can concurrency change the result?
- how would you inspect the generated SQL and the resulting plan?

## Topic Bridge

| Earlier SQL Topic | Application-Level Question | Repository Anchor |
| --- | --- | --- |
| [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md) | Does the code project only the rows and columns it needs? | [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs) |
| [Lesson 02: Joins And Aggregations](02-joins-and-aggregations.md) | Does the query shape preserve the intended row cardinality when counts or related data are involved? | [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs) |
| [Lesson 03: Window Functions And Intermediate Querying](03-window-functions-and-intermediate-querying.md) | Is sorting deterministic and is paging still happening in SQL Server? | [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs) and [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs) |
| [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md) | Would this model or migration change still be safe to roll forward? | [SqlAcademy.Migrations](../../src/libs/SqlAcademy.Migrations) |
| [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md) | Where does optimistic concurrency live and how is the conflict surfaced? | [OrderConfiguration](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs) |
| [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](06-indexing-execution-plans-and-parameter-sensitivity.md) | What SQL does the abstraction produce, and how would you detect a bad plan or regression? | [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), and [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) |

## How To Use The Bridge

1. Start from the SQL question, not from the library name.
2. Read the application code and predict the SQL shape in plain English.
3. Inspect the generated SQL or explicit SQL text.
4. Compare the actual query shape with the result contract, sort order, and concurrency behavior you expected.
5. Only then decide whether EF Core, Dapper, or a different query shape is the right tool.

## A Good Pre-Lesson 09 Workflow

1. Revisit [Lesson 03](03-window-functions-and-intermediate-querying.md), [Lesson 05](05-transactions-blocking-and-deadlocks.md), and [Lesson 06](06-indexing-execution-plans-and-parameter-sensitivity.md).
2. Read this bridge and restate one EF Core read path and one Dapper read path in SQL terms.
3. Read [Phase 8: .NET And EF Core Integration](../phases/phase-8-dotnet-and-efcore-integration.md).
4. Continue to [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md).
5. Use [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) as the first applied lab bundle, starting with [Lab 01](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-01-efcore-posts-read-path.md).

## What To Watch For

- elegant LINQ that still produces unstable or heavy SQL
- pagination code without a deterministic tie-breaker
- full-entity materialization when a projection would do
- rowversion handling that exists in configuration but is not handled in the write path
- migration convenience that hides rollout risk

If any of those feel familiar, that is the point: the SQL lessons are still the foundation.