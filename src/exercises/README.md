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
| [Senior 001](Senior/001-concurrency-blocking-and-deadlocks/README.md) | validation pack |
| [Senior 002](Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) | guided lab |
| [Senior 003](Senior/003-transactional-outbox-and-delivery-consistency/README.md) | validation pack |
| [Senior 004](Senior/004-posts-api-latency-and-observability-triage/README.md) | investigation pack |
| [Senior 005](Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) | guided lab |
| [Senior 006](Senior/006-release-readiness-and-rollback-gates/README.md) | investigation pack |

## Validation-Ready Packs

The following packs include `answer.sql` and `validation.sql`:

- [Beginner 000](Beginner/000-sql-fundamentals-and-safe-changes/README.md)
- [Beginner 002](Beginner/002-filtering-constraints-and-data-quality/README.md)
- [Beginner 001](Beginner/001-joins-and-aggregations/README.md)
- [Intermediate 002](Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
- [Intermediate 001](Intermediate/001-window-functions-and-pagination/README.md)
- [Advanced 002](Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Advanced 001](Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Senior 002](Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) as a supplemental review pack inside the guided lab bundle
- [Senior 003](Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 001](Senior/001-concurrency-blocking-and-deadlocks/README.md)

The current non-validation packs are:

- [Advanced 003](Advanced/003-plan-cache-memory-grants-and-waits/README.md) as a guided lab
- [Advanced 004](Advanced/004-query-store-and-regression-triage/README.md) as a guided lab
- [Senior 001](Senior/001-concurrency-blocking-and-deadlocks/README.md) as a validation pack with a supplemental deadlock-graph walkthrough
- [Senior 002](Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) as a guided lab bundle with a supplemental review pack
- [Senior 004](Senior/004-posts-api-latency-and-observability-triage/README.md) as an investigation pack
- [Senior 005](Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) as a guided lab
- [Senior 006](Senior/006-release-readiness-and-rollback-gates/README.md) as an investigation pack

Run the validation harness with:

`powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Beginner/002-filtering-constraints-and-data-quality`