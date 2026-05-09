# Performance Track

This page is the performance-engineering reference surface for the academy.

If you want the ordered learner route first, start in [Curriculum Map](../learning/curriculum-map.md). Use this page when you already know the performance concern you want to study and need the fastest path to the right lesson, exercise, code anchor, or executable validation surface.

Do not use this page as the main curriculum index. It is a reference page for measurement, tuning, and regression work after the basic learning route is already clear.

[Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) is the main optional performance deepening lab for persisted regression evidence after the default route is already clear. It is intentionally routed through this page instead of the default curriculum sequence.

## Scope Boundary

The current performance route targets evidence-backed tuning across SQL Server and .NET query paths. The emphasis is on defining a workload, measuring it consistently, reasoning about plans and waits, and validating that a change preserved correctness instead of just moving numbers around.

It does not treat performance as a library popularity contest or as isolated micro-optimization. It also does not yet claim full coverage of capacity planning, storage-engine deep dives, or infrastructure benchmarking beyond the core academy scenarios.

The tracked backlog for deeper future expansion lives in [../../.ai/sql-efcore-mastery-backlog.md](../../.ai/sql-efcore-mastery-backlog.md).

Coverage targets:
- baseline measurement before tuning
- logical reads, CPU, duration, and wait signals
- indexing strategies grounded in workload shape
- execution-plan analysis and parameter sensitivity
- plan cache versus Query Store reasoning
- EF Core tracking cost, projection shape, and explicit SQL comparison
- API-level telemetry and regression triage
- focused performance tests and benchmarks used with discipline

## When To Use This Page

- when you need the nearest lesson, exercise, test, or benchmark for a performance concern you already recognize
- when you want to compare EF Core, Dapper, or alternative SQL shapes with evidence rather than preference
- when you need a persisted regression surface such as Query Store instead of a one-moment cache snapshot
- when you need the concrete repo files that let you rerun the same scenario after a tuning change

## When Not To Use This Page

- when you still need the default teaching order; use [Curriculum Map](../learning/curriculum-map.md)
- when the main question is about application-side data-access boundaries rather than measurement and tuning; use [EF Core Track](../efcore/README.md)
- when the main question is release sequencing, smoke tests, or rollback posture; use [Operations Track](../operations/README.md)

## Performance Concern Matrix

