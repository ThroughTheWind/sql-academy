# Learning Docs

This folder is the learner-facing entry point for the academy.

If you are new to the repository, read [How To Start](how-to-start.md) first.

Use [Learning Glossary](glossary.md) when a term blocks progress, and clear the checkpoint section at the end of each lesson before moving on.

## Recommended Order

1. [How To Start](how-to-start.md)
2. [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md)
3. [Lesson 02: Joins And Aggregations](02-joins-and-aggregations.md)
4. [Lesson 03: Window Functions And Intermediate Querying](03-window-functions-and-intermediate-querying.md)
5. [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
6. [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md)
7. [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](06-indexing-execution-plans-and-parameter-sensitivity.md)
8. [Lesson 07: SQL Server Internals](07-sql-server-internals.md)
9. [Lesson 08: Operational Engineering And Release Safety](08-operational-engineering-and-release-safety.md)
10. [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md)
11. [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md)
12. [Lesson 11: Capstones And Interview Readiness](11-capstones-and-interview-readiness.md)
13. [Cumulative Review](cumulative-review.md)

## By Level

| Level | Lessons | Primary Exercises |
| --- | --- | --- |
| Beginner | [Lesson 01](01-sql-fundamentals.md), [Lesson 02](02-joins-and-aggregations.md) | [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md), [Beginner 002](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md) |
| Intermediate | [Lesson 03](03-window-functions-and-intermediate-querying.md), [Lesson 04](04-schema-design-and-migration-safety.md) | [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md), [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md) |
| Advanced | [Lesson 05](05-transactions-blocking-and-deadlocks.md), [Lesson 06](06-indexing-execution-plans-and-parameter-sensitivity.md), [Lesson 07](07-sql-server-internals.md) | [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md), [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md), [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) |
| Senior | [Lesson 08](08-operational-engineering-and-release-safety.md), [Lesson 09](09-ef-core-dapper-and-query-shape.md), [Lesson 10](10-observability-testing-and-performance-engineering.md), [Lesson 11](11-capstones-and-interview-readiness.md) | [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md), [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md), [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md), [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) |

## Supporting Guides

- [How To Start](how-to-start.md)
- [Learning Glossary](glossary.md)
- [Cumulative Review](cumulative-review.md)
- [Follow-Up Exercises By Level](follow-up-exercises-by-level.md)
- [Repository Improvement Suggestions](repository-improvement-suggestions.md)

## Validation Workflow

For validation-ready packs, run:

`powershell -ExecutionPolicy Bypass -File ../../infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift`