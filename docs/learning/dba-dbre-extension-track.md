# DBA And DBRE Extension Track

This is the optional specialization route for learners who already finished the core academy path and want deeper database-platform coverage beyond the application-engineering focus.

It is not part of the default route in [Curriculum Map](curriculum-map.md).

## Entry Bar

Use this track only after the core route feels stable enough that specialization will not hide a core weakness in schema safety, observability, or release reasoning.

You are ready when the following evidence feels routine rather than aspirational:

- [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
- [Lesson 08: Operational Engineering And Release Safety](08-operational-engineering-and-release-safety.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md)
- [Senior 003: Transactional Outbox And Delivery Consistency](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)

You should also be able to do all of the following without leaning on generic slogans:

- explain one migration gate, one telemetry gate, and one rollback or roll-forward trigger
- choose a narrow validation surface before making an operational claim loudly
- explain why useful-work checks matter beyond container or process health

## Time Planning

- one selected specialization pack: 60 to 120 minutes
- one two-pack branch such as recovery plus failover or security plus row-level security: 3 to 5 hours
- a broad pass across the current specialization menu: 8 to 14 hours

Treat this as a menu, not a second mandatory curriculum. Pick the gap that matches your real work.

## Why This Track Is Separate

The core academy targets backend and application engineers who need strong SQL Server fluency for schema design, query shape, performance, concurrency, EF Core, Dapper, and production operations.

DBA and DBRE work extends beyond that into recovery objectives, scheduled operations, failover posture, security boundaries, row-level isolation policy, and change-data movement at the platform level.

Those topics deserve their own route so the main learner path does not get diluted or front-loaded with infrastructure specialization too early.

## Specialization Menu

| If Your Gap Is | Start Here | Proof You Are Ready | Typical Focused Time | Current Practice Surface |
| --- | --- | --- | --- |
| backup, restore, and recovery verification | recovery objectives, restore drills, and post-restore useful-work proof | you can already explain release gates and post-release validation from [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) | 60 to 120 minutes | [Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md) backed by [BackupRestoreRecoveryDrillAssetTests](../../tests/SqlAcademy.PerformanceTests/BackupRestoreRecoveryDrillTests.cs) |
| scheduling, jobs, and operational automation | what belongs in SQL Server Agent versus worker or operator-controlled flows | you can already defend idempotency and useful-work validation from [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) | 60 to 120 minutes | [Senior 008](../../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md) backed by [SqlServerAgentSchedulingDrillAssetTests](../../tests/SqlAcademy.PerformanceTests/SqlServerAgentSchedulingDrillTests.cs) |
| HA, DR, and failover posture | RPO or RTO reasoning, failover credibility, and post-failover verification | you can already explain rollback versus roll-forward gates and watch plans from [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) | 60 to 120 minutes | [Senior 009](../../src/exercises/Senior/009-ha-dr-failover-posture-and-verification/README.md) backed by [HaDrFailoverPostureDrillAssetTests](../../tests/SqlAcademy.PerformanceTests/HaDrFailoverPostureDrillTests.cs) |
| security boundaries and tenant isolation | least privilege, separation of duties, policy filters, and explicit bypass boundaries | you can already defend schema contracts and request-to-data boundaries from [Lesson 04](04-schema-design-and-migration-safety.md) and [Lesson 09](09-ef-core-dapper-and-query-shape.md) | 2 to 4 hours | [Senior 011](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md) backed by [LeastPrivilegeSecurityBoundaryDrillAssetTests](../../tests/SqlAcademy.PerformanceTests/LeastPrivilegeSecurityBoundaryDrillTests.cs), then [Senior 012](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md) backed by [RowLevelSecurityTenantIsolationDrillAssetTests](../../tests/SqlAcademy.PerformanceTests/RowLevelSecurityTenantIsolationDrillTests.cs) |
| change capture and platform data movement | provenance, replay boundaries, reconciliation, and downstream trust | you can already explain idempotent delivery and staged data movement from [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) and [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) | 60 to 120 minutes | [Senior 010](../../src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md) backed by [ChangeCaptureReconciliationDrillAssetTests](../../tests/SqlAcademy.PerformanceTests/ChangeCaptureReconciliationDrillTests.cs) |

## Suggested Branches

- recovery and resilience branch: [Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md), then [Senior 009](../../src/exercises/Senior/009-ha-dr-failover-posture-and-verification/README.md)
- operations automation branch: [Senior 008](../../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md), then [Senior 010](../../src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md)
- security and isolation branch: [Senior 011](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md), then [Senior 012](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md)

## How To Use This Track

1. Keep the core learner route as the baseline.
2. Pick one branch or one module based on your actual gap, not on fear of missing out.
3. Use the listed repo anchors to build vocabulary, operational checklists, and decision frames.
4. Return to the core route if the extension topic starts exposing a weakness in schema safety, observability, or concurrency reasoning.

## What This Track Does Not Do Yet

This extension track now exists as a routed specialization guide and now has one concrete hands-on pack for each currently listed specialization module.

Backup and restore now has [Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md), SQL Server Agent or job-safety work now has [Senior 008](../../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md), HA or DR posture now has [Senior 009](../../src/exercises/Senior/009-ha-dr-failover-posture-and-verification/README.md), change capture or platform data movement now has [Senior 010](../../src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md), and security work now has [Row-Level Security And Tenant Isolation](row-level-security-and-tenant-isolation.md) plus [Senior 011](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md) and [Senior 012](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md), but each module is still an initial specialization surface rather than complete DBA or DBRE coverage.
