# SQL Track

This page is the SQL Server reference surface for the academy.

Start with [Curriculum Map](../learning/curriculum-map.md) if you want the ordered path rather than a topic reference.

Use this page when you already know the SQL concern you want to review and need the fastest route to the right lesson, phase, exercise, and repository anchor.

Do not use this page as the main curriculum index. It is a lookup surface, not the default course sequence.

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
- need API-facing SQL shape and stable paging: [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md) and [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)
- need tuning and plan reasoning: [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) and [Performance Track](../performance/README.md)
- need engine-behavior hypothesis work: [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)
- need production-style SQL triage: [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) and [Operations Track](../operations/README.md)

## Linked Exercises

- [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)
- [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)
- [Beginner 002](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md)
- [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)
- [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
- [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)
- [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
- [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)

Use [Phase Guides](../phases/README.md) for the ordered phase progression and [Exercise System](../exercises/README.md) for the hands-on ladder.