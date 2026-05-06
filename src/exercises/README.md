# Exercise Index

The exercise packs are intentionally progressive.

- `Beginner` focuses on correctness and relational intuition.
- `Intermediate` adds analytical SQL and result-shaping patterns.
- `Advanced` introduces tuning, parameter sensitivity, and migration safety.
- `Senior` focuses on concurrency, operational debugging, EF Core tradeoffs, and ingestion strategy.

Each exercise folder contains the minimum assets needed to work the problem locally without hunting for supporting material.

## Current Ladder

1. [Beginner 001](Beginner/001-joins-and-aggregations/README.md)
2. [Beginner 002](Beginner/002-filtering-constraints-and-data-quality/README.md)
3. [Intermediate 001](Intermediate/001-window-functions-and-pagination/README.md)
4. [Intermediate 002](Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
5. [Advanced 001](Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
6. [Advanced 002](Advanced/002-staged-backfill-and-contract-enforcement/README.md)
7. [Senior 001](Senior/001-concurrency-blocking-and-deadlocks/README.md)
8. [Senior 002](Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
9. [Senior 003](Senior/003-transactional-outbox-and-delivery-consistency/README.md)
10. [Senior 004](Senior/004-posts-api-latency-and-observability-triage/README.md)

## Validation-Ready Packs

The following packs include `answer.sql` and `validation.sql`:

- [Beginner 002](Beginner/002-filtering-constraints-and-data-quality/README.md)
- [Beginner 001](Beginner/001-joins-and-aggregations/README.md)
- [Intermediate 002](Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
- [Intermediate 001](Intermediate/001-window-functions-and-pagination/README.md)
- [Advanced 002](Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Advanced 001](Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Senior 003](Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 001](Senior/001-concurrency-blocking-and-deadlocks/README.md)

Run the validation harness with:

`powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Beginner/002-filtering-constraints-and-data-quality`