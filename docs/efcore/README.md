# EF Core Track

This page is the application-side data-access reference surface for the academy.

If you want the learner path first, begin in [Curriculum Map](../learning/curriculum-map.md), read [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md), and then return here as a topic reference.

Use this page when you already know the application concern you want to review and need the fastest route to the right lesson, code anchor, test surface, or exercise.

Do not use this page as the main curriculum index. It is a reference page for EF Core and hybrid EF-plus-Dapper concerns after the SQL foundation is already in place.

## Scope Boundary

The current EF Core route targets production-oriented application data access: query shape, projection, tracking behavior, concurrency, migrations, hybrid EF-plus-Dapper boundaries, staged import review, and validation through tests and telemetry.

It does not yet claim full EF Core mastery across every advanced mapping, interception, batching, or materialization feature.

Near-term expansion areas such as deeper generated-SQL inspection workflows, split-query tradeoffs, and broader batching patterns are tracked in [../../.ai/sql-efcore-mastery-backlog.md](../../.ai/sql-efcore-mastery-backlog.md).

Primary topics:
- DbContext design and aggregate mapping
- migrations and deployment hygiene
- tracking versus `AsNoTracking()`
- pagination, filtering, and sorting in APIs
- optimistic concurrency with `rowversion`
- transaction orchestration across EF Core and Dapper boundaries
- staged trade-import review and batch-history inspection
- N+1 detection, generated-SQL inspection, and query-shape control

## When To Use This Page

- when you need a code or test anchor for a focused EF Core concern
- when you want to connect an EF Core behavior back to the SQL lesson that explains it
- when you need the nearest exercise or validation surface for query shape, concurrency, migrations, or hybrid read paths

## When Not To Use This Page

- when you still need the ordered teaching route; use [Curriculum Map](../learning/curriculum-map.md)
- when the main gap is still SQL reasoning rather than application abstraction; use [SQL Track](../sql/README.md) and [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md)
- when the main question is release sequencing or incident response posture; use [Operations Track](../operations/README.md)

## EF Core Concern Matrix

