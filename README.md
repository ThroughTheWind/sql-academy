# SqlAcademy

SqlAcademy is a long-lived engineering training platform for SQL Server, .NET 10, EF Core 10, Dapper, performance engineering, and production operations. It is designed to be Docker-first, exercise-driven, documentation-first, and structured for incremental learning from beginner through senior-level scenarios.

## Quick Start

1. Restore the local tool manifest: `dotnet tool restore`
2. Start the full learning stack: `docker compose up --build`
3. Open the API at `http://localhost:8080/openapi/v1.json`
4. Open Prometheus at `http://localhost:9090`
5. Open Grafana at `http://localhost:3000` with `admin` / `admin`
6. Run the fast validation slice: `dotnet test tests/SqlAcademy.UnitTests/SqlAcademy.UnitTests.csproj`

## Start Learning

If you want the shortest useful path through the repository, start here:

1. Read the [How To Start guide](docs/learning/how-to-start.md).
2. Complete [Phase 0: Local Setup](docs/phases/phase-0-local-setup.md).
3. Read [Lesson 01: SQL Fundamentals](docs/learning/01-sql-fundamentals.md).
4. Complete the [Beginner exercise pack](src/exercises/Beginner/001-joins-and-aggregations/README.md).
5. Move to [Lesson 02: Joins And Aggregations](docs/learning/02-joins-and-aggregations.md) and [Lesson 03: Window Functions And Intermediate Querying](docs/learning/03-window-functions-and-intermediate-querying.md).
6. Use the [Learning Docs index](docs/learning/README.md) as the main progression map after that point.

## Learner Paths

Use the path that matches your current goal.

| Goal | Start Here | Continue With | First Exercise |
| --- | --- | --- | --- |
| SQL beginner | [How To Start](docs/learning/how-to-start.md) | [Lesson 01](docs/learning/01-sql-fundamentals.md), [Lesson 02](docs/learning/02-joins-and-aggregations.md) | [Beginner 001](src/exercises/Beginner/001-joins-and-aggregations/README.md) |
| Intermediate SQL engineer | [Lesson 03](docs/learning/03-window-functions-and-intermediate-querying.md) | [Lesson 04](docs/learning/04-schema-design-and-migration-safety.md), [Lesson 05](docs/learning/05-transactions-blocking-and-deadlocks.md) | [Intermediate 001](src/exercises/Intermediate/001-window-functions-and-pagination/README.md) |
| Performance and production focus | [Lesson 06](docs/learning/06-indexing-execution-plans-and-parameter-sensitivity.md) | [Lesson 07](docs/learning/07-sql-server-internals.md), [Lesson 08](docs/learning/08-operational-engineering-and-release-safety.md) | [Advanced 001](src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) |
| .NET and data-platform integration | [Lesson 09](docs/learning/09-ef-core-dapper-and-query-shape.md) | [Lesson 10](docs/learning/10-observability-testing-and-performance-engineering.md), [Lesson 11](docs/learning/11-capstones-and-interview-readiness.md) | [Senior 002](src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) |
| Interview and senior-practice track | [Lesson 05](docs/learning/05-transactions-blocking-and-deadlocks.md) | [Lesson 08](docs/learning/08-operational-engineering-and-release-safety.md), [Lesson 11](docs/learning/11-capstones-and-interview-readiness.md) | [Senior 001](src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) |

## Learning Docs

The learner-oriented lesson set lives in [docs/learning](docs/learning/README.md).

Use the [Learning Glossary](docs/learning/glossary.md) when vocabulary slows you down, and use the checkpoint section at the end of each lesson before moving on.

