# Operations Track

This page is the production-operations reference surface for the academy.

If you want the default learner route first, start in [Curriculum Map](../learning/curriculum-map.md). Use this page when you already know the operational concern you want to inspect and need the fastest route to the right lesson, platform anchor, exercise, or validation workflow.

Do not use this page as the main curriculum index. It is a reference page for release safety, runtime verification, and incident-style reasoning after the core learning path is already in motion.

## Scope Boundary

The current operations route targets deterministic local bootstrap, startup sequencing, migration safety, health and telemetry review, incident triage, and rollback versus roll-forward discipline.

It does not yet claim full DBA or DBRE mastery across backup and restore, SQL Server Agent, HA or DR, replication, or security administration. Those now route through [DBA And DBRE Extension Track](../learning/dba-dbre-extension-track.md) after the core platform and release workflows are stronger.

The tracked backlog for deeper future expansion lives in [../../.ai/sql-efcore-mastery-backlog.md](../../.ai/sql-efcore-mastery-backlog.md).

Coverage targets:
- environment bootstrap and reproducibility
- startup ordering and readiness signals
- migrations and release safety
- health checks, logs, metrics, and traces during change windows
- blocking, deadlock, and API regression triage
- rollback strategy, roll-forward discipline, and change audit habits
- validation harnesses and CI guardrails

## When To Use This Page

- when you need the nearest lesson, exercise, script, or workflow for a release or incident concern you already recognize
- when you want to connect Docker bootstrap, API readiness, telemetry, and validation into one operational story
- when you need concrete repo anchors for first-five-minutes watch plans, smoke tests, or migration review
- when you want to move from a vague “the system is unhealthy” statement to a narrower boundary and validation surface

## When Not To Use This Page

- when you still need the ordered learner path; use [Curriculum Map](../learning/curriculum-map.md)
- when the main question is optional DBA or DBRE specialization rather than the core release or incident route; use [DBA And DBRE Extension Track](../learning/dba-dbre-extension-track.md)
- when the main question is SQL query design or execution-plan interpretation; use [SQL Track](../sql/README.md) and [Performance Track](../performance/README.md)
- when the main question is application-side EF Core or Dapper boundaries; use [EF Core Track](../efcore/README.md)

## Operations Reference Matrix

