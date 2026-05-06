# Senior 002: EF Core, Dapper, Concurrency, And Bulk Ingestion Labs

## Objective

Connect SQL Server behavior directly to application code with runnable labs for EF Core read paths, Dapper read paths, optimistic concurrency, and staged trade ingestion, then optionally use a short SQL review pack to rehearse the same boundaries from the database side.

## Exercise Type

This pack is a guided lab bundle with a supplemental validation pack.

Use the three lab guides below as the primary Phase 8 route. After you finish them, you can optionally use `answer.sql` and `validation.sql` as a short review pack for the ingestion design and concurrency summary.

## Scenario

The application is about to ingest a larger trade feed while also serving read traffic. Phase 8 is where you stop talking about EF Core and Dapper in the abstract and start using the repository's API, tests, and persistence code directly.

## Lab Sequence

1. [Lab 01: EF Core Posts Read Path](lab-01-efcore-posts-read-path.md)
2. [Lab 02: Dapper Trades Read Path](lab-02-dapper-trades-read-path.md)
3. [Lab 03: Rowversion And Staged Ingestion](lab-03-rowversion-and-staged-ingestion.md)

## Repository Anchors

- [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs)
- [TradeReadService](../../../libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)
- [TradeImportService](../../../libs/SqlAcademy.Persistence/Commands/Trades/TradeImportService.cs)
- [OrdersController](../../../apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs)
- [OrderWriteService](../../../libs/SqlAcademy.Persistence/Commands/Orders/OrderWriteService.cs)
- [TradesController](../../../apps/SqlAcademy.Api/Controllers/V1/TradesController.cs)
- [PostsEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs)
- [TradesEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs)
- [OrdersEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/OrdersEndpointTests.cs)

## Supplemental Review Pack

The original SQL assets remain here as a short review pack after the code labs:

- `starter.sql` gives baseline SQL surfaces and a staged import fixture to compare with the application code.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the expected review tables in one SQL session.
- `broken.sql` captures the risky or incomplete implementation to review.
- `expected-outcomes.md` defines the completion contract.
- `hints.md` gives progressive guidance.
- `optional-solution.sql` shows one valid SQL-side answer shape.

Run the review pack with:

`powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion`

## Validation

- Lab 01 leaves the posts read path projection-first, deterministic, and verified by focused tests.
- Lab 02 leaves the trades read path explicit, deterministic, and verified by focused tests or API requests.
- Lab 03 proves a `rowversion` conflict through the HTTP surface and exercises a real trade-import path with explicit staging boundaries.
