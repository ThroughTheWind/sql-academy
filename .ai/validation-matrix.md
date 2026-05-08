# Validation Matrix

Match the first check to the claim you are making. Run the narrowest executable validation before widening to broader builds or CI-style sweeps.

## Markdown, Routing, And Maintainer Docs

- Use when: you changed Markdown links, documentation routing, maintainer workflow docs, or other text-only guidance.
- First check: `powershell -ExecutionPolicy Bypass -File infra/scripts/check-markdown-links.ps1`
- Escalate when needed: if the change also affects generated commands or code snippets, run the nearest code or script validation as well.

## Printable Runbooks And Operational Checklists

- Use when: you changed printable runbooks, release checklists, or incident-response cheat sheets under `docs/operations`.
- First check: `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~OperationsRunbookAssetTests"`
- Escalate when needed: `powershell -ExecutionPolicy Bypass -File infra/scripts/check-markdown-links.ps1`

## Validation Packs

- Use when: you changed a pack that ships `starter.sql`, `answer.sql`, and `validation.sql`.
- First check: `powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise <relative-exercise-path>`
- Escalate when needed: if the pack also changed application code, supplemental lab assets, or migrations, add the nearest focused test or build after the harness passes.
- Current concrete companion example: `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~DeadlockGraphLabAssetTests"`

## Guided Labs And Investigation Packs

- Use when: you changed a guided lab, manual investigation pack, or expected-outcomes contract.
- First check: run the nearest focused executable companion if one exists; otherwise use the README completion criteria plus `expected-outcomes.md` as the explicit manual contract.
- Current reusable companions: `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"`, `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~TrackingBehaviorTests"`, `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~PlanCacheLabSmokeTests"`, `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryStoreLabSmokeTests"`, `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~PostsApiObservabilityDrillAssetTests"`, `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~ReleaseReadinessDrillAssetTests"`, `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~BackupRestoreRecoveryDrillAssetTests"`, and `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~SqlServerAgentSchedulingDrillAssetTests"`

## EF Core Read Paths And Query Shape

- Use when: you changed a projection, tracking behavior, paging, or generated-SQL-sensitive EF Core path.
- First check: `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"`
- Escalate when needed: `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~TrackingBehaviorTests"`

## Performance And Regression Labs

- Use when: you changed a Query Store lab, performance comparison, or regression-triage companion surface.
- First check: `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryStoreLabSmokeTests"`
- Escalate when needed: run the nearest additional performance or integration test that exercises the same query path.

## Performance Workload Scripts

- Use when: you changed `db/performance` workload scripts or docs that rely on a specific synthetic-volume setup.
- First check: initialize the baseline database, then execute the workload script with a small parameter set.
- Current concrete example: `docker compose run --rm sqlserver-init /bin/bash /workspace/infra/scripts/init-database.sh` followed by `docker compose run --rm --no-deps sqlserver-init /bin/bash -lc '/opt/mssql-tools18/bin/sqlcmd -S sqlserver,1433 -U sa -P "$MSSQL_SA_PASSWORD" -d LearningDb -C -v SyntheticUserCount=4 PostsPerUser=5 MaxCommentsPerPost=3 TradeDays=3 TradesPerUserPerDay=4 -i /workspace/db/performance/001_high_cardinality_workload_variant.sql'`
- Escalate when needed: rerun the nearest guided lab, performance test, or benchmark against the expanded workload.

## Unit-Level .NET Changes

- Use when: you changed application logic that already has in-process contract tests.
- First check: `dotnet test tests/SqlAcademy.UnitTests/SqlAcademy.UnitTests.csproj -v minimal`
- Escalate when needed: `dotnet build SqlAcademy.slnx -v minimal`

## API Runtime Or Endpoint Behavior

- Use when: you changed controllers, HTTP contracts, request handling, or runtime wiring.
- First check: run the nearest focused integration test under `tests/SqlAcademy.IntegrationTests`.
- Current concrete example: `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"`
- Escalate when needed: `dotnet build SqlAcademy.slnx -v minimal`

## Docker, Platform Shape, And Compose Wiring

- Use when: you changed `docker-compose.yml`, container wiring, or application image contracts.
- First check: `docker compose config`
- Escalate when needed: `docker compose build api worker`

## Build, Package, And Shared Tooling Changes

- Use when: you changed workflow files, build props, package versions, SDK pins, or shared tools.
- First check: `dotnet build SqlAcademy.slnx -v minimal`
- Escalate when needed: `dotnet format SqlAcademy.slnx --verify-no-changes --no-restore`

## When No Narrow Executable Check Exists

- Use the owning README, `expected-outcomes.md`, or exercise contract as the manual completion bar.
- Record the exact manual check in the PR or change brief instead of saying the change was only "reviewed manually."
- If the same manual check keeps recurring, capture it in this file or promote it into a reusable script or focused test.