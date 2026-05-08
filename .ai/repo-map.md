# Repo Map

Use this map to choose the owning surface before editing. When in doubt, start with the most local surface that directly controls the behavior instead of a summary page.

## Documentation Routing And Learner Sequence

- Start with [../docs/README.md](../docs/README.md) for routing questions and [../docs/learning/curriculum-map.md](../docs/learning/curriculum-map.md) for the ordered learner path.
- Main owner surfaces: [../docs/learning](../docs/learning/README.md) and [../docs/phases](../docs/phases/README.md).
- Supporting anchors: [../README.md](../README.md) for repository shape and [../docs/exercises/README.md](../docs/exercises/README.md) for practice-mode rules.
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
- Main owner surfaces: [../infra/observability/otel-collector.yml](../infra/observability/otel-collector.yml), [../infra/observability/prometheus.yml](../infra/observability/prometheus.yml), [../src/exercises/Senior/004-posts-api-latency-and-observability-triage](../src/exercises/Senior/004-posts-api-latency-and-observability-triage), and [../src/exercises/Senior/006-release-readiness-and-rollback-gates](../src/exercises/Senior/006-release-readiness-and-rollback-gates).
- Supporting anchors: [../docs/performance/README.md](../docs/performance/README.md), [../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement](../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement), and the API runtime files.
- Do not treat the whole platform as suspect when one endpoint, metric, or telemetry path is the real owner.

## Performance And Regression Labs

- Start with [../docs/performance/README.md](../docs/performance/README.md), the relevant lesson, and the supporting test or lab under [../tests/SqlAcademy.PerformanceTests](../tests/SqlAcademy.PerformanceTests).
- Current concrete anchors: [../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs](../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs) and [../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs](../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs).
- Supporting anchors: [../db/performance/README.md](../db/performance/README.md) and the relevant exercise pack.
- Do not start with a benchmark or performance summary if the real question is one guided lab contract or one query path.

## AI Workflow And Maintainer Planning

- Start with [README.md](README.md) and [maintainer-workflow.md](maintainer-workflow.md).
- Main owner surfaces: [validation-matrix.md](validation-matrix.md), [templates/change-brief.md](templates/change-brief.md), and [sql-efcore-mastery-backlog.md](sql-efcore-mastery-backlog.md).
- Supporting anchors: [../CONTRIBUTING.md](../CONTRIBUTING.md) and [../docs/learning/repository-improvement-suggestions.md](../docs/learning/repository-improvement-suggestions.md).
- Do not store maintainer execution state in learner docs just because the work eventually affects the public route.

## Build, CI, And Tooling

- Start with [../.github/workflows](../.github/workflows), [../Directory.Build.props](../Directory.Build.props), [../Directory.Packages.props](../Directory.Packages.props), [../global.json](../global.json), and [../dotnet-tools.json](../dotnet-tools.json).
- Main owner surfaces: [../.github/workflows/ci.yml](../.github/workflows/ci.yml) and [../.github/workflows/docker-validation.yml](../.github/workflows/docker-validation.yml).
- Supporting anchors: [../README.md](../README.md) for the stated CI contract.
- Do not change workflow summaries before changing the actual workflow or build configuration that controls behavior.