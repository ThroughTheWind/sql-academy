# SQL To .NET Data Access Path

Use this guide when you already know SQL well enough to reason about query shape and you want the shortest honest route into EF Core, Dapper, and ASP.NET Core data access in this repository.

This is an optional on-ramp. It does not replace [Curriculum Map](curriculum-map.md).

## Use This Path When

- you can already explain base rowset, filters, projection, sort order, and pagination in SQL terms
- you want to learn how those same concerns show up in EF Core, Dapper, endpoints, and tests
- you do not need the full SQL ladder before you start working through application integration

## Do Not Use This Path When

- local setup is still noisy; start with [SQL-First Day One](sql-first-day-one.md) or [How To Start](how-to-start.md)
- deterministic ordering and paging from [Lesson 03](03-window-functions-and-intermediate-querying.md) are still shaky
- migration safety from [Lesson 04](04-schema-design-and-migration-safety.md) or optimistic concurrency from [Lesson 05](05-transactions-blocking-and-deadlocks.md) still feel unfamiliar

## Minimum Entry Bar

These are the minimum earlier surfaces this route assumes:

1. [How To Start](how-to-start.md)
2. [Phase 0: Local Setup](../phases/phase-0-local-setup.md)
3. [Lesson 03: Window Functions And Intermediate Querying](03-window-functions-and-intermediate-querying.md)
4. [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
5. [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md)

If those topics already feel predictable, continue with the route below.

## Exact Route

1. Optional: [SQL-First Day One](sql-first-day-one.md) if you still need one low-friction setup or query session
2. [How To Start](how-to-start.md)
3. [Phase 0: Local Setup](../phases/phase-0-local-setup.md)
4. [Lesson 03: Window Functions And Intermediate Querying](03-window-functions-and-intermediate-querying.md)
5. [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
6. [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md)
7. [From SQL To EF Core And Dapper](from-sql-to-efcore-and-dapper.md)
8. [Phase 8: .NET And EF Core Integration](../phases/phase-8-dotnet-and-efcore-integration.md)
9. [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md)
10. [Advanced 005: EF Core, Dapper, And Query-Contract Read Paths](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md)
11. [Senior 005: EF Core N+1 And Generated SQL Investigation](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md)
12. [Senior 002: Optimistic Concurrency And Staged Trade Ingestion](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
13. Rejoin the main route at [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md)
14. Continue to [Lesson 11: Capstones And Interview Readiness](11-capstones-and-interview-readiness.md)

## Why These Stops Matter

- [Lesson 03](03-window-functions-and-intermediate-querying.md) is the minimum bar for stable pagination and deterministic ordering before you reason about LINQ or Dapper paging.
- [Lesson 04](04-schema-design-and-migration-safety.md) is the minimum bar for migration generation and rollout safety before EF Core design-time or schema-change work.
- [Lesson 05](05-transactions-blocking-and-deadlocks.md) is the minimum bar for `rowversion`, stale-write conflicts, and retry decisions.
- [From SQL To EF Core And Dapper](from-sql-to-efcore-and-dapper.md) keeps the conversation about query shape rather than library preference.
- [Lesson 09](09-ef-core-dapper-and-query-shape.md) explains why this repository mixes EF Core and Dapper instead of forcing a single abstraction.

## Current Repo Anchors

- [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs) for projection, `AsNoTracking()`, filter composition, and deterministic sorting
- [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs) for explicit SQL, count query shape, and paging
- [OrderConfiguration](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs) and [OrdersController](../../src/apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs) for optimistic concurrency backed by `rowversion`
- [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs), [TradesEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs), and [TrackingBehaviorTests](../../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs) for focused validation of the read paths
- [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) when you want a later EF Core versus Dapper comparison surface
- [DesignTimeLearningDbContextFactory](../../src/libs/SqlAcademy.Migrations/DesignTimeLearningDbContextFactory.cs) when the question becomes design-time configuration or migration creation behavior

## Rejoin The Main Route

Return to the default route after [Lesson 09](09-ef-core-dapper-and-query-shape.md), [Advanced 005: EF Core, Dapper, And Query-Contract Read Paths](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md), [Senior 005: EF Core N+1 And Generated SQL Investigation](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md), and [Senior 002: Optimistic Concurrency And Staged Trade Ingestion](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) all feel predictable.

At that point, continue in this order:

1. [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md)
2. [Lesson 11: Capstones And Interview Readiness](11-capstones-and-interview-readiness.md)
3. [Cumulative Review](cumulative-review.md)