# EF Core Track

This section teaches .NET 10 data-access patterns against the same SQL Server model used in the SQL phases.

If you want the learner path first, begin in [Learning Docs](../learning/README.md), read [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md), and then return here as a topic reference.

Primary topics:
- DbContext design and aggregate mapping
- migrations and deployment hygiene
- tracking vs `AsNoTracking`
- pagination, filtering, and sorting in APIs
- optimistic concurrency with `rowversion`
- transaction orchestration across EF Core and Dapper
- N+1 detection and query-shape control

## Best Entry Points

- [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md)
- [Lesson 09: EF Core, Dapper, And Query Shape](../learning/09-ef-core-dapper-and-query-shape.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](../learning/10-observability-testing-and-performance-engineering.md)
- [Phase 8: .NET And EF Core Integration](../phases/phase-8-dotnet-and-efcore-integration.md)

## Linked Exercises

- [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
- [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)

The EF Core material begins in Phase 8 after the learner already understands the SQL behavior under the abstraction, and the bridge guide above is the handoff between those two parts of the course.