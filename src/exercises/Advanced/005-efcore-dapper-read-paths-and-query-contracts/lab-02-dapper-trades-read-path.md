# Lab 02: Dapper Trades Read Path

## Objective

Use the explicit Dapper trade query as a contract-first read path: prove the current behavior, then extend one filter or sort without weakening deterministic paging.

## Start Here

- [TradesController](../../../apps/SqlAcademy.Api/Controllers/V1/TradesController.cs)
- [TradeReadService](../../../libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)
- [TradesEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs)
- [SqlAcademy.Api.http](../../../apps/SqlAcademy.Api/SqlAcademy.Api.http)

## Baseline Run

1. Run `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~TradesEndpointTests"`.
2. Read the SQL in [TradeReadService](../../../libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs) and identify the filter clause, sort selection, paging clause, and count query.
3. Optionally call `/api/v1/trades` through [SqlAcademy.Api.http](../../../apps/SqlAcademy.Api/SqlAcademy.Api.http) if your local API is already running.

## Tasks

1. Explain why `ORDER BY {sortColumn} {sortDirection}, t.Id DESC` is the line that protects deterministic paging.
2. Make one deliberate extension to the contract without losing explicit SQL control.
3. If you want a concrete default task, add a `sortBy=side` option and keep the `t.Id DESC` tie-breaker.
4. Extend [TradesEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs) with one focused assertion for the new or refined behavior.
5. Rerun the focused trade tests until they pass.

## Verification

- the trade query still exposes the full SQL shape directly in one place
- the added filter or sort keeps deterministic paging intact
- the focused trade tests pass after the change
- you can explain why this path is a better Dapper candidate than a tracked EF Core write model