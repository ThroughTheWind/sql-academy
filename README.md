# SqlAcademy

SqlAcademy is a long-lived engineering training platform for SQL Server, .NET 10, EF Core 10, Dapper, performance engineering, and production operations. It is designed to be Docker-first, exercise-driven, and documentation-first, with a guided route that starts at SQL fundamentals and grows into senior-level scenarios for working backend and application engineers.

## Scope

The current core route targets working backend and application engineers who need stronger SQL Server, EF Core, Dapper, performance, and production-operability fluency.

It is a good fit when you are already comfortable with a local development environment, source control, and application code, but you want a more deliberate SQL Server learning path.

In this repository, `beginner` means beginner to SQL Server and relational reasoning inside an existing engineering workflow, not beginner to software development itself.

It is not yet a zero-assumption course for someone learning their first developer toolchain or first-ever SQL syntax at the same time.

It does not currently claim full DBA or DBRE mastery. Backup and restore, SQL Server Agent, HA/DR, replication, security administration, and similar operational specialties remain optional extension-track work after the core route is stronger.

Use [DBA And DBRE Extension Track](docs/learning/dba-dbre-extension-track.md) when you want that optional specialization without changing the default learning route.

The maintainer execution backlog for closing those gaps lives in [.ai/sql-efcore-mastery-backlog.md](.ai/sql-efcore-mastery-backlog.md).

## Quick Start

1. Review [.env](.env) before first startup and leave the defaults alone unless a local port or password conflicts with your machine. Use [.env.example](.env.example) as the reference for supported settings.
2. Restore the local tool manifest: `dotnet tool restore`
3. Start the full learning stack: `docker compose up --build`
4. Complete the minimum day-one checks: open `http://localhost:8080/health/ready` and verify `SELECT COUNT(*) FROM academy.Users` against `LearningDb`
5. Treat OpenAPI, Prometheus, Grafana, and the fast unit-test slice as follow-up checks after the API and database already feel predictable

## Start Learning

Use [Curriculum Map](docs/learning/curriculum-map.md) as the authoritative lesson order, practice mapping, and default route contract. Use the quick split below only to choose the right starting surface.

If SQL is still brand new, move more slowly than the sample pacing and treat Lesson 01, the glossary, and Beginner 000 as the first checkpoint before you continue deeper into the route.

## Choose Your Starting Route

| If You Want | Start Here | Then Use |
| --- | --- | --- |
| the smallest first success | [SQL-First Day One](docs/learning/sql-first-day-one.md) | [How To Start](docs/learning/how-to-start.md), [Phase 0: Local Setup](docs/phases/phase-0-local-setup.md), then [Curriculum Map](docs/learning/curriculum-map.md) |
| the default guided route | [How To Start](docs/learning/how-to-start.md) | [Phase 0: Local Setup](docs/phases/phase-0-local-setup.md), [Curriculum Map](docs/learning/curriculum-map.md), the current lesson, then the matching practice surface |
| the shorter SQL-to-.NET route | [SQL To .NET Data Access Path](docs/learning/sql-to-dotnet-data-access-path.md) | rejoin the main route at [Lesson 10](docs/learning/10-observability-testing-and-performance-engineering.md) and [Lesson 11](docs/learning/11-capstones-and-interview-readiness.md) |
| optional DBA or DBRE specialization after the core route | [DBA And DBRE Extension Track](docs/learning/dba-dbre-extension-track.md) | keep the core route as the baseline before specializing |

## Topic-Specific Reference Routes

Use these when you already know the topic area and you do not need another learner-order summary:

