# Documentation Hub

This is the documentation landing page for SqlAcademy.

If you are here to learn rather than inspect the repository structure, start with [Curriculum Map](learning/curriculum-map.md). Use this page when you need to choose the right documentation surface quickly instead of browsing the repo tree or guessing which README owns a topic.

The default learner route is built for working backend and application engineers. It is not currently a zero-assumption introduction to programming, Docker, or first-ever SQL syntax.

Do not treat this page as a second curriculum map. It is the routing layer for the docs set.

## Start Here

Use the table below when you are choosing a route, then use the rest of this page only as a docs router.

| If You Want | Start Here | Then Use |
| --- | --- | --- |
| the smallest first success | [SQL-First Day One](learning/sql-first-day-one.md) | [How To Start](learning/how-to-start.md), [Phase 0](phases/phase-0-local-setup.md), then [Curriculum Map](learning/curriculum-map.md) |
| the default guided route | [How To Start](learning/how-to-start.md) | [Curriculum Map](learning/curriculum-map.md), [Learning Docs](learning/README.md), then [Exercise System](exercises/README.md) |
| the shorter SQL-to-.NET route | [SQL To .NET Data Access Path](learning/sql-to-dotnet-data-access-path.md) | rejoin the main route at [Lesson 10](learning/10-observability-testing-and-performance-engineering.md) and [Lesson 11](learning/11-capstones-and-interview-readiness.md) |
| optional DBA or DBRE specialization after the core route | [DBA And DBRE Extension Track](learning/dba-dbre-extension-track.md) | keep [Curriculum Map](learning/curriculum-map.md) as the completed baseline before specializing |

## What Each Surface Is For

