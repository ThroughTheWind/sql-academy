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
| B003 | P1 | completed | EF Core depth | add an EF Core N+1 and generated-SQL investigation lab | guided lab wired through Lesson 09, Phase 8, track and exercise indexes, and senior assessment routing; focused companion checks use `PostsEndpointTests` plus `TrackingBehaviorTests` |
| B004 | P1 | completed | data realism | add larger-cardinality seed or workload variants for tuning and plan-stability lessons | optional workload variant added under `db/performance` and wired through Lesson 06, Lesson 07, Phase 5, and the performance track without changing the default baseline seed |
| B005 | P1 | completed | senior diagnostics | add a deadlock-graph interpretation lab with retry-policy reasoning | Senior 001 now includes a supplemental deadlock graph walkthrough and sample graph, with a focused asset smoke test under `DeadlockGraphLabAssetTests` |
| B006 | P2 | completed | release engineering | add a release-readiness drill that combines migration safety, telemetry checks, and rollback gates | Senior 006 now owns the Lesson 08 and Phase 7 release-readiness route, backed by `ReleaseReadinessDrillAssetTests` |
| B007 | P2 | completed | validation | expand executable validation for guided labs where a narrow companion check is realistic | current companions now cover plan-cache, Query Store, deadlock-graph, observability-pack, release-readiness, and the existing EF Core/query-shape validation surfaces, while keeping incident reasoning itself manual |
| B008 | P3 | completed | optional specialization | add a DBA or DBRE extension track for backup and restore, Agent, HA/DR, security, and change capture | learner-facing extension track added at `docs/learning/dba-dbre-extension-track.md` and routed from the main SQL, operations, learning, and docs hub surfaces |
| B009 | P3 | completed | optional specialization follow-on | add a concrete HA or DR failover-posture pack with a narrow companion check | `src/exercises/Senior/009-ha-dr-failover-posture-and-verification` now provides the investigation pack, and `HaDrFailoverPostureDrillAssetTests` is the focused validation surface |

## Current Slice

### Completed

- B001: scope clarification and public promise cleanup
- B002: Query Store guided lab wired into the lesson route and covered by a starter-script smoke test
- B003: EF Core N+1 and generated-SQL investigation lab wired through the learner route, with focused companion checks using `PostsEndpointTests` and `TrackingBehaviorTests`
- B004: optional larger-cardinality workload variant added under `db/performance`, with lesson and track routing that keeps the default baseline seed unchanged
- B005: Senior 001 extended with a deadlock-graph walkthrough, concrete victim analysis, and retry-policy reasoning backed by `DeadlockGraphLabAssetTests`
- B006: Senior 006 added as the release-readiness investigation pack for migration gates, first-five-minutes watch plans, and rollback or roll-forward decisions
- B007: executable companion coverage now includes `PlanCacheLabSmokeTests`, `QueryStoreLabSmokeTests`, `DeadlockGraphLabAssetTests`, `PostsApiObservabilityDrillAssetTests`, and `ReleaseReadinessDrillAssetTests`, alongside the existing focused EF Core and API tests used by Senior 002 and Senior 005
- B008: learner-facing DBA and DBRE extension track added so optional platform-specialist depth is routed clearly without changing the default core route
- B009: HA or DR failover posture now has a concrete investigation pack under `src/exercises/Senior/009-ha-dr-failover-posture-and-verification`, backed by `HaDrFailoverPostureDrillAssetTests`

### Next Recommended Slice

- no immediate backlog slice remains in this track; the next clean extension candidates are security administration or change-capture work once one concrete owner surface and one narrow validation path are clear


## Acceptance Checks

1. The root and track docs must state the same target audience and the same optional-specialization boundary.
2. Every new advanced topic should map to one teaching surface, one practice surface, and one validation surface.
3. The backlog should stay execution-oriented: concrete outcomes, status, and the next recommended slice.