# EF Core Track

This section teaches .NET 10 data-access patterns against the same SQL Server model used in the SQL phases.

Primary topics:
- DbContext design and aggregate mapping
- migrations and deployment hygiene
- tracking vs `AsNoTracking`
- pagination, filtering, and sorting in APIs
- optimistic concurrency with `rowversion`
- transaction orchestration across EF Core and Dapper
- N+1 detection and query-shape control

The EF Core material begins in Phase 8 after the learner already understands the SQL behavior under the abstraction.