| Concern | SQL Prerequisite | Main Teaching Surface | Concrete Repo Anchors | Practice And Validation | Common Failure Mode |
| --- | --- | --- | --- | --- | --- |
| baseline definition and fair comparison | [Lessons 01-03](../learning/01-sql-fundamentals.md) and [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md) | [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md), [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md), [Phase 5](../phases/phase-5-indexing-and-performance.md) | [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs), [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs) | [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md), [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | tuning before a workload and contract are stable enough to compare |
| indexing, statistics, and plan interpretation | [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md) | [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md), [Phase 5](../phases/phase-5-indexing-and-performance.md) | [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), [db/performance notes](../../db/performance/README.md), [workload variant](../../db/performance/001_high_cardinality_workload_variant.sql), [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) | [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) | adding an index without explaining read gain, write cost, and maintenance tradeoffs |
| parameter sensitivity and plan instability | [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md) | [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md), [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) | [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), [QueryStoreLabSmokeTests](../../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs) | [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md), [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) | assuming one cached plan explains every regression |
| plan cache, memory grants, and waits | [Lesson 07](../learning/07-sql-server-internals.md) | [Lesson 07](../learning/07-sql-server-internals.md), [Performance Labs](../../db/performance/README.md) | [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), [db/performance notes](../../db/performance/README.md), [workload variant](../../db/performance/001_high_cardinality_workload_variant.sql), [PlanCacheLabSmokeTests](../../tests/SqlAcademy.PerformanceTests/PlanCacheLabSmokeTests.cs) | [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) | treating every slowdown as an index problem instead of an engine-behavior problem |
| Query Store and persisted regression evidence | [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md) and [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md) | [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md), [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) | [QueryStoreLabSmokeTests](../../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs), [db/performance notes](../../db/performance/README.md), [workload variant](../../db/performance/001_high_cardinality_workload_variant.sql) | [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) | browsing generic top offenders instead of isolating the tagged workload you care about |
| EF Core tracking, projection, and generated-SQL cost | [Lessons 03, 06, and 09](../learning/03-window-functions-and-intermediate-querying.md) | [Lesson 09](../learning/09-ef-core-dapper-and-query-shape.md), [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md), [Phase 8](../phases/phase-8-dotnet-and-efcore-integration.md) | [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), [TrackingBehaviorTests](../../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs), [TrackingModeBenchmarks](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs) | [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md), [Advanced 005](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md) | measuring abstraction overhead without first checking query shape and projection |
| API latency, telemetry, and regression triage | [Lessons 06, 08, and 10](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md) | [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md), [Operations Track](../operations/README.md) | [SqlAcademy.Api Program](../../src/apps/SqlAcademy.Api/Program.cs), [OpenTelemetry setup](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs), [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs), [PostsApiObservabilityDrillTests](../../tests/SqlAcademy.PerformanceTests/PostsApiObservabilityDrillTests.cs) | [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | blaming the entire platform before narrowing the incident to one endpoint and one query path |
| blocking and deadlocks as latency multipliers | [Lesson 05](../learning/05-transactions-blocking-and-deadlocks.md) | [Lesson 05](../learning/05-transactions-blocking-and-deadlocks.md), [Operations Track](../operations/README.md) | [DeadlockReproductionTests](../../tests/SqlAcademy.PerformanceTests/DeadlockReproductionTests.cs), [Senior 001 deadlock graph lab](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/deadlock-graph-lab.md), [OrdersController](../../src/apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs) | [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) | reading latency only from duration numbers while ignoring concurrency symptoms |

## Best Entry Points

- [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md)
- [Phase 5: Indexing And Performance](../phases/phase-5-indexing-and-performance.md)
- [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](../learning/10-observability-testing-and-performance-engineering.md)

## Fast Jumps By Need

- need a fair, repeatable comparison surface first: [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) and [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md)
- need indexing and parameter-sensitivity practice against a real workload shape: [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- need more local volume before estimates, memory grants, or Query Store comparisons become interesting: [workload variant](../../db/performance/001_high_cardinality_workload_variant.sql) and [Performance Labs](../../db/performance/README.md)
- need engine-behavior diagnosis rather than just plan screenshots: [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) and [PlanCacheLabSmokeTests](../../tests/SqlAcademy.PerformanceTests/PlanCacheLabSmokeTests.cs)
- need persisted query history instead of plan-cache luck: [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) and [QueryStoreLabSmokeTests](../../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs)
- need a printable persisted-regression checklist: [Query Store Regression Runbook](../operations/query-store-regression-runbook.md)
- need to explain tracking versus no-tracking with executable evidence: [TrackingBehaviorTests](../../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs) and [TrackingModeBenchmarks](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs)
- need to triage a slow API path with logs, metrics, traces, and SQL-shape reasoning together: [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) and [PostsApiObservabilityDrillTests](../../tests/SqlAcademy.PerformanceTests/PostsApiObservabilityDrillTests.cs)
- need to distinguish latency from locking and deadlock behavior: [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md), [Senior 001 deadlock graph lab](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/deadlock-graph-lab.md), and [DeadlockReproductionTests](../../tests/SqlAcademy.PerformanceTests/DeadlockReproductionTests.cs)

## Executable Repo Anchors

The repository intentionally provides code and tests that let you validate a claim instead of stopping at doc-level advice.

- [QueryPerformanceComparisonTests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) compares equivalent EF Core and Dapper work on the trade read path
- [workload variant](../../db/performance/001_high_cardinality_workload_variant.sql) expands posts, comments, and trades with repeatable synthetic rows without changing the default baseline seed path
- [TrackingBehaviorTests](../../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs) proves the identity and tracking behavior difference directly
- [PlanCacheLabSmokeTests](../../tests/SqlAcademy.PerformanceTests/PlanCacheLabSmokeTests.cs) verifies the Advanced 003 starter script leaves the tagged trade query in plan cache and still inspects the intended storage snapshot tables
- [QueryStoreLabSmokeTests](../../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs) verifies the Query Store lab captures tagged queries as intended
- [Query Store Regression Runbook](../operations/query-store-regression-runbook.md) gives the compact operational checklist for persisted-regression triage when the full lab is more detail than you need in the moment
- [DeadlockReproductionTests](../../tests/SqlAcademy.PerformanceTests/DeadlockReproductionTests.cs) smoke-tests that the Senior 001 pack ships the deadlock graph lab and retry-guidance assets
- [PostsApiObservabilityDrillTests](../../tests/SqlAcademy.PerformanceTests/PostsApiObservabilityDrillTests.cs) verifies the Senior 004 investigation pack keeps its logs, metrics, traces, workbook prompts, and expected outcomes aligned
- [TrackingModeBenchmarks](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs) isolates the cost of tracking versus no tracking on the posts table
- [PostReadService](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs) and [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs) are the main application query-shape anchors
- [SqlAcademy.Api Program](../../src/apps/SqlAcademy.Api/Program.cs) and [OpenTelemetry setup](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs) connect endpoint behavior to telemetry and performance observation

## Working Sequence In This Repo

Use this page with a measurement-first workflow:

1. define the claim: which endpoint, query, or workload shape is allegedly slow?
2. pick the narrowest evidence surface that can prove or disprove it: plan, test, benchmark, metric, or trace
3. capture the baseline before changing code, SQL, or indexes
4. make one deliberate change at a time
5. rerun the same validation surface instead of switching to a different one mid-stream
6. keep correctness, rollout safety, and telemetry intact after the performance change

## Fast Validation Commands

Use these when you want an executable anchor instead of a conceptual reminder:

```bash
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryPerformanceComparisonTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~TrackingBehaviorTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~PlanCacheLabSmokeTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryStoreLabSmokeTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~PostsApiObservabilityDrillAssetTests"
dotnet run --project tests/SqlAcademy.Benchmarks/SqlAcademy.Benchmarks.csproj -c Release
```

The benchmark project uses `SQLACADEMY_BENCHMARK_CONNECTIONSTRING` when you want to override the default local SQL Server connection.

## Linked Exercises

- [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)
- [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
- [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)
- [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md)

## Bridge Back To SQL, EF Core, And Operations

This page does not replace the underlying SQL or operational mental model.

- query shape still starts in [Lesson 01](../learning/01-sql-fundamentals.md), [Lesson 02](../learning/02-joins-and-aggregations.md), [Lesson 03](../learning/03-window-functions-and-intermediate-querying.md), and [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md)
- application-level tracking, projection, and hybrid EF-plus-Dapper decisions still belong in [EF Core Track](../efcore/README.md)
- release-safe verification, smoke tests, and incident sequencing still belong in [Operations Track](../operations/README.md)

Use [Exercise System](../exercises/README.md) for the full ladder and [Learning Docs](../learning/README.md) for the default course order.