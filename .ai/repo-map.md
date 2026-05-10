# Repo Map

Use this map to choose the owning surface before editing. When in doubt, start with the most local surface that directly controls the behavior instead of a summary page.

## Documentation Routing And Learner Sequence

- Start with [../docs/README.md](../docs/README.md) for routing questions and [../docs/learning/curriculum-map.md](../docs/learning/curriculum-map.md) for the ordered learner path.
- Main owner surfaces: [../docs/learning](../docs/learning/README.md) and [../docs/phases](../docs/phases/README.md).
- Supporting anchors: [../README.md](../README.md) for repository shape, [../docs/exercises/README.md](../docs/exercises/README.md) for practice-mode rules, [../docs/learning/command-cheat-sheet.md](../docs/learning/command-cheat-sheet.md) for compact startup or validation command routing, and [../docs/learning/early-route-printable-checklists.md](../docs/learning/early-route-printable-checklists.md) for compact early-route completion checks.
- Do not lead with the root README when the real change belongs in a lesson, phase, or routing page.

## Exercises And Validation Packs

- Start with [../docs/exercises/README.md](../docs/exercises/README.md) and the specific pack under [../src/exercises](../src/exercises).
- Main owner surfaces: the exercise `README.md`, `starter.sql`, `answer.sql`, `validation.sql`, `expected-outcomes.md`, and any lab workbook in the specific pack.
- Supporting anchors: [../docs/learning/curriculum-map.md](../docs/learning/curriculum-map.md) and the linked lesson or phase doc.
- Do not start by changing the public lesson narrative when the contract is really inside one exercise pack.

## SQL Bootstrap, Seed, And Migrations

- Start with [../db/schemas](../db/schemas), [../db/seed](../db/seed), and [../db/migrations/README.md](../db/migrations/README.md).
- Main owner surfaces: [../db/schemas/001_create_learning_db.sql](../db/schemas/001_create_learning_db.sql), the relevant seed script, and [../src/libs/SqlAcademy.Migrations](../src/libs/SqlAcademy.Migrations).
- Supporting anchors: [../docs/learning/04-schema-design-and-migration-safety.md](../docs/learning/04-schema-design-and-migration-safety.md) and [../docs/sql/README.md](../docs/sql/README.md).
- Do not start in EF Core or operations summary pages when the change is really schema or seed behavior.

## Local Startup Tasks And First-Query Flow

- Start with [../docs/learning/sql-first-day-one.md](../docs/learning/sql-first-day-one.md), [../infra/scripts/start-database-first.ps1](../infra/scripts/start-database-first.ps1), and [../.vscode/tasks.json](../.vscode/tasks.json) when the change is about the reduced database-first path or first-run startup ergonomics.
- Main owner surfaces: [../infra/scripts/start-database-first.ps1](../infra/scripts/start-database-first.ps1), [../.vscode/tasks.json](../.vscode/tasks.json), and [../docs/learning/local-setup-troubleshooting.md](../docs/learning/local-setup-troubleshooting.md).
- Supporting anchors: [../docker-compose.yml](../docker-compose.yml), [../docs/learning/how-to-start.md](../docs/learning/how-to-start.md), and [../docs/phases/phase-0-local-setup.md](../docs/phases/phase-0-local-setup.md).
- Do not change the full Compose topology when the real work is only the first-query entry point or the learner-facing startup command.

## EF Core And Dapper Query Paths

- Start with [../docs/learning/09-ef-core-dapper-and-query-shape.md](../docs/learning/09-ef-core-dapper-and-query-shape.md) and the owning service under [../src/libs/SqlAcademy.Persistence/Queries](../src/libs/SqlAcademy.Persistence/Queries).
- Current concrete anchors: [../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs](../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), [../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs](../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), [../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs](../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs), and [../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs](../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs).
- Supporting anchors: [../docs/efcore/README.md](../docs/efcore/README.md) and [../docs/sql/README.md](../docs/sql/README.md).
- Do not start in a track summary page when the behavior lives in one query service or one focused test.

## API And Runtime Behavior

- Start with [../src/apps/SqlAcademy.Api](../src/apps/SqlAcademy.Api) and the nearest integration test under [../tests/SqlAcademy.IntegrationTests](../tests/SqlAcademy.IntegrationTests).
- Main owner surfaces: endpoint controllers, request and response contracts, and runtime wiring in `Program.cs`.
- Supporting anchors: [../docs/operations/README.md](../docs/operations/README.md), [../docs/learning/10-observability-testing-and-performance-engineering.md](../docs/learning/10-observability-testing-and-performance-engineering.md), and the relevant exercise pack.
- Do not begin by changing broad architecture notes when the contract is really one endpoint or one service registration.

## Operations, Observability, And Incident Drills

