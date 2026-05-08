# SQL Track

This page is the SQL Server reference surface for the academy.

Start with [Curriculum Map](../learning/curriculum-map.md) if you want the ordered path rather than a topic reference.

Use this page when you already know the SQL concern you want to review and need the fastest route to the right lesson, phase, exercise, and repository anchor.

Do not use this page as the main curriculum index. It is a lookup surface, not the default course sequence.

## Scope Boundary

The current SQL route targets backend and application engineers who need strong SQL Server fluency for schema design, query shape, performance, concurrency, and production troubleshooting.

It does not currently claim full DBA or DBRE coverage for backup and restore, SQL Server Agent, HA/DR, replication, or security administration.

Those deeper administration topics now route through [DBA And DBRE Extension Track](../learning/dba-dbre-extension-track.md) after the core route is stronger. The tracked implementation backlog lives in [../../.ai/sql-efcore-mastery-backlog.md](../../.ai/sql-efcore-mastery-backlog.md).

Coverage targets:
- filtering, projection, joins, grouping, and set-based thinking
- CTEs, window functions, and query decomposition
- schema design, constraints, and relational integrity
- isolation levels, locking, blocking, and deadlocks
- indexing, statistics, and execution-plan interpretation
- internals, waits, and production operations

## When To Use This Page

- when you need the nearest lesson and exercise for a SQL concern you already recognize
- when you want to jump from a production symptom to the most relevant SQL asset or test
- when you want to cross-reference SQL lessons with performance or operations docs

## When Not To Use This Page

- when you are new to the repository and need the main order; use [Curriculum Map](../learning/curriculum-map.md)
- when the main question is about LINQ, EF Core tracking, or EF-plus-Dapper boundaries; use [EF Core Track](../efcore/README.md) and [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md)
- when the main question is backup and restore, SQL Server Agent, HA or DR, or security specialization; use [DBA And DBRE Extension Track](../learning/dba-dbre-extension-track.md)

## SQL Reference Matrix

| Concern | Main Teaching Surface | Phase | Primary Practice | Concrete Repo Anchors | Validation Surface |
| --- | --- | --- | --- | --- | --- |
| fundamentals, deterministic ordering, and safe changes | [Lesson 01](../learning/01-sql-fundamentals.md) | [Phase 1](../phases/phase-1-sql-fundamentals.md) | [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) | [schema bootstrap](../../db/schemas/001_create_learning_db.sql), [seed data](../../db/seed/002_seed_social_and_orders.sql) | validation pack |
| joins and grouped correctness | [Lesson 02](../learning/02-joins-and-aggregations.md) | [Phase 2](../phases/phase-2-intermediate-querying.md) | [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md) | [schema bootstrap](../../db/schemas/001_create_learning_db.sql), [social seed data](../../db/seed/002_seed_social_and_orders.sql) | validation pack |
| window functions and stable pagination | [Lesson 03](../learning/03-window-functions-and-intermediate-querying.md) | [Phase 2](../phases/phase-2-intermediate-querying.md) | [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md), [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md) | [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) | validation packs |
| schema design and migration safety | [Lesson 04](../learning/04-schema-design-and-migration-safety.md) | [Phase 3](../phases/phase-3-schema-design.md) | [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md) | [migration strategy](../../db/migrations/README.md), [SqlAcademy.Migrations](../../src/libs/SqlAcademy.Migrations/SqlAcademy.Migrations.csproj) | validation pack |
| transactions, blocking, and deadlocks | [Lesson 05](../learning/05-transactions-blocking-and-deadlocks.md) | [Phase 4](../phases/phase-4-transactions-and-concurrency.md) | [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) | [schema bootstrap](../../db/schemas/001_create_learning_db.sql), [operations track](../operations/README.md) | validation pack |
| indexing, plans, and parameter sensitivity | [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md) | [Phase 5](../phases/phase-5-indexing-and-performance.md) | [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) | [performance labs](../performance/README.md), [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) | validation pack |
| plan cache, memory grants, and waits | [Lesson 07](../learning/07-sql-server-internals.md) | [Phase 6](../phases/phase-6-sql-server-internals.md) | [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) | [performance lab notes](../../db/performance/README.md), [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs) | guided lab |
| operational SQL and release safety | [Lesson 08](../learning/08-operational-engineering-and-release-safety.md) | [Phase 7](../phases/phase-7-operational-engineering.md) | [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | [operations track](../operations/README.md), [database init script](../../infra/scripts/init-database.sh) | investigation pack |

## Best Entry Points

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Lesson 03: Window Functions And Intermediate Querying](../learning/03-window-functions-and-intermediate-querying.md)
- [Lesson 05: Transactions, Blocking, And Deadlocks](../learning/05-transactions-blocking-and-deadlocks.md)
- [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md)
- [Lesson 07: SQL Server Internals](../learning/07-sql-server-internals.md)

