# DBA And DBRE Extension Track

This is the optional specialization route for learners who already finished the core academy path and want deeper database-platform coverage beyond the application-engineering focus.

It is not part of the default route in [Curriculum Map](curriculum-map.md).

## When To Start

Use this track only after the core route feels stable for you.

A practical entry bar is:

- [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
- [Lesson 08: Operational Engineering And Release Safety](08-operational-engineering-and-release-safety.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md)
- [Senior 003: Transactional Outbox And Delivery Consistency](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)

## Why This Track Is Separate

The core academy targets backend and application engineers who need strong SQL Server fluency for schema design, query shape, performance, concurrency, EF Core, Dapper, and production operations.

DBA and DBRE work extends beyond that into recovery objectives, scheduled operations, failover posture, security boundaries, and change-data movement at the platform level.

Those topics deserve their own route so the main learner path does not get diluted or front-loaded with infrastructure specialization too early.

## Module Map

| Module | What To Learn | Current Repo Anchors | Current Practice Surface |
| --- | --- | --- | --- |
| backup, restore, and recovery verification | recovery objectives, backup validation, restore drills, and what makes rollback or restore realistic | [Lesson 08](08-operational-engineering-and-release-safety.md), [Operations Track](../operations/README.md), [SQL Server infra notes](../../infra/sqlserver/README.md) | [Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md) backed by [BackupRestoreRecoveryDrillAssetTests](../../tests/SqlAcademy.PerformanceTests/BackupRestoreRecoveryDrillTests.cs) |
| scheduling, jobs, and operational automation | what belongs in scheduled SQL work, how to reason about job safety, and how background processing changes operational risk | [Repository Architecture](../../README.md#initial-architecture), [Operations Track](../operations/README.md), [SqlAcademy.Worker](../../src/apps/SqlAcademy.Worker) | [Senior 008](../../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md) backed by [SqlServerAgentSchedulingDrillAssetTests](../../tests/SqlAcademy.PerformanceTests/SqlServerAgentSchedulingDrillTests.cs) |
| HA, DR, and failover posture | RPO or RTO thinking, failover decision frames, and why recovery plans must be tested rather than assumed | [Lesson 08](08-operational-engineering-and-release-safety.md), [Operations Track](../operations/README.md), [Repository Improvement Suggestions](repository-improvement-suggestions.md) | currently reference-driven; the release-readiness drill is the closest current decision surface |
| security and access boundaries | least privilege, operational separation of duties, and how schema, runtime, and observability choices affect exposure | [Lesson 04](04-schema-design-and-migration-safety.md), [SQL Track](../sql/README.md), [Operations Track](../operations/README.md) | currently reference-driven; use schema and release reviews to practice risk identification |
| change capture and platform data movement | CDC-style thinking, operational data movement, reconciliation, and downstream delivery safety | [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md), [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md), [Operations Track](../operations/README.md) | [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) is the nearest current exercise surface |

## How To Use This Track

1. Keep the core learner route as the baseline.
2. Pick one specialization module based on your actual gap.
3. Use the listed repo anchors to build vocabulary, operational checklists, and decision frames.
4. Return to the core route if the extension topic starts exposing a weakness in schema safety, observability, or concurrency reasoning.

## What This Track Does Not Do Yet

This extension track now exists as a routed specialization guide, but it does not yet claim fully built hands-on packs for HA or DR, security administration, or CDC.

Backup and restore now has an initial recovery-verification investigation pack in [Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md), and SQL Server Agent or job-safety work now has [Senior 008](../../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md), but the remaining specialization modules can still expand later without changing the main academy route.
