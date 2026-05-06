# Follow-Up Exercises By Level

These are suggested next exercises to extend the repository without bloating the first scaffold.

This file is not part of the default guided route.

Use [Curriculum Map](curriculum-map.md) for the main course sequence, and use this file only after the implemented route is already clear.

## Beginner Follow-Up Exercises

- Implemented next pack: [Beginner 002](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md)
- Expand the beginner filtering track with date ranges, null semantics, and `IN` versus `EXISTS` comparisons.
- Add a data-cleanup exercise where learners must identify duplicate business rows safely before deletion.
- Add a simple insert-and-constraint lab focused on unique usernames, emails, and foreign-key violations.
- Add a reporting exercise that distinguishes between detail rows and grouped summaries.

## Intermediate Follow-Up Exercises

- Implemented next pack: [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
- Add a gap-and-island exercise using trade timestamps.
- Add a recursive CTE exercise around hierarchical categories or comment threads.
- Add a retention-style cohort exercise that tracks users across later activity windows.
- Add a live pagination-drift exercise showing page movement before and after concurrent inserts.

## Advanced Follow-Up Exercises

- Implemented next pack: [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- Implemented guided lab: [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- Add an index-consolidation exercise where several overlapping indexes must be simplified.
- Add a parameter-sniffing lab with skewed data distribution and plan comparison.
- Add a statistics-staleness exercise that shows why a once-good plan degrades.
- Add a zero-downtime column rename exercise with compatibility windows and smoke-test gates.

## Senior Follow-Up Exercises

- Implemented next pack: [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- Implemented incident pack: [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)
- Implemented guided lab: [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md)
- Add a full deadlock-graph interpretation lab with retry policy design.
- Add an inbox or deduplication exercise for consumer-side delivery guarantees.
- Add a bulk-ingestion pipeline lab with staging tables, deduplication, and publish steps.
- Add a release-readiness exercise that combines migration safety, telemetry checks, and rollback strategy.