- [SQL Track](docs/sql/README.md) for a SQL-only refresher or concept lookup.
- [SQL Syntax And Query Patterns](docs/sql/sql-syntax-and-query-patterns.md) for a one-page refresher on the core SQL operators, examples, and common mistakes used throughout the course.
- [EF Core Track](docs/efcore/README.md) for application-integration reference material after the SQL foundations are already familiar.
- [Performance Track](docs/performance/README.md) for measurement, Query Store, and regression-evidence deepening.
- [Operations Track](docs/operations/README.md) for readiness, release, and incident-oriented reference work.
- [Optional On-Ramps](docs/learning/curriculum-map.md#optional-on-ramps) for targeted starts that still rejoin the main route later.

## Learning Docs

The learner-oriented lesson set lives in [docs/learning](docs/learning/README.md).

Use [Curriculum Map](docs/learning/curriculum-map.md) for the authoritative sequence and lesson-to-practice mapping.

Use the [Learning Glossary](docs/learning/glossary.md) when vocabulary slows you down. Use [SQL Syntax And Query Patterns](docs/sql/sql-syntax-and-query-patterns.md) when the main question is how to write or choose a core SQL operation. Use the checkpoint section at the end of each lesson before moving on.

- [SQL-First Day One](docs/learning/sql-first-day-one.md) is the lowest-friction first-query guide.
- [SQL To .NET Data Access Path](docs/learning/sql-to-dotnet-data-access-path.md) is the shorter learner route for SQL-fluent .NET engineers who want EF Core and Dapper integration before the full ladder.
- [SQL Client Connection Guide](docs/learning/sql-client-connection-guide.md) covers Azure Data Studio, SSMS, and `sqlcmd` connection examples.
- [Local Setup Troubleshooting](docs/learning/local-setup-troubleshooting.md) narrows Docker, port, readiness, and login failures.
- [Schema Quick Reference](docs/learning/schema-quick-reference.md) is the compact first-lessons table and relationship map.
- [SQL Syntax And Query Patterns](docs/sql/sql-syntax-and-query-patterns.md) is the one-page handbook for the core SQL operators and query patterns the academy expects you to recognize.
- [Effort And Pacing Guide](docs/learning/effort-and-pacing-guide.md) summarizes the expected time budget for the current route.
- [Curriculum Map](docs/learning/curriculum-map.md) is the main learner progression map.
- [Phase Guides](docs/phases/README.md) define the objectives and exit criteria for each stage.
- [Exercise System](docs/exercises/README.md) explains the ladder, validation workflow, and exercise modes.

## Core Route Practice Snapshot

Use this snapshot when you want the default-route practice sequence from the root README. It is not the full exercise inventory.

For the full ladder, optional deepening labs, and all validation modes, use the [Exercise System](docs/exercises/README.md).

1. Lesson 01: [Beginner 000: SQL Fundamentals And Safe Changes](src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)
2. Lessons 02 and 03: [Beginner 001](src/exercises/Beginner/001-joins-and-aggregations/README.md), [Intermediate 001](src/exercises/Intermediate/001-window-functions-and-pagination/README.md), and [Intermediate 002](src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
3. Lesson 04: [Advanced 002](src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
4. Lessons 05 through 07: [Senior 001](src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md), [Advanced 001](src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md), and [Advanced 003](src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)
5. Lesson 08: [Senior 006](src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)
6. Lesson 09: [Advanced 005](src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md), [Senior 005](src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md), and [Senior 002](src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
7. Lesson 10: [Senior 004](src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) plus the focused integration and performance validation surfaces linked from the lesson and track docs
8. Lesson 11: [Senior 003](src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) plus a capstone from [Phase 9: Capstone Projects](docs/phases/phase-9-capstone-projects.md)

Optional performance deepening after [Lesson 06](docs/learning/06-indexing-execution-plans-and-parameter-sensitivity.md) or [Lesson 10](docs/learning/10-observability-testing-and-performance-engineering.md): [Advanced 004: Query Store And Regression Triage](src/exercises/Advanced/004-query-store-and-regression-triage/README.md).

## Exercise Validation

Validation-ready packs include `answer.sql` and `validation.sql` so learners can self-check quickly.

That workflow now covers Beginner 000-002, Intermediate 001-002, Advanced 001-002, Senior 001, Senior 003, and the supplemental review pack inside Senior 002.

Run the harness with:

`powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Advanced/002-staged-backfill-and-contract-enforcement`

## Subsequent Improvement Suggestions

The learner-facing summary remains in [Repository Improvement Suggestions](docs/learning/repository-improvement-suggestions.md).

The active execution-tracked maintainer backlog for route and quality follow-on work lives in [.ai/academy-enhancements-backlog.md](.ai/academy-enhancements-backlog.md), while the earlier mastery-scope history remains in [.ai/sql-efcore-mastery-backlog.md](.ai/sql-efcore-mastery-backlog.md).

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

- Expand the new specialization packs and add replication-focused drills.
- Add a dedicated migrations review pipeline with generated SQL artifacts.
- Add dashboards and alerts for blocking chains, p95 latency, and retry storms.
- Add a second API focused on ingestion and bulk-write patterns.
- Add Kubernetes manifests or Helm charts after the local platform and exercises stabilize.