| Surface | Use It For | Best Entry Point | Do Not Use It For |
| --- | --- | --- | --- |
| learner path | ordered teaching route across lessons and phases | [Curriculum Map](learning/curriculum-map.md) | codebase architecture lookup |
| lesson docs | subject-level teaching material and checkpoints | [Learning Docs](learning/README.md) | stage-level exit criteria only |
| phase docs | milestones, outcomes, and validation expectations | [Phase Guides](phases/README.md) | detailed teaching on one SQL or EF Core topic |
| exercise docs | hands-on packs, lab modes, and validation flow | [Exercise System](exercises/README.md) | replacing the lesson sequence |
| SQL reference | SQL Server concept lookup, syntax handbook, and concrete SQL anchors | [SQL Track](sql/README.md) | LINQ or application-abstraction questions |
| EF Core reference | application-side query shape, tracking, and hybrid data access | [EF Core Track](efcore/README.md) | first-time SQL learning |
| performance reference | measurement, plans, Query Store, and regression evidence | [Performance Track](performance/README.md) | release sequencing and rollback posture |
| operations reference | bootstrap, readiness, telemetry, release safety, and incidents | [Operations Track](operations/README.md) | plan analysis without runtime context |
| architecture summary | solution layout, services, and platform boundaries | [Repository Architecture](../README.md#initial-architecture) | ordered teaching flow |

## Recommended Routes

Use one of these routes based on the question you are trying to answer.

- default learning route: [How To Start](learning/how-to-start.md), [Curriculum Map](learning/curriculum-map.md), the current lesson in [Learning Docs](learning/README.md), then the matching pack in [Exercise System](exercises/README.md)
- lowest-friction first-query route: [SQL-First Day One](learning/sql-first-day-one.md), [Schema Quick Reference](learning/schema-quick-reference.md), then [How To Start](learning/how-to-start.md)
- SQL-first route: [SQL Track](sql/README.md) or [SQL Syntax And Query Patterns](sql/sql-syntax-and-query-patterns.md), then the linked lesson or exercise, then return to [Curriculum Map](learning/curriculum-map.md)
- SQL to .NET data-access route: [SQL To .NET Data Access Path](learning/sql-to-dotnet-data-access-path.md), then the linked bridge, lesson, labs, and return point into the main route
- EF Core and application-data reference route: [EF Core Track](efcore/README.md), then the linked lesson, labs, tests, or performance surfaces
- tuning and regression route: [Performance Track](performance/README.md), then the linked performance tests, benchmarks, or advanced labs
- release and incident route: [Operations Track](operations/README.md), then the relevant phase doc, smoke surface, or investigation pack
- optional DBA or DBRE specialization route: [DBA And DBRE Extension Track](learning/dba-dbre-extension-track.md), then the linked SQL, operations, and delivery anchors without changing the default learner route

## Topic Navigation

| Topic | Use It For | Best Entry Point | Companion Surface |
| --- | --- | --- | --- |
| SQL | query design, schema, concurrency, and internals | [SQL Track](sql/README.md) | [Phase Guides](phases/README.md) |
| EF Core | DbContext, Dapper, tracking, and query-shape learning | [EF Core Track](efcore/README.md) | [From SQL To EF Core And Dapper](learning/from-sql-to-efcore-and-dapper.md) |
| Performance | execution plans, benchmarks, and tuning evidence | [Performance Track](performance/README.md) | [Lesson 10](learning/10-observability-testing-and-performance-engineering.md) |
| Operations | migration safety, telemetry, and release discipline | [Operations Track](operations/README.md) | [Phase 7](phases/phase-7-operational-engineering.md) |
| DBA or DBRE specialization | backup or restore, job safety, HA or DR, security, and change capture after the core route | [DBA And DBRE Extension Track](learning/dba-dbre-extension-track.md) | [Operations Track](operations/README.md) |
| Exercises | hands-on packs, labs, and validation modes | [Exercise System](exercises/README.md) | [Follow-Up Exercises By Level](learning/follow-up-exercises-by-level.md) |
| Phases | ordered stage progression and exit criteria | [Phase Guides](phases/README.md) | [Curriculum Map](learning/curriculum-map.md) |
| Architecture | repository structure and runtime boundaries | [Repository Architecture](../README.md#initial-architecture) | [README](../README.md) |

The architecture summary currently lives in the root [README](../README.md#initial-architecture) instead of a separate docs page.

## Fast Jumps By Job

- need the authoritative lesson order: [Curriculum Map](learning/curriculum-map.md)
- need first-run local setup: [How To Start](learning/how-to-start.md) and [Phase 0](phases/phase-0-local-setup.md)
- need the smallest first-query target before the full route: [SQL-First Day One](learning/sql-first-day-one.md) and [Schema Quick Reference](learning/schema-quick-reference.md)
- need the shorter learner route from SQL into EF Core and Dapper: [SQL To .NET Data Access Path](learning/sql-to-dotnet-data-access-path.md)
- need SQL client-specific connection help: [SQL Client Connection Guide](learning/sql-client-connection-guide.md)
- need first-run recovery steps for Docker, ports, or login failures: [Local Setup Troubleshooting](learning/local-setup-troubleshooting.md)
- need one planning view for course effort: [Effort And Pacing Guide](learning/effort-and-pacing-guide.md)
- need the stage objectives before you start working: [Phase Guides](phases/README.md)
- need a hands-on pack right now: [Exercise System](exercises/README.md)
- need a SQL-only reference path: [SQL Track](sql/README.md)
- need one-page SQL syntax examples and when-to-use guidance: [SQL Syntax And Query Patterns](sql/sql-syntax-and-query-patterns.md)
- need EF Core, Dapper, or application-side query-shape guidance: [EF Core Track](efcore/README.md)
- need tuning, Query Store, or comparative measurement: [Performance Track](performance/README.md)
- need release safety, telemetry, or incident-style debugging: [Operations Track](operations/README.md)
- need optional DBA or DBRE specialization after the core route: [DBA And DBRE Extension Track](learning/dba-dbre-extension-track.md)
- need a term defined quickly: [Learning Glossary](learning/glossary.md)

## Validation And Runtime Anchors

This repository is meant to be exercised, not only read.

- [docker-compose.yml](../docker-compose.yml) is the main local platform entry point
- [run-exercise-validation.ps1](../infra/scripts/run-exercise-validation.ps1) is the repeatable validation harness for validation-ready packs
- [SqlAcademy.UnitTests](../tests/SqlAcademy.UnitTests) provides narrow in-process contract checks
- [SqlAcademy.IntegrationTests](../tests/SqlAcademy.IntegrationTests) provides API and persistence boundary checks
- [SqlAcademy.PerformanceTests](../tests/SqlAcademy.PerformanceTests) provides comparative performance and lab-smoke validation
- [SqlAcademy.Benchmarks](../tests/SqlAcademy.Benchmarks) provides focused benchmark surfaces
- `/health/live`, `/health/ready`, and `/metrics` are the main runtime smoke surfaces exposed by the API

## Documentation Boundaries

Keep these boundaries in mind so you do not use the wrong surface for the wrong question.

- [Learning Docs](learning/README.md) teach in order
- track pages such as [SQL Track](sql/README.md), [EF Core Track](efcore/README.md), [Performance Track](performance/README.md), and [Operations Track](operations/README.md) are topic-reference surfaces
- [Phase Guides](phases/README.md) define outcomes and exit criteria
- [Exercise System](exercises/README.md) explains how the hands-on packs are organized and validated
- the root [README](../README.md) owns architecture, platform, and repository-shape explanation

## Recommended Learner Flow

1. run the stack locally
2. follow the ordered route in [Curriculum Map](learning/curriculum-map.md)
3. complete the current lesson
4. solve the linked exercise pack or guided lab
5. run the validation harness if the pack is validation-ready
6. record what you still cannot explain clearly
7. move to the next lesson only after you can defend the current one