# Effort And Pacing Guide

Use this page when you want one planning view for the current course instead of collecting time estimates from each lesson individually.

These ranges are planning aids, not deadlines.

They assume:

- your local environment already works or can be made to work with normal setup effort
- you are comfortable with basic developer workflows such as terminals, source control, and opening code or SQL files
- you are studying with focus rather than multitasking heavily

## What "Beginner" Means Here

In this repository, `beginner` means beginner to SQL Server and relational reasoning inside a working engineering environment.

It does not mean beginner to software development, terminals, version control, or first-time local environment setup.

## How To Use These Estimates

- use the low end when the topic is familiar and you mainly need repository context
- use the high end when the topic is new or when you stop to verify examples carefully
- treat exercises and reviews as separate time blocks from the lesson docs themselves

## Entry And Early Route

| Surface | Typical Focused Time | Notes |
| --- | --- | --- |
| [SQL-First Day One](sql-first-day-one.md) | 20 to 45 minutes | smallest supported first-query session |
| [How To Start](how-to-start.md) | 20 to 30 minutes | route selection and study-loop setup |
| [SQL To .NET Data Access Path](sql-to-dotnet-data-access-path.md) | 20 to 40 minutes | targeted route selection when SQL fundamentals already feel predictable |
| [Phase 0: Local Setup](../phases/phase-0-local-setup.md) | 30 to 60 minutes | longer when Docker or SQL client setup is new |
| [Lesson 01](01-sql-fundamentals.md) | 60 to 90 minutes | read plus baseline inspection queries |
| [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) | 45 to 75 minutes | first validation pack |
| [Lesson 02](02-joins-and-aggregations.md) | 60 to 90 minutes | joins, row multiplication, and grouped reasoning |
| [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md) | 45 to 75 minutes | join and aggregation validation pack |
| [Beginner 002](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md) | 45 to 75 minutes | filtering and normalization validation pack |
| [Beginner Assessment Pack](beginner-assessment-pack.md) | 30 to 45 minutes | checkpoint after Lessons 01 and 02 |

## Core Lesson Estimates

| Surface | Typical Focused Time | Notes |
| --- | --- | --- |
| [Lesson 03](03-window-functions-and-intermediate-querying.md) | 75 to 120 minutes | first real analytical SQL jump |
| [Lesson 04](04-schema-design-and-migration-safety.md) | 75 to 120 minutes | design plus rollout safety |
| [Lesson 05](05-transactions-blocking-and-deadlocks.md) | 75 to 120 minutes | concurrency and wait-cycle reasoning |
| [Lesson 06](06-indexing-execution-plans-and-parameter-sensitivity.md) | 75 to 120 minutes | evidence-based tuning |
| [Lesson 07](07-sql-server-internals.md) | 75 to 120 minutes | practical internals, not trivia |
| [Lesson 08](08-operational-engineering-and-release-safety.md) | 75 to 120 minutes | rollout, smoke checks, and observation |
| [Lesson 09](09-ef-core-dapper-and-query-shape.md) | 75 to 120 minutes | application query-shape translation |
| [Lesson 10](10-observability-testing-and-performance-engineering.md) | 75 to 120 minutes | evidence surfaces and regression thinking |
| [Lesson 11](11-capstones-and-interview-readiness.md) | 60 to 120 minutes | synthesis and answer structure |
| [Cumulative Review](cumulative-review.md) | 45 to 90 minutes | mixed-topic checkpoint |

## Intermediate And Review Pack Estimates

| Surface | Typical Focused Time | Notes |
| --- | --- | --- |
| [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md) | 60 to 90 minutes | first window-function validation pack |
| [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md) | 60 to 90 minutes | cohort reporting plus deterministic paging |
| [Intermediate Assessment Pack](intermediate-assessment-pack.md) | 30 to 45 minutes | checkpoint after Lessons 03 and 04 |
| [Advanced Assessment Pack](advanced-assessment-pack.md) | 45 to 60 minutes | checkpoint after Lessons 05 through 07 |
| [Senior Assessment Pack](senior-assessment-pack.md) | 45 to 60 minutes | checkpoint after Lessons 08 through 11 |

## Advanced And Senior Pack Estimates

| Surface | Typical Focused Time | Notes |
| --- | --- | --- |
| [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) | 75 to 120 minutes | validation pack |
| [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md) | 75 to 120 minutes | validation pack |
| [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) | 90 to 150 minutes | guided lab |
| [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) | 90 to 150 minutes | guided lab |
| [Advanced 005](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md) | 120 to 180 minutes | guided lab bundle |
| [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) | 90 to 150 minutes | validation pack plus deadlock graph work |
| [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) | 90 to 150 minutes | guided lab plus supplemental review pack |
| [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) | 75 to 120 minutes | validation pack |
| [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | 90 to 150 minutes | investigation pack |
| [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) | 90 to 150 minutes | guided lab |
| [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) | 90 to 150 minutes | investigation pack |
| [Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md) through [Senior 012](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md) | 60 to 120 minutes each | optional specialization packs |

## Planning Advice By Pace

| Pace | What It Usually Looks Like |
| --- | --- |
| cautious ramp | one lesson or one validation pack per focused session |
| steady weekly pace | two to four lessons or packs per week once setup is boring |
| professional refresh pace | selective jumps into the SQL-first route, one assessment pack, then targeted advanced or senior labs |

## Best Companions

1. [Curriculum Map](curriculum-map.md)
2. [How To Start](how-to-start.md)
3. [SQL-First Day One](sql-first-day-one.md)
4. [SQL To .NET Data Access Path](sql-to-dotnet-data-access-path.md)
5. [Repository Improvement Suggestions](repository-improvement-suggestions.md)