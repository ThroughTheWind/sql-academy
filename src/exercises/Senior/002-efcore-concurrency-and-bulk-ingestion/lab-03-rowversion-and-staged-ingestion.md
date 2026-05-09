# Lab 03: Rowversion And Staged Ingestion

## Objective

Use real HTTP write paths to reproduce optimistic concurrency and run a staged trade-ingestion flow with explicit validation, deduplication, dry-run preview, and publish boundaries.

Use this after [Advanced 005: EF Core, Dapper, And Query-Contract Read Paths](../../Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md) and [Senior 005: EF Core N+1 And Generated SQL Investigation](../005-efcore-n-plus-one-and-generated-sql-investigation/README.md) when the read-path material already feels predictable.

## Start Here

- [OrdersController](../../../apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs)
- [OrderWriteService](../../../libs/SqlAcademy.Persistence/Commands/Orders/OrderWriteService.cs)
- [OrderConfiguration](../../../libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs)
- [TradesController](../../../apps/SqlAcademy.Api/Controllers/V1/TradesController.cs)
- [TradeImportService](../../../libs/SqlAcademy.Persistence/Commands/Trades/TradeImportService.cs)
- [TradeImportBatchReadService](../../../libs/SqlAcademy.Persistence/Queries/Trades/TradeImportBatchReadService.cs)
- [OrdersEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/OrdersEndpointTests.cs)
- [TradesEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs)
- [SqlAcademy.Api.http](../../../apps/SqlAcademy.Api/SqlAcademy.Api.http)
- [answer.sql](answer.sql)
- [validation.sql](validation.sql)

## Baseline Run

1. Run `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~OrdersEndpointTests"`.
2. Run `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~TradesEndpointTests"`.
3. Read [OrderWriteService](../../../libs/SqlAcademy.Persistence/Commands/Orders/OrderWriteService.cs) and trace where the incoming rowversion becomes the EF Core original value.
4. Read [TradeImportService](../../../libs/SqlAcademy.Persistence/Commands/Trades/TradeImportService.cs) and identify the landing, validation, deduplication, dry-run preview, and publish stages.
5. Use [SqlAcademy.Api.http](../../../apps/SqlAcademy.Api/SqlAcademy.Api.http) to run one import request, inspect the persisted batch through `GET /api/v1/trades/import-batches/{batchId}`, query recent history with `pageNumber`, `pageSize`, `dryRun`, `source`, or `correlationId`, and inspect filtered row outcomes through `GET /api/v1/trades/import-batches/{batchId}/rows`.

## Tasks

1. Explain how [OrderConfiguration](../../../libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs) and [OrderWriteService](../../../libs/SqlAcademy.Persistence/Commands/Orders/OrderWriteService.cs) work together to make optimistic concurrency real.
2. Explain how [TradeImportService](../../../libs/SqlAcademy.Persistence/Commands/Trades/TradeImportService.cs) keeps landing, validation, batch deduplication, existing-trade deduplication, dry-run preview, and publish as distinct steps.
3. Explain how [TradeImportBatchReadService](../../../libs/SqlAcademy.Persistence/Queries/Trades/TradeImportBatchReadService.cs) lets you inspect a completed import after the POST request has returned.
4. Make one small improvement to either the order-status path or the trade-import contract.
5. If you want a concrete default task, add one focused test for an invalid status, a missing rowversion, a `404` order update, an invalid import row, or batch-history retrieval.
6. Rerun the focused order and trade tests until they pass.
7. Optionally finish the SQL-side reinforcement pack in [answer.sql](answer.sql) and validate it with `powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion`.

## Verification

- you can point to the exact line where the client rowversion becomes the concurrency token EF Core checks
- the order endpoint tests pass after your refinement
- the trade import endpoint reports clear counts for submitted, validated, duplicate, ready-to-publish, imported, and rejected rows
- rejected rows now expose stage and code metadata so you can tell validation failures from deduplication outcomes without inferring them
- each import response exposes a `batchId`, plus source and correlation metadata so you can inspect the same batch later without rerunning the import
- the batch-history list now follows the same paged API shape as other read endpoints, so you can page through recent imports and isolate dry runs
- import history is queryable by provenance, so you can isolate one lab run or request correlation without scanning every batch
- row-level inspection does not require loading an entire batch detail payload, because you can page and filter batch rows by outcome and stage
- you can explain where batch duplicates and existing-trade duplicates are separated in the code
- you can explain why `dryRun=true` separates validation from publish instead of just hiding a write behind a flag
- you can show where ready-to-publish, imported, and rejected outcomes are persisted for later inspection
- the SQL reinforcement pack is optional, not the primary ingestion contract

## Exit Criteria

You are done when you can reproduce a stale write, explain the resulting `409 Conflict`, and trace one imported trade batch from landing through publish and later inspection without guessing at the stages.