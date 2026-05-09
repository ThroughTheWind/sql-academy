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
