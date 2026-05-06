# Lab 03: Rowversion And Staged Ingestion

## Objective

Use real HTTP write paths to reproduce optimistic concurrency and run a staged trade-ingestion flow with explicit validation, deduplication, and publish boundaries.

## Start Here

- [OrdersController](../../../apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs)
- [OrderWriteService](../../../libs/SqlAcademy.Persistence/Commands/Orders/OrderWriteService.cs)
- [OrderConfiguration](../../../libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs)
- [TradesController](../../../apps/SqlAcademy.Api/Controllers/V1/TradesController.cs)
- [TradeImportService](../../../libs/SqlAcademy.Persistence/Commands/Trades/TradeImportService.cs)
- [OrdersEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/OrdersEndpointTests.cs)
- [TradesEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs)
- [SqlAcademy.Api.http](../../../apps/SqlAcademy.Api/SqlAcademy.Api.http)
- [answer.sql](answer.sql)
- [validation.sql](validation.sql)

## Baseline Run

1. Run `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~OrdersEndpointTests"`.
2. Run `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~TradesEndpointTests"`.
3. Read [OrderWriteService](../../../libs/SqlAcademy.Persistence/Commands/Orders/OrderWriteService.cs) and trace where the incoming rowversion becomes the EF Core original value.
4. Read [TradeImportService](../../../libs/SqlAcademy.Persistence/Commands/Trades/TradeImportService.cs) and identify the landing, validation, deduplication, and publish stages.

## Tasks

1. Explain how [OrderConfiguration](../../../libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs) and [OrderWriteService](../../../libs/SqlAcademy.Persistence/Commands/Orders/OrderWriteService.cs) work together to make optimistic concurrency real.
2. Explain how [TradeImportService](../../../libs/SqlAcademy.Persistence/Commands/Trades/TradeImportService.cs) keeps landing, validation, batch deduplication, existing-trade deduplication, and publish as distinct steps.
3. Make one small improvement to either the order-status path or the trade-import contract.
4. If you want a concrete default task, add one focused test for an invalid status, a missing rowversion, a `404` order update, or an invalid import row.
5. Rerun the focused order and trade tests until they pass.
6. Optionally finish the SQL-side reinforcement pack in [answer.sql](answer.sql) and validate it with `powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion`.

## Verification

- you can point to the exact line where the client rowversion becomes the concurrency token EF Core checks
- the order endpoint tests pass after your refinement
- the trade import endpoint reports clear counts for submitted, validated, duplicate, imported, and rejected rows
- you can explain where batch duplicates and existing-trade duplicates are separated in the code
- the SQL reinforcement pack is optional, not the primary ingestion contract

## Exit Criteria

You are done when you can reproduce a stale write, explain the resulting `409 Conflict`, and trace one imported trade batch from landing through publish without guessing at the stages.