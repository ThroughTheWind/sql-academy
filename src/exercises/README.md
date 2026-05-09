# Exercise Index

The exercise packs are intentionally progressive.

- `Beginner` focuses on correctness and relational intuition.
- `Intermediate` adds analytical SQL and result-shaping patterns.
- `Advanced` introduces tuning, parameter sensitivity, and migration safety.
- `Senior` focuses on concurrency, operational debugging, EF Core tradeoffs, and ingestion strategy.

Each exercise folder contains the minimum assets needed to work the problem locally without hunting for supporting material.

Exercises currently ship in three supported modes:

- `validation pack`: includes `starter.sql`, `answer.sql`, and `validation.sql`
- `guided lab`: includes starter assets and explicit completion criteria, but manual validation
- `investigation pack`: includes evidence files or workbooks and expects a written analysis

## First-Time Learner Route

If you are new to this repository, do not start by scanning the whole ladder.

Use this order first:

1. [SQL-First Day One](../../docs/learning/sql-first-day-one.md)
2. [Lesson 01](../../docs/learning/01-sql-fundamentals.md)
3. [Beginner 000](Beginner/000-sql-fundamentals-and-safe-changes/README.md)
4. [Lesson 02](../../docs/learning/02-joins-and-aggregations.md)
5. [Beginner 001](Beginner/001-joins-and-aggregations/README.md)
6. [Beginner 002](Beginner/002-filtering-constraints-and-data-quality/README.md)

Do not treat the rest of the ladder as required day-one reading.

## If You Are Unsure Which Pack To Open

- use [Beginner 000](Beginner/000-sql-fundamentals-and-safe-changes/README.md) when deterministic ordering, safe updates, and constraint reasoning are still the main gap
- use [Beginner 001](Beginner/001-joins-and-aggregations/README.md) when left joins, row multiplication, or counting are still confusing
- use [Beginner 002](Beginner/002-filtering-constraints-and-data-quality/README.md) when filtering, normalization, and missing-value triage still feel weak
- use [Intermediate 001](Intermediate/001-window-functions-and-pagination/README.md) only after the beginner packs already feel predictable

## Current Ladder

| Pack | Mode |
| --- | --- |
| [Beginner 000](Beginner/000-sql-fundamentals-and-safe-changes/README.md) | validation pack |
| [Beginner 001](Beginner/001-joins-and-aggregations/README.md) | validation pack |
| [Beginner 002](Beginner/002-filtering-constraints-and-data-quality/README.md) | validation pack |
| [Intermediate 001](Intermediate/001-window-functions-and-pagination/README.md) | validation pack |
| [Intermediate 002](Intermediate/002-cohort-analysis-and-pagination-drift/README.md) | validation pack |
| [Advanced 001](Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) | validation pack |
| [Advanced 002](Advanced/002-staged-backfill-and-contract-enforcement/README.md) | validation pack |
| [Advanced 003](Advanced/003-plan-cache-memory-grants-and-waits/README.md) | guided lab |
| [Advanced 004](Advanced/004-query-store-and-regression-triage/README.md) | guided lab |
| [Advanced 005](Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md) | guided lab |
| [Senior 001](Senior/001-concurrency-blocking-and-deadlocks/README.md) | validation pack |
| [Senior 002](Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) | guided lab |
| [Senior 003](Senior/003-transactional-outbox-and-delivery-consistency/README.md) | validation pack |
| [Senior 004](Senior/004-posts-api-latency-and-observability-triage/README.md) | investigation pack |
| [Senior 005](Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) | guided lab |
| [Senior 006](Senior/006-release-readiness-and-rollback-gates/README.md) | investigation pack |
| [Senior 007](Senior/007-backup-restore-and-recovery-verification/README.md) | investigation pack |
| [Senior 008](Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md) | investigation pack |
| [Senior 009](Senior/009-ha-dr-failover-posture-and-verification/README.md) | investigation pack |
| [Senior 010](Senior/010-change-capture-provenance-and-reconciliation/README.md) | investigation pack |
| [Senior 011](Senior/011-least-privilege-and-operational-security-boundaries/README.md) | investigation pack |
| [Senior 012](Senior/012-row-level-security-and-tenant-isolation/README.md) | guided lab |

## Validation-Ready Packs

The following packs include `answer.sql` and `validation.sql`:

- [Beginner 000](Beginner/000-sql-fundamentals-and-safe-changes/README.md)
- [Beginner 002](Beginner/002-filtering-constraints-and-data-quality/README.md)
- [Beginner 001](Beginner/001-joins-and-aggregations/README.md)
- [Intermediate 002](Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
- [Intermediate 001](Intermediate/001-window-functions-and-pagination/README.md)
- [Advanced 002](Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Advanced 001](Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Senior 002](Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) as a supplemental review pack inside the later concurrency and ingestion follow-up
- [Senior 003](Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 001](Senior/001-concurrency-blocking-and-deadlocks/README.md)

The current non-validation packs are:

- [Advanced 003](Advanced/003-plan-cache-memory-grants-and-waits/README.md) as a guided lab
- [Advanced 004](Advanced/004-query-store-and-regression-triage/README.md) as a guided lab
- [Advanced 005](Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md) as a guided lab bundle for EF Core and Dapper read-path contracts
- [Senior 001](Senior/001-concurrency-blocking-and-deadlocks/README.md) as a validation pack with a supplemental deadlock-graph walkthrough
- [Senior 002](Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) as a guided lab for optimistic concurrency and staged trade ingestion with a supplemental review pack
- [Senior 004](Senior/004-posts-api-latency-and-observability-triage/README.md) as an investigation pack
- [Senior 005](Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) as a guided lab
- [Senior 006](Senior/006-release-readiness-and-rollback-gates/README.md) as an investigation pack
- [Senior 007](Senior/007-backup-restore-and-recovery-verification/README.md) as an investigation pack for optional DBA or DBRE recovery-verification work
- [Senior 008](Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md) as an investigation pack for optional DBA or DBRE scheduling and job-safety work
- [Senior 009](Senior/009-ha-dr-failover-posture-and-verification/README.md) as an investigation pack for optional DBA or DBRE HA or DR failover-posture work
- [Senior 010](Senior/010-change-capture-provenance-and-reconciliation/README.md) as an investigation pack for optional DBA or DBRE change-capture and reconciliation work
- [Senior 011](Senior/011-least-privilege-and-operational-security-boundaries/README.md) as an investigation pack for optional DBA or DBRE security and least-privilege work
- [Senior 012](Senior/012-row-level-security-and-tenant-isolation/README.md) as a guided lab for optional DBA or DBRE row-level security and tenant-isolation work

Run the validation harness with:

`powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Beginner/002-filtering-constraints-and-data-quality`