## Fast Jumps By Need

- need the first hands-on SQL correctness pack: [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)
- need schema design and rollout-safe change practice: [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md) and [db/migrations](../../db/migrations/README.md)
- need API-facing SQL shape and stable paging: [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md) and [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)
- need concurrency, blocking, or deadlock reasoning: [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) and [Operations Track](../operations/README.md)
- need tuning and plan reasoning: [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) and [Performance Track](../performance/README.md)
- need engine-behavior hypothesis work: [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)
- need persisted regression evidence and Query Store workflow: [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- need production-style SQL triage: [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) and [Operations Track](../operations/README.md)
- need DBA or DBRE specialization beyond the core route: [DBA And DBRE Extension Track](../learning/dba-dbre-extension-track.md)

## Executable Repo Anchors

The SQL route is supported by concrete code and tests so the learner can validate a claim instead of stopping at theory.

- [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs) is the clearest explicit SQL and paging anchor in the application code
- [TradeImportBatchReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeImportBatchReadService.cs) shows queryable staged-ingestion review and batch-history access
- [TradesEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs) validate the application-facing contract on a SQL-shaped read path
- [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) compare equivalent EF Core and Dapper work against the same underlying trade-page scenario
- [QueryStoreLabSmokeTests](../../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs) verify the Query Store lab captures the tagged workload it is supposed to diagnose
- [DeadlockReproductionTests](../../tests/SqlAcademy.PerformanceTests/DeadlockReproductionTests.cs) intentionally hands off concurrency reproduction to the guided lab rather than pretending a skipped test is sufficient
- [db/performance](../../db/performance/README.md) is the scratch surface for ad hoc SQL Server performance scripts that support the formal packs

## Working Sequence In This Repo

Use this page with a SQL-first workflow:

1. name the exact result contract or production symptom you are trying to explain
2. locate the owning query, schema object, or exercise pack before changing anything
3. validate the current shape against seed data, tests, or a focused lab surface
4. collect the relevant evidence such as result shape, plan, Query Store history, or concurrency symptoms
5. make one SQL or index change at a time
6. rerun the same validation surface before moving into broader application or operational questions

## Fast Validation Commands

Use these when you want an executable anchor instead of a doc-only reminder:

```bash
powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Beginner/001-joins-and-aggregations
powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Senior/001-concurrency-blocking-and-deadlocks
dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~TradesEndpointTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryPerformanceComparisonTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryStoreLabSmokeTests"
```

## Bridge To Performance, Operations, And EF Core

This page stays closest to raw SQL reasoning, but it is not isolated from the other tracks.

- use [Performance Track](../performance/README.md) when the question becomes measurement discipline, Query Store, waits, benchmarks, or regression evidence
- use [Operations Track](../operations/README.md) when the same SQL issue becomes a release, telemetry, or incident-response problem
- use [EF Core Track](../efcore/README.md) when the underlying SQL behavior is now being expressed through LINQ, `AsNoTracking()`, `rowversion`, or hybrid EF-plus-Dapper code

## Linked Exercises

- [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)
- [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)
- [Beginner 002](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md)
- [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)
- [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
- [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)
- [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
- [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)

Use [Phase Guides](../phases/README.md) for the ordered phase progression and [Exercise System](../exercises/README.md) for the hands-on ladder.