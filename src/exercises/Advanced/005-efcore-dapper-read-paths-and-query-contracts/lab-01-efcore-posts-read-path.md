# Lab 01: EF Core Posts Read Path

## Objective

Trace the EF Core posts query from controller to SQL-facing intent, then make one small contract change without losing projection, deterministic ordering, or read-only behavior.

## Start Here

- [PostsController](../../../apps/SqlAcademy.Api/Controllers/V1/PostsController.cs)
- [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs)
- [PostsEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs)
- [TrackingBehaviorTests](../../../../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs)

## Baseline Run

1. Run `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"`.
2. Run `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~TrackingBehaviorTests"`.
3. Write down the current result contract: filters, sort options, projection columns, and pagination behavior.

## Tasks

1. Read [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs) and explain the base rowset, filter application order, sort behavior, and final projection.
2. Keep `AsNoTracking()` and direct projection in place while making one deliberate extension to the endpoint contract.
3. If you want a concrete default task, add a new `sortBy=authorUserName` option with a deterministic tie-breaker.
4. Extend [PostsEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs) with one focused test for the new or refined contract.
5. Rerun the same focused tests until they pass.

## Verification

- the endpoint still uses projection instead of materializing full entities and reshaping later
- deterministic ordering is preserved with an explicit tie-breaker
- the focused posts test and tracking test both pass after the change
- you can explain what SQL shape EF Core should generate before you inspect it