# Curriculum Map

This is the authoritative guided route for the academy.

If another index or summary table disagrees with this file, follow this file.

## Default Guided Route

1. [How To Start](how-to-start.md)
2. [Phase 0: Local Setup](../phases/phase-0-local-setup.md)
3. [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md)
4. [Lesson 02: Joins And Aggregations](02-joins-and-aggregations.md)
5. [Lesson 03: Window Functions And Intermediate Querying](03-window-functions-and-intermediate-querying.md)
6. [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
7. [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md)
8. [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](06-indexing-execution-plans-and-parameter-sensitivity.md)
9. [Lesson 07: SQL Server Internals](07-sql-server-internals.md)
10. [Lesson 08: Operational Engineering And Release Safety](08-operational-engineering-and-release-safety.md)
11. [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md)
12. [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md)
13. [Lesson 11: Capstones And Interview Readiness](11-capstones-and-interview-readiness.md)
14. [Cumulative Review](cumulative-review.md)

## Assessment Packs

Use these when you want a level-scoped checkpoint instead of the full mixed review:

- [Beginner Assessment Pack](beginner-assessment-pack.md)
- [Intermediate Assessment Pack](intermediate-assessment-pack.md)
- [Advanced Assessment Pack](advanced-assessment-pack.md)
- [Senior Assessment Pack](senior-assessment-pack.md)

## Practice Matrix

Use this matrix when you want to know what to do after reading a lesson.

| Phase | Core Material | Primary Practice Surface | Validation Mode | Notes |
| --- | --- | --- | --- | --- |
| 0 | [Phase 0: Local Setup](../phases/phase-0-local-setup.md) | seeded-data inspection, API health checks, and observability endpoints | checklist | required before later exercises |
| 1 | [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md) | [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) | validation pack | projection, filtering, deterministic ordering, and constraint-aware reasoning |
| 2 | [Lesson 02: Joins And Aggregations](02-joins-and-aggregations.md) | [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md) | validation pack | first joins and aggregations pack |
| 2 | [Lesson 03: Window Functions And Intermediate Querying](03-window-functions-and-intermediate-querying.md) | [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md) | validation pack | stable paging and window functions |
| 2 | [Lesson 03 follow-up](03-window-functions-and-intermediate-querying.md) | [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md) | validation pack | cohort analysis and pagination drift |
| 3 | [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md) | [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md) | validation pack | apply this after you understand forward-only change patterns from the lesson |
| 4 | [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md) | [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) | validation pack | concurrency and deadlock reasoning |
| 5 | [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](06-indexing-execution-plans-and-parameter-sensitivity.md) | [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) | validation pack | indexing, plan stability, and rollout safety |
| 6 | [Lesson 07: SQL Server Internals](07-sql-server-internals.md) | [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) | guided lab | tagged workload, plan cache, storage pages, and wait signals |
| 7 | [Lesson 08: Operational Engineering And Release Safety](08-operational-engineering-and-release-safety.md) | [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) | investigation pack | combines migration safety, telemetry watch points, and rollback or roll-forward gates before the release window starts |
| 8 | [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md) | [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) and [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) | guided lab bundle and guided lab | Senior 002 covers the main EF Core, Dapper, rowversion, and staged-ingestion route; Senior 005 is the focused N+1 and generated-SQL follow-up |
| 8 | [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md) | [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) and [SqlAcademy.IntegrationTests](../../tests/SqlAcademy.IntegrationTests) | investigation and code review | combines observability with executable validation surfaces |
| 9 | [Lesson 11: Capstones And Interview Readiness](11-capstones-and-interview-readiness.md) | [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) plus a capstone from [Phase 9](../phases/phase-9-capstone-projects.md) | validation pack and capstone | final synthesis |

## Exercise Modes

- `validation pack`: uses `starter.sql`, `answer.sql`, and `validation.sql` so the learner can self-check quickly.
- `guided lab`: uses lesson walkthroughs, SQL scripts, or code changes with explicit review prompts but no answer-template contract yet.
- `investigation pack`: uses evidence files, workbook templates, and narrative output instead of a single SQL answer file.                                                                                                                                                      

## Optional On-Ramps

The default route above is the main course.

Use these only if you already know your gap and you are willing to rejoin the main route later.

| Goal | Start Here | Rejoin Main Route At |
| --- | --- | --- |
| SQL refresher | [Lesson 02: Joins And Aggregations](02-joins-and-aggregations.md) | [Lesson 03](03-window-functions-and-intermediate-querying.md) |
| Performance and production focus | [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md) | [Lesson 08](08-operational-engineering-and-release-safety.md) |
| EF Core and Dapper focus | [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md) | [Lesson 10](10-observability-testing-and-performance-engineering.md) and [Lesson 11](11-capstones-and-interview-readiness.md) |

## Route Status

- Lesson 01 now uses [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) as the dedicated fundamentals validation pack.
- Lesson 07 now uses [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) as the concrete internals lab.
- Lesson 08 now uses [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) as the release-readiness investigation pack.
- Lesson 09 now uses [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) as the main guided lab bundle and [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) as the focused N+1 and generated-SQL follow-up lab.