| Subject | Lesson | Primary Phase | Primary Exercise |
| --- | --- | --- | --- |
| SQL fundamentals | [Lesson 01](docs/learning/01-sql-fundamentals.md) | [Phase 1](docs/phases/phase-1-sql-fundamentals.md) | [Beginner 001](src/exercises/Beginner/001-joins-and-aggregations/README.md) |
| Joins and aggregations | [Lesson 02](docs/learning/02-joins-and-aggregations.md) | [Phase 2](docs/phases/phase-2-intermediate-querying.md) | [Beginner 001](src/exercises/Beginner/001-joins-and-aggregations/README.md) |
| Window functions and analytical SQL | [Lesson 03](docs/learning/03-window-functions-and-intermediate-querying.md) | [Phase 2](docs/phases/phase-2-intermediate-querying.md) | [Intermediate 001](src/exercises/Intermediate/001-window-functions-and-pagination/README.md) |
| Schema design and migration safety | [Lesson 04](docs/learning/04-schema-design-and-migration-safety.md) | [Phase 3](docs/phases/phase-3-schema-design.md) | [Advanced 001](src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) |
| Transactions, blocking, and deadlocks | [Lesson 05](docs/learning/05-transactions-blocking-and-deadlocks.md) | [Phase 4](docs/phases/phase-4-transactions-and-concurrency.md) | [Senior 001](src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) |
| Indexing, execution plans, and parameter sensitivity | [Lesson 06](docs/learning/06-indexing-execution-plans-and-parameter-sensitivity.md) | [Phase 5](docs/phases/phase-5-indexing-and-performance.md) | [Advanced 001](src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) |
| SQL Server internals | [Lesson 07](docs/learning/07-sql-server-internals.md) | [Phase 6](docs/phases/phase-6-sql-server-internals.md) | [Follow-up ideas](docs/learning/follow-up-exercises-by-level.md#advanced-follow-up-exercises) |
| Operational engineering | [Lesson 08](docs/learning/08-operational-engineering-and-release-safety.md) | [Phase 7](docs/phases/phase-7-operational-engineering.md) | [Senior 001](src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md), [Senior 004](src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) |
| EF Core, Dapper, and query shape | [Lesson 09](docs/learning/09-ef-core-dapper-and-query-shape.md) | [Phase 8](docs/phases/phase-8-dotnet-and-efcore-integration.md) | [Senior 002](src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) |
| Observability, testing, and performance engineering | [Lesson 10](docs/learning/10-observability-testing-and-performance-engineering.md) | [Phase 7](docs/phases/phase-7-operational-engineering.md), [Phase 8](docs/phases/phase-8-dotnet-and-efcore-integration.md) | [Advanced 001](src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md), [Senior 004](src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) |
| Capstones and interview readiness | [Lesson 11](docs/learning/11-capstones-and-interview-readiness.md) | [Phase 9](docs/phases/phase-9-capstone-projects.md) | [Senior exercises](src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md), [Senior 004](src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) |

## Exercise Ladder

Use this ladder if you want a clear progression from the README without browsing the repo tree.

1. [Beginner 001: Joins And Aggregations](src/exercises/Beginner/001-joins-and-aggregations/README.md)
2. [Beginner 002: Filtering, Constraints, And Data Quality](src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md)
3. [Intermediate 001: Window Functions And Pagination](src/exercises/Intermediate/001-window-functions-and-pagination/README.md)
4. [Intermediate 002: Cohort Analysis And Pagination Drift](src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
5. [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
6. [Advanced 002: Staged Backfill And Contract Enforcement](src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
7. [Senior 001: Concurrency, Blocking, And Deadlocks](src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
8. [Senior 002: EF Core Concurrency And Bulk Ingestion](src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
9. [Senior 003: Transactional Outbox And Delivery Consistency](src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
10. [Senior 004: Posts API Latency And Observability Triage](src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)

## What To Do After Each Level

- Beginner next pack: [Beginner 002](src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md)
- Intermediate next pack: [Intermediate 002](src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
- Advanced next pack: [Advanced 002](src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- Senior next pack: [Senior 003](src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- More ideas by level live in [Follow-Up Exercises By Level](docs/learning/follow-up-exercises-by-level.md).

## Exercise Validation

Validation-ready packs include `answer.sql` and `validation.sql` so learners can self-check quickly.

That workflow now covers Beginner 001-002, Intermediate 001-002, Advanced 001-002, Senior 001, and Senior 003.

Run the harness with:

`powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Advanced/002-staged-backfill-and-contract-enforcement`

## Subsequent Improvement Suggestions

The next repository-level improvements are tracked in [Repository Improvement Suggestions](docs/learning/repository-improvement-suggestions.md).

## Repository Bootstrap Plan

- Establish a boring, explicit architecture with clear application, domain, persistence, and observability boundaries.
- Ship a deterministic local platform first: SQL Server, API, worker, OpenTelemetry Collector, Prometheus, and Grafana.
- Teach from real assets: migrations, schema scripts, seed data, integration tests, and production-adjacent troubleshooting labs.
- Progressively layer complexity across phases instead of front-loading advanced topics.
- Keep the repository extension-friendly by centralizing package versions, build configuration, and infrastructure conventions.
- Treat documentation and exercises as first-class deliverables, not post-implementation notes.

## Repository Tree

```text
/
├── README.md
├── ACADEMY_ROADMAP.md
├── docker-compose.yml
├── .env
├── .env.example
├── .editorconfig
├── .gitignore
├── global.json
├── dotnet-tools.json
├── Directory.Build.props
├── Directory.Packages.props
├── SqlAcademy.slnx
├── docs/
├── infra/
├── db/
├── src/
├── tests/
└── playground/
```

## Initial Architecture

- `SqlAcademy.Api` is the HTTP entry point for guided SQL and data-access scenarios.
- `SqlAcademy.Worker` is a background host used for operational and observability demonstrations.
- `SqlAcademy.Domain` owns the relational model and aggregate vocabulary.
- `SqlAcademy.Persistence` owns EF Core mappings, Dapper query paths, the database initializer, and the hybrid access strategy.
- `SqlAcademy.Migrations` owns the generated EF Core migration assembly.
- `SqlAcademy.Observability` owns OpenTelemetry and Serilog defaults shared by the API and worker.
- `SqlAcademy.SharedKernel` owns low-level cross-cutting primitives such as paging and sorting.

The hybrid data-access boundary is intentional:

- EF Core is used where rich mapping, change tracking, and optimistic concurrency matter.
- Dapper is used where the teaching goal is SQL shape, execution plans, or explicit query control.

## Docker Architecture

- `sqlserver` runs SQL Server 2022 Developer Edition with a persistent volume.
- `sqlserver-init` applies schema and seed scripts deterministically on stack startup.
- `api` and `worker` run as application containers so the repo stays Docker-first instead of relying on hidden host assumptions.
- `otel-collector` receives OTLP telemetry from .NET services.
- `prometheus` scrapes `/metrics` from the API and telemetry metrics from the collector.
- `grafana` is preconfigured with Prometheus as the default datasource.

## Database Strategy

- The logical database is `LearningDb`.
- The application schema is `academy`.
- Raw SQL bootstrap assets live in `db/schemas` and `db/seed`.
- EF Core schema evolution lives in `src/libs/SqlAcademy.Migrations`.
- Seed data is available in both SQL bootstrap scripts and the runtime initializer so Docker, app startup, and tests all stay aligned.
- The initial model includes `Users`, `Posts`, `Comments`, `Orders`, `Trades`, and `Instruments` to support relational, analytical, concurrency, and operational labs.

## Learning Progression Strategy

- Phase 0 builds the local platform and toolchain confidence.
- Phases 1-3 teach SQL fundamentals, intermediate querying, and relational design.
- Phases 4-7 shift into concurrency, indexing, internals, and production operations.
- Phase 8 introduces .NET 10, EF Core 10, and hybrid EF + Dapper patterns against the same database.
- Phase 9 closes with capstones, incident-style debugging, and interview-grade problem solving.

See `ACADEMY_ROADMAP.md` and `docs/phases` for the detailed phase plans.

## CI/CD Strategy

- Central package management and shared build props keep restore and build behavior deterministic.
- `ci.yml` restores tools, restores packages, builds, runs unit tests, compiles heavier test projects, verifies formatting, and uploads TRX results.
- `docker-validation.yml` validates the Compose model and builds the application images.
- The current bootstrap keeps integration and performance tests compile-ready in CI while leaving container execution opt-in for later pipeline stages.

## Step-by-Step Implementation Roadmap

1. Bring up the full Docker stack and verify SQL, API, and observability endpoints.
2. Walk Phase 0 and Phase 1 with only the SQL bootstrap assets and the seed data.
3. Add more exercise packs under `src/exercises` without changing the core application layout.
4. Expand the API surface to expose additional performance and concurrency scenarios.
5. Introduce richer operational scripts in `db/performance` and `db/migrations`.
6. Promote selected integration tests into fully automated container tests in CI.
7. Add dashboards, alert rules, and more detailed telemetry signals.
8. Introduce capstone services or bounded contexts when the learning platform outgrows the initial schema.

## Suggested First Commits

1. `bootstrap: create solution, central build config, and project graph`
2. `bootstrap: add domain model, persistence layer, and initial migration`
3. `bootstrap: add API, worker, observability, and Docker stack`
4. `bootstrap: add docs roadmap and first exercise packs`
5. `bootstrap: add CI workflows and test scaffolding`

## Risks And Tradeoffs

- Auto-applying migrations at API startup is convenient for local learning but should be gated more carefully in higher environments.
- The first observability slice prioritizes portability and local clarity over a full production telemetry backend such as Tempo or Jaeger.
- The SQL bootstrap scripts and runtime seed both exist on purpose; that duplication improves determinism for learners but requires discipline when the model evolves.
- Integration and performance tests are compile-ready and container-backed, but the default CI flow does not yet execute them to keep the bootstrap fast and reliable.

## Future Extension Ideas

- Add SQL Server Agent-style scheduling labs and backup/restore drills.
- Add a dedicated migrations review pipeline with generated SQL artifacts.
- Add dashboards and alerts for blocking chains, p95 latency, and retry storms.
- Add a second API focused on ingestion and bulk-write patterns.
- Add Kubernetes manifests or Helm charts after the local platform and exercises stabilize.