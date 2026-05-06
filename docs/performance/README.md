# Performance Track

This section organizes performance engineering material across SQL Server and .NET.

Use [Learning Docs](../learning/README.md) for the ordered path and this page for the performance-specific slices.

Core concerns:
- baseline measurement before tuning
- logical reads, CPU, duration, and wait signals
- indexing strategies grounded in workload shape
- execution-plan analysis and parameter sensitivity
- EF Core tracking cost and query-shape comparison
- benchmark and regression tooling

## Best Entry Points

- [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](../learning/10-observability-testing-and-performance-engineering.md)
- [Phase 5: Indexing And Performance](../phases/phase-5-indexing-and-performance.md)

## Linked Exercises

- [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
- [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)

The performance test project and benchmark project provide executable anchors for this track.