- Start with [../docs/operations/README.md](../docs/operations/README.md), [../infra/observability](../infra/observability), and the incident or release exercise that motivated the change.
- Main owner surfaces: [../infra/observability/otel-collector.yml](../infra/observability/otel-collector.yml), [../infra/observability/prometheus.yml](../infra/observability/prometheus.yml), [../src/exercises/Senior/001-concurrency-blocking-and-deadlocks](../src/exercises/Senior/001-concurrency-blocking-and-deadlocks), [../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency](../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency), [../src/exercises/Senior/004-posts-api-latency-and-observability-triage](../src/exercises/Senior/004-posts-api-latency-and-observability-triage), [../src/exercises/Senior/006-release-readiness-and-rollback-gates](../src/exercises/Senior/006-release-readiness-and-rollback-gates), [../docs/operations/release-runbook.md](../docs/operations/release-runbook.md), [../docs/operations/query-store-regression-runbook.md](../docs/operations/query-store-regression-runbook.md), [../docs/operations/deadlock-response-runbook.md](../docs/operations/deadlock-response-runbook.md), [../docs/operations/outbox-delivery-runbook.md](../docs/operations/outbox-delivery-runbook.md), and [../docs/operations/incident-triage-runbook.md](../docs/operations/incident-triage-runbook.md).
- Supporting anchors: [../docs/performance/README.md](../docs/performance/README.md), [../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement](../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement), and the API runtime files.
- Do not treat the whole platform as suspect when one endpoint, metric, or telemetry path is the real owner.

## Optional DBA And DBRE Specialization

- Start with [../docs/learning/dba-dbre-extension-track.md](../docs/learning/dba-dbre-extension-track.md) when the change is about backup and restore, job safety, HA or DR, security administration, row-level security or tenant isolation, or change-capture routing after the core learner path.
- Main owner surfaces: [../docs/learning/dba-dbre-extension-track.md](../docs/learning/dba-dbre-extension-track.md), [../docs/learning/row-level-security-and-tenant-isolation.md](../docs/learning/row-level-security-and-tenant-isolation.md), [../src/exercises/Senior/007-backup-restore-and-recovery-verification](../src/exercises/Senior/007-backup-restore-and-recovery-verification), [../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling](../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling), [../src/exercises/Senior/009-ha-dr-failover-posture-and-verification](../src/exercises/Senior/009-ha-dr-failover-posture-and-verification), [../src/exercises/Senior/010-change-capture-provenance-and-reconciliation](../src/exercises/Senior/010-change-capture-provenance-and-reconciliation), [../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries](../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries), [../src/exercises/Senior/012-row-level-security-and-tenant-isolation](../src/exercises/Senior/012-row-level-security-and-tenant-isolation), [../docs/sql/README.md](../docs/sql/README.md), and [../docs/operations/README.md](../docs/operations/README.md).
- Supporting anchors: [../infra/sqlserver/README.md](../infra/sqlserver/README.md), [../README.md](../README.md), [../docs/learning/curriculum-map.md](../docs/learning/curriculum-map.md), [../docs/learning/repository-improvement-suggestions.md](../docs/learning/repository-improvement-suggestions.md), and [sql-efcore-mastery-backlog.md](sql-efcore-mastery-backlog.md).
- Do not change the default learner route when the real work is optional specialization routing or future extension-track scaffolding.

## Performance And Regression Labs

- Start with [../docs/performance/README.md](../docs/performance/README.md), the relevant lesson, and the supporting test or lab under [../tests/SqlAcademy.PerformanceTests](../tests/SqlAcademy.PerformanceTests).
- Current concrete anchors: [../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs](../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs) and [../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs](../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs).
- Supporting anchors: [../db/performance/README.md](../db/performance/README.md) and the relevant exercise pack.
- Do not start with a benchmark or performance summary if the real question is one guided lab contract or one query path.

## AI Workflow And Maintainer Planning

- Start with [README.md](README.md) and [maintainer-workflow.md](maintainer-workflow.md).
- Main owner surfaces: [validation-matrix.md](validation-matrix.md), [templates/change-brief.md](templates/change-brief.md), [lesson-exercise-quality-backlog.md](lesson-exercise-quality-backlog.md), [academy-enhancements-backlog.md](academy-enhancements-backlog.md), and [sql-efcore-mastery-backlog.md](sql-efcore-mastery-backlog.md).
- Supporting anchors: [../CONTRIBUTING.md](../CONTRIBUTING.md) and [../docs/learning/repository-improvement-suggestions.md](../docs/learning/repository-improvement-suggestions.md).
- Do not store maintainer execution state in learner docs just because the work eventually affects the public route.

## Build, CI, And Tooling

- Start with [../.github/workflows](../.github/workflows), [../Directory.Build.props](../Directory.Build.props), [../Directory.Packages.props](../Directory.Packages.props), [../global.json](../global.json), and [../dotnet-tools.json](../dotnet-tools.json).
- Main owner surfaces: [../.github/workflows/ci.yml](../.github/workflows/ci.yml) and [../.github/workflows/docker-validation.yml](../.github/workflows/docker-validation.yml).
- Supporting anchors: [../README.md](../README.md) for the stated CI contract.
- Do not change workflow summaries before changing the actual workflow or build configuration that controls behavior.