| Concern | Main Teaching Surface | Concrete Repo Anchors | Practice And Validation | Common Failure Mode |
| --- | --- | --- | --- | --- |
| deterministic local bootstrap and readiness | [How To Start](../learning/how-to-start.md), [Lesson 08](../learning/08-operational-engineering-and-release-safety.md), [Phase 0](../phases/phase-0-local-setup.md) | [docker-compose.yml](../../docker-compose.yml), [init-database.sh](../../infra/scripts/init-database.sh), [SQL Server infra notes](../../infra/sqlserver/README.md), [SqlAcademy.Api Program](../../src/apps/SqlAcademy.Api/Program.cs) | stack startup plus `/health/ready`, `/health/live`, and `/metrics` checks | assuming a running container means the system is ready for useful work |
| migration rollout and contract safety | [Lesson 04](../learning/04-schema-design-and-migration-safety.md), [Lesson 08](../learning/08-operational-engineering-and-release-safety.md), [Phase 7](../phases/phase-7-operational-engineering.md) | [db/migrations notes](../../db/migrations/README.md), [SqlAcademy.Migrations](../../src/libs/SqlAcademy.Migrations/SqlAcademy.Migrations.csproj), [DesignTime factory](../../src/libs/SqlAcademy.Migrations/DesignTimeLearningDbContextFactory.cs) | [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md) | treating generated migrations as automatically rollout-safe |
| release readiness and rollback gates | [Lesson 08](../learning/08-operational-engineering-and-release-safety.md), [Phase 7](../phases/phase-7-operational-engineering.md) | [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md), [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md), [SqlAcademy.Api Program](../../src/apps/SqlAcademy.Api/Program.cs), [OpenTelemetry setup](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs) | [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) and [ReleaseReadinessDrillTests](../../tests/SqlAcademy.PerformanceTests/ReleaseReadinessDrillTests.cs) | approving a release from the migration diff alone without explicit watch signals or stop conditions |
| health checks, logs, metrics, and traces | [Lesson 08](../learning/08-operational-engineering-and-release-safety.md), [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md) | [SqlAcademy.Api Program](../../src/apps/SqlAcademy.Api/Program.cs), [OpenTelemetry setup](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs), [Prometheus config](../../infra/observability/prometheus.yml), [OTel collector config](../../infra/observability/otel-collector.yml) | [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | deciding what to watch only after the release has already gone wrong |
| blocking, deadlocks, and concurrency response | [Lesson 05](../learning/05-transactions-blocking-and-deadlocks.md), [Phase 4](../phases/phase-4-transactions-and-concurrency.md), [Operations Track](README.md) | [DeadlockReproductionTests](../../tests/SqlAcademy.PerformanceTests/DeadlockReproductionTests.cs), [Senior 001 deadlock graph lab](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/deadlock-graph-lab.md), [OrdersController](../../src/apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs), [OrderConfiguration](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs) | [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) | blaming application code for every timeout without lock or wait evidence |
| idempotent delivery and operational consistency checks | [Lesson 08](../learning/08-operational-engineering-and-release-safety.md), [Exercise System](../exercises/README.md) | [OrdersController](../../src/apps/SqlAcademy.Api/Controllers/V1/OrdersController.cs), [schema bootstrap](../../db/schemas/001_create_learning_db.sql), [validation harness](../../infra/scripts/run-exercise-validation.ps1) | [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) | writing delivery logic that duplicates messages or loses deterministic dispatch order |
| incident-style API regression narrowing | [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md), [Performance Track](../performance/README.md) | [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs), [SqlAcademy.Api Program](../../src/apps/SqlAcademy.Api/Program.cs), [OpenTelemetry setup](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs) | [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | treating the whole platform as suspect instead of narrowing to one endpoint and one query path |
| repeatable validation and CI guardrails | [Exercise System](../exercises/README.md), [Lesson 10](../learning/10-observability-testing-and-performance-engineering.md) | [run-exercise-validation.ps1](../../infra/scripts/run-exercise-validation.ps1), [ci.yml](../../.github/workflows/ci.yml), [docker-validation.yml](../../.github/workflows/docker-validation.yml) | validation packs, focused tests, and Compose validation | relying on ad hoc manual checks instead of repeatable guardrails |

## Best Entry Points

- [How To Start](../learning/how-to-start.md)
- [Phase 7: Operational Engineering](../phases/phase-7-operational-engineering.md)
- [Lesson 08: Operational Engineering And Release Safety](../learning/08-operational-engineering-and-release-safety.md)
- [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](../learning/10-observability-testing-and-performance-engineering.md)
- [Senior 004: Posts API Latency And Observability Triage](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)

## Fast Jumps By Need

- stack starts but readiness still feels unclear: [docker-compose.yml](../../docker-compose.yml), [init-database.sh](../../infra/scripts/init-database.sh), and [SqlAcademy.Api Program](../../src/apps/SqlAcademy.Api/Program.cs)
- planning a schema change, backfill, or compatibility window: [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md) and [SqlAcademy.Migrations](../../src/libs/SqlAcademy.Migrations/SqlAcademy.Migrations.csproj)
- need a go or no-go release drill before a schema-affecting window opens: [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md), [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md), and [OpenTelemetry setup](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs)
- need a printable release checklist for a change window: [Release Runbook](release-runbook.md)
- need a first-five-minutes watch plan for a release: [Lesson 08](../learning/08-operational-engineering-and-release-safety.md), [Prometheus config](../../infra/observability/prometheus.yml), and [OpenTelemetry setup](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs)
- need a printable Query Store regression checklist: [Query Store Regression Runbook](query-store-regression-runbook.md)
- need to reproduce or reason about blocking and deadlocks: [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md), [Senior 001 deadlock graph lab](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/deadlock-graph-lab.md), and [DeadlockReproductionTests](../../tests/SqlAcademy.PerformanceTests/DeadlockReproductionTests.cs)
- need a printable deadlock-response checklist: [Deadlock Response Runbook](deadlock-response-runbook.md)
- need an idempotent consistency check for a data-moving workflow: [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- need a printable outbox consistency checklist: [Outbox Delivery Consistency Runbook](outbox-delivery-runbook.md)
- need to narrow a slow-but-healthy API incident: [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) and [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs)
- need a printable incident narrowing checklist: [Incident Triage Runbook](incident-triage-runbook.md)
- need backup and restore recovery verification after the core route: [DBA And DBRE Extension Track](../learning/dba-dbre-extension-track.md) and [Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md)
- need optional DBA or DBRE specialization after the core route: [DBA And DBRE Extension Track](../learning/dba-dbre-extension-track.md)
- need a repeatable validation or CI baseline: [run-exercise-validation.ps1](../../infra/scripts/run-exercise-validation.ps1), [ci.yml](../../.github/workflows/ci.yml), and [docker-validation.yml](../../.github/workflows/docker-validation.yml)

## Printable Runbooks

- [Release Runbook](release-runbook.md) compresses the Senior 006 release decision and first-five-minutes watch plan into a one-page change-window checklist.
- [Query Store Regression Runbook](query-store-regression-runbook.md) compresses the Advanced 004 persisted-regression workflow into a one-page Query Store checklist.
- [Deadlock Response Runbook](deadlock-response-runbook.md) compresses the Senior 001 blocking and deadlock response loop into a one-page concurrency checklist.
- [Outbox Delivery Consistency Runbook](outbox-delivery-runbook.md) compresses the Senior 003 idempotent outbox and deterministic dispatch contract into a one-page delivery checklist.
- [Incident Triage Runbook](incident-triage-runbook.md) compresses the Senior 004 boundary-narrowing flow into a one-page degraded-service checklist.

## Operational Loop In This Repo

Use this page with an explicit operational loop instead of vague environment confidence:

1. start from a deterministic stack and known seed state
2. verify readiness and observability surfaces before changing anything
3. choose the smallest smoke test that proves useful work, not just process startup
4. apply or simulate one change at a time
5. watch the first relevant logs, metrics, traces, and health signals immediately
6. decide rollback versus stabilizing roll-forward based on evidence, not on panic or habit

## Concrete Smoke Surfaces

The repo already gives you a small but real production-style watch surface:

- `/health/live` proves the process is up
- `/health/ready` proves dependencies and startup work are in place
- `/metrics` exposes Prometheus-readable runtime and request data
- [PostsEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs), [OrdersEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/OrdersEndpointTests.cs), and [TradesEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/TradesEndpointTests.cs) show concrete API contract checks
- [run-exercise-validation.ps1](../../infra/scripts/run-exercise-validation.ps1) provides repeatable validation for SQL exercise packs

## Fast Validation Commands

```bash
docker compose up --build
powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Senior/003-transactional-outbox-and-delivery-consistency
dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryStoreLabSmokeTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~OperationsRunbookAssetTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~BackupRestoreRecoveryDrillAssetTests"
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~ReleaseReadinessDrillAssetTests"
```

Use `docker compose config` when you want a quick structural check before starting the full stack or pushing a Compose change into CI.

## Linked Exercises

- [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Advanced 004](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
- [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)
- [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)
- [Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md)

## Bridge To SQL, Performance, And EF Core

This page is operational by design, but it depends on the earlier tracks rather than replacing them.

- query-shape and locking reasoning still start in [SQL Track](../sql/README.md)
- performance baselines, plan reasoning, and Query Store investigation still belong in [Performance Track](../performance/README.md)
- EF Core, Dapper, concurrency tokens, and generated-SQL inspection still belong in [EF Core Track](../efcore/README.md)

The Docker stack, SQL bootstrap scripts, observability wiring, and CI workflows are the operational platform for this material.