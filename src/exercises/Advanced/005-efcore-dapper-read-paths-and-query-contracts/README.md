# Advanced 005: EF Core, Dapper, And Query-Contract Read Paths

## Objective

Connect SQL query-shape reasoning directly to application read paths with runnable labs for EF Core projection and Dapper explicit SQL, then prove the contracts with focused tests before you move into generated-SQL investigations or write-side concurrency.

## Exercise Type

This pack is a guided lab bundle.

Use the two lab guides below as the first applied Phase 8 route. After you finish them, continue to [Senior 005: EF Core N+1 And Generated SQL Investigation](../../Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) for proof-oriented follow-up, then use [Senior 002: Optimistic Concurrency And Staged Trade Ingestion](../../Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) when you are ready for rowversion conflicts and staged publish flow.

## Scenario

Phase 8 starts with read traffic, not write-side conflict handling. You need to trace the current posts and trades contracts, explain them in SQL terms, and make one small contract change without losing projection, deterministic ordering, or explicit SQL control.

## Lab Sequence

1. [Lab 01: EF Core Posts Read Path](lab-01-efcore-posts-read-path.md)
2. [Lab 02: Dapper Trades Read Path](lab-02-dapper-trades-read-path.md)

## Repository Anchors

- [PostsController](../../../apps/SqlAcademy.Api/Controllers/V1/PostsController.cs)
- [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs)
- [TradesController](../../../apps/SqlAcademy.Api/Controllers/V1/TradesController.cs)
- [TradeReadService](../../../libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)
- [PostsEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs)
- [TradesEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs)
- [TrackingBehaviorTests](../../../../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs)
- [QueryPerformanceComparisonTests](../../../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs)
- [SqlAcademy.Api.http](../../../apps/SqlAcademy.Api/SqlAcademy.Api.http)

## Pack Assets

- [expected-outcomes.md](expected-outcomes.md) defines the completion contract for the read-path bundle.
- [hints.md](hints.md) gives progressive guidance without forcing one answer too early.
- [optional-solution.md](optional-solution.md) outlines one defensible implementation path after you already have an opinion.

## Next Surfaces

- [Senior 005: EF Core N+1 And Generated SQL Investigation](../../Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md)
- [Senior 002: Optimistic Concurrency And Staged Trade Ingestion](../../Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)

## Validation

- Lab 01 leaves the posts read path projection-first, deterministic, and verified by focused tests.
- Lab 02 leaves the trades read path explicit, deterministic, and verified by focused tests or API requests.
- you can restate both read paths in SQL terms before you argue about library preference.

## Completion Checklist

- [ ] you finished both [Lab 01](lab-01-efcore-posts-read-path.md) and [Lab 02](lab-02-dapper-trades-read-path.md)
- [ ] the posts read path still keeps `AsNoTracking()`, direct projection, and deterministic ordering after your contract change
- [ ] the trades read path still keeps explicit SQL control and deterministic paging after your contract change
- [ ] the focused posts and trades checks pass for the changed contracts
- [ ] you can explain, in SQL terms, why the posts path is a strong EF Core candidate and the trades path is a strong Dapper candidate

## Focused Companion Checks

Use the narrowest executable anchor that matches the lab you just changed:

- `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"`
- `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~TrackingBehaviorTests"`
- `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~TradesEndpointTests"`

Use the posts endpoint and tracking checks for Lab 01, and the trades endpoint check for Lab 02.