| Concern | SQL Prerequisite | Main Teaching Surface | Concrete Repo Anchors | Practice And Validation | Common Failure Mode |
| --- | --- | --- | --- | --- | --- |
| read-model projection and deterministic sorting | [Lessons 01-03](../learning/01-sql-fundamentals.md) | [bridge guide](../learning/from-sql-to-efcore-and-dapper.md), [Lesson 09](../learning/09-ef-core-dapper-and-query-shape.md) | [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs) | [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md), [Lab 01](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-01-efcore-posts-read-path.md) | materializing too much data or relying on unstable sort order |
| tracking versus `AsNoTracking()` | [Lessons 01-03](../learning/01-sql-fundamentals.md) | [Lesson 09](../learning/09-ef-core-dapper-and-query-shape.md), [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md) | [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), [TrackingBehaviorTests](../../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs), [TrackingModeBenchmarks](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs) | [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md), [Lab 01](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-01-efcore-posts-read-path.md) | tracking read models that are never updated |
| N+1 prevention and generated-SQL inspection | [Lessons 02, 03, 06, and 09](../learning/02-joins-and-aggregations.md) | [Lesson 09](../learning/09-ef-core-dapper-and-query-shape.md), [Phase 8](../phases/phase-8-dotnet-and-efcore-integration.md) | [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs), [TrackingBehaviorTests](../../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs) | [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md), [Lab 01](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-01-efcore-posts-read-path.md) | assuming readable LINQ automatically means one efficient SQL statement |
| optimistic concurrency with `rowversion` | [Lesson 05](../learning/05-transactions-blocking-and-deadlocks.md) | [Lesson 09](../learning/09-ef-core-dapper-and-query-shape.md), [Phase 8](../phases/phase-8-dotnet-and-efcore-integration.md) | [OrderConfiguration](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs), [schema bootstrap](../../db/schemas/001_create_learning_db.sql), [OrdersController](../../src/apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs), [OrdersEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/OrdersEndpointTests.cs) | [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md), [Lab 03](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-03-rowversion-and-staged-ingestion.md) | silently overwriting a conflicting update |
| migrations and deployment hygiene | [Lesson 04](../learning/04-schema-design-and-migration-safety.md) | [bridge guide](../learning/from-sql-to-efcore-and-dapper.md), [Phase 8](../phases/phase-8-dotnet-and-efcore-integration.md) | [SqlAcademy.Migrations](../../src/libs/SqlAcademy.Migrations/SqlAcademy.Migrations.csproj), [DesignTime factory](../../src/libs/SqlAcademy.Migrations/DesignTimeLearningDbContextFactory.cs), [InitialCreate migration](../../src/libs/SqlAcademy.Migrations/Migrations/20260506034638_InitialCreate.cs) | [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md), [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) | treating generated migrations as automatically rollout-safe |
| EF Core plus Dapper boundaries and staged trade-import review | [Lessons 03 and 06](../learning/03-window-functions-and-intermediate-querying.md) | [bridge guide](../learning/from-sql-to-efcore-and-dapper.md), [Lesson 09](../learning/09-ef-core-dapper-and-query-shape.md), [Phase 8](../phases/phase-8-dotnet-and-efcore-integration.md) | [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), [TradeImportBatchReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeImportBatchReadService.cs), [TradesEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs) | [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md), [Lab 02](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-02-dapper-trades-read-path.md), [Lab 03](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-03-rowversion-and-staged-ingestion.md) | arguing about libraries instead of query shape, workload fit, and operational clarity |
| API validation, telemetry, and regression checks | [Lessons 06, 08, and 10](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md) | [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md), [Performance Track](../performance/README.md) | [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs), [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs), [SqlAcademy.Api Program](../../src/apps/SqlAcademy.Api/Program.cs) | [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | waiting until late release stages to inspect SQL, telemetry, and contract behavior together |

## Best Entry Points

- [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md)
- [Lesson 09: EF Core, Dapper, And Query Shape](../learning/09-ef-core-dapper-and-query-shape.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](../learning/10-observability-testing-and-performance-engineering.md)
- [Phase 8: .NET And EF Core Integration](../phases/phase-8-dotnet-and-efcore-integration.md)

## Fast Jumps By Need

- need the read-only posts path with projection and deterministic sorting: [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs), and [Lab 01](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-01-efcore-posts-read-path.md)
- need the explicit SQL read path for contrast: [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), [TradesEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs), and [Lab 02](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-02-dapper-trades-read-path.md)
- need the concurrency and stale-rowversion path: [OrdersController](../../src/apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs), [OrderConfiguration](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs), and [Lab 03](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-03-rowversion-and-staged-ingestion.md)
- need to prove the current posts path is not N+1 and see what would break it: [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md)
- need migration and deployment context for an EF model change: [SqlAcademy.Migrations](../../src/libs/SqlAcademy.Migrations/SqlAcademy.Migrations.csproj), [DesignTime factory](../../src/libs/SqlAcademy.Migrations/DesignTimeLearningDbContextFactory.cs), and [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- need performance evidence instead of an abstraction argument: [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) and [TrackingModeBenchmarks](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs)
- need to inspect staged trade-import history after publish: [TradeImportBatchReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeImportBatchReadService.cs) and [Lab 03](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-03-rowversion-and-staged-ingestion.md)

## Recommended Working Sequence

Use this page with a query-shape-first workflow:

1. restate the API or background-work contract in plain SQL terms
2. locate the EF Core or Dapper query path that actually owns that behavior
3. inspect projection, filters, sorting, pagination, and concurrency assumptions before changing code
4. choose the narrowest validation surface: focused integration test, performance test, benchmark, or exercise lab
5. only then make the code change and rerun the same surface

## Executable Validation Surfaces

Use these commands when you want a concrete validation anchor while working through the track:

```bash
dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"
dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~TradesEndpointTests"
dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~OrdersEndpointTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~TrackingBehaviorTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryPerformanceComparisonTests"
dotnet run --project tests/SqlAcademy.Benchmarks/SqlAcademy.Benchmarks.csproj -c Release
```

The benchmark project uses `SQLACADEMY_BENCHMARK_CONNECTIONSTRING` when you want to point it somewhere other than the default local SQL Server container.

## Bridge Back To SQL

This page does not replace the SQL mental model.

- query shape and projection still come from [Lesson 01](../learning/01-sql-fundamentals.md), [Lesson 02](../learning/02-joins-and-aggregations.md), and [Lesson 03](../learning/03-window-functions-and-intermediate-querying.md)
- concurrency behavior still starts in [Lesson 05](../learning/05-transactions-blocking-and-deadlocks.md)
- plan reasoning and regression thinking still come from [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md)

Use [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md) when you need the explicit handoff from SQL concepts to application code.

## Linked Exercises

- [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
- [Lab 01](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-01-efcore-posts-read-path.md)
- [Lab 02](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-02-dapper-trades-read-path.md)
- [Lab 03](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-03-rowversion-and-staged-ingestion.md)
- [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md)
- [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)
- [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)

The EF Core material begins in Phase 8 after the learner already understands the SQL behavior under the abstraction, and the bridge guide above is the handoff between those two parts of the course.