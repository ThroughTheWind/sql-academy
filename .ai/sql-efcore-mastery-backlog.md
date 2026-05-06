# SQL Server And EF Core Mastery Backlog

This is the maintainer execution backlog for closing the gap between the current academy route and a stronger SQL Server plus EF Core mastery target.

## Target

The core route targets senior backend and application engineers who need strong SQL Server, EF Core, Dapper, performance, and production-operability fluency.

It does not currently claim full DBA or DBRE mastery. Backup and restore, SQL Server Agent, HA/DR, replication, security administration, and similar operational specialties belong in an optional extension track after the core route is stronger.

## Status Legend

- `not-started`: planned but not yet underway
- `in-progress`: active implementation slice
- `completed`: merged into the repository
- `blocked`: waiting on a prerequisite decision or missing surface

## Backlog

| ID | Priority | Status | Area | Outcome | Notes |
| --- | --- | --- | --- | --- | --- |
| B001 | P0 | completed | scope and public promise | make the mastery target explicit and separate the core route from optional DBA specialization | implemented in `README.md`, `ACADEMY_ROADMAP.md`, `docs/sql/README.md`, and `docs/efcore/README.md` |
| B002 | P1 | completed | advanced SQL curriculum | add a Query Store and regression-triage exercise with concrete scripts and validation criteria | guided lab scaffold added under `src/exercises/Advanced/004-query-store-and-regression-triage`, wired into Lesson 06 and Lesson 07, and backed by `QueryStoreLabSmokeTests.cs` |
| B003 | P1 | in-progress | EF Core depth | add an EF Core N+1 and generated-SQL investigation lab | guided lab scaffold added under `src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation`; broader exercise wiring and any executable companion checks still pending |
| B004 | P1 | not-started | data realism | add larger-cardinality seed or workload variants for tuning and plan-stability lessons | required for stronger statistics and regression work |
| B005 | P1 | not-started | senior diagnostics | add a deadlock-graph interpretation lab with retry-policy reasoning | should extend the current concurrency pack instead of replacing it |
| B006 | P2 | not-started | release engineering | add a release-readiness drill that combines migration safety, telemetry checks, and rollback gates | belongs near operational engineering |
| B007 | P2 | not-started | validation | expand executable validation for guided labs where a narrow companion check is realistic | keep incident packs partly manual when realism matters |
| B008 | P3 | not-started | optional specialization | add a DBA or DBRE extension track for backup and restore, Agent, HA/DR, security, and change capture | only after the core route and advanced application track stabilize |

## Current Slice

### Completed

- B001: scope clarification and public promise cleanup
- B002: Query Store guided lab wired into the lesson route and covered by a starter-script smoke test

### In Progress

- B003: EF Core N+1 and generated-SQL investigation scaffold added under `src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation`

### Next Recommended Slice

- wire B003 through the remaining EF Core learner surfaces as needed
- decide whether B003 should gain a narrow executable companion check beyond the guided lab workflow
- start B004 with a larger-cardinality seed or workload variant for tuning and plan-stability lessons

## Acceptance Checks

1. The root and track docs must state the same target audience and the same optional-specialization boundary.
2. Every new advanced topic should map to one teaching surface, one practice surface, and one validation surface.
3. The backlog should stay execution-oriented: concrete outcomes, status, and the next recommended slice.