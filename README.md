# SqlAcademy

SqlAcademy is a long-lived engineering training platform for SQL Server, .NET 10, EF Core 10, Dapper, performance engineering, and production operations. It is designed to be Docker-first, exercise-driven, documentation-first, and structured for incremental learning from beginner through senior-level scenarios.

## Quick Start

1. Restore the local tool manifest: `dotnet tool restore`
2. Start the full learning stack: `docker compose up --build`
3. Open the API at `http://localhost:8080/openapi/v1.json`
4. Open Prometheus at `http://localhost:9090`
5. Open Grafana at `http://localhost:3000` with `admin` / `admin`
6. Run the fast validation slice: `dotnet test tests/SqlAcademy.UnitTests/SqlAcademy.UnitTests.csproj`

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