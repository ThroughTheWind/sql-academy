# Senior 002: Optimistic Concurrency And Staged Trade Ingestion

## Objective

Connect SQL Server write-side behavior directly to application code with a runnable lab for optimistic concurrency and staged trade ingestion, then optionally use a short SQL review pack to rehearse the same boundaries from the database side.

## Exercise Type

This pack is a guided lab with a supplemental validation pack.

Use the lab guide below as the later Phase 8 follow-up after the read-path bundle and generated-SQL investigation. After you finish it, you can optionally use `answer.sql` and `validation.sql` as a short review pack for the ingestion design and concurrency summary.

## Scenario

The read paths already feel predictable. Now the application is about to ingest a larger trade feed while also serving write traffic, and you need to move from query-shape discussion into stale-write handling and staged publish flow.

## Lab Sequence

1. [Lab 03: Rowversion And Staged Ingestion](lab-03-rowversion-and-staged-ingestion.md)

## Repository Anchors

- [Lesson 09: EF Core, Dapper, And Query Shape](../../../../docs/learning/09-ef-core-dapper-and-query-shape.md)
- [Advanced 005: EF Core, Dapper, And Query-Contract Read Paths](../../Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md)
- [TradeImportService](../../../libs/SqlAcademy.Persistence/Commands/Trades/TradeImportService.cs)
- [OrdersController](../../../apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs)
- [OrderWriteService](../../../libs/SqlAcademy.Persistence/Commands/Orders/OrderWriteService.cs)
- [TradesController](../../../apps/SqlAcademy.Api/Controllers/V1/TradesController.cs)
- [TradeImportBatchReadService](../../../libs/SqlAcademy.Persistence/Queries/Trades/TradeImportBatchReadService.cs)
- [TradesEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs)
- [OrdersEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/OrdersEndpointTests.cs)
- [SqlAcademy.Api.http](../../../apps/SqlAcademy.Api/SqlAcademy.Api.http)

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

- Lab 03 proves a `rowversion` conflict through the HTTP surface and exercises a real trade-import path with explicit dry-run preview and publish boundaries.
- the optional SQL review pack stays available after the code lab as reinforcement rather than the main exercise contract.

## Completion Checklist

- [ ] you finished [Lab 03](lab-03-rowversion-and-staged-ingestion.md) and can explain the stale-write conflict through the orders HTTP surface
- [ ] you exercised the trade-import flow with explicit dry-run preview and publish boundaries instead of treating ingestion as one opaque step
- [ ] you can point to the application boundary that detects the `rowversion` mismatch and the boundary that stages trade import work
- [ ] if you used the optional SQL review pack, the validation command passes and your summary still matches the application behavior
- [ ] you can explain why optimistic concurrency and staged ingestion solve different write-side risks even though they appear in the same pack

## Focused Companion Checks

Use the narrowest executable anchor that matches the slice you are working on:

- `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~OrdersEndpointTests"`
- `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~TradesEndpointTests"`
- `powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion`

Use the orders endpoint tests when you are checking the `rowversion` conflict path, the trades endpoint tests when you are checking staged import behavior, and the optional SQL validation only after the code lab when you want the database-side recap.
