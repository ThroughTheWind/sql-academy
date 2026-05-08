# Senior 009: HA/DR Failover Posture And Verification

## Objective

Decide whether the current HA or DR posture is credible enough to support a planned maintenance window by reviewing recovery targets, replica lag, failover-drill freshness, and post-failover validation.

## Exercise Type

This pack is an investigation pack.

Complete `investigation-template.md` as the learner-editable workbook. This pack does not use `answer.sql` or `validation.sql`.

## Scenario

The team plans to patch the primary SQL Server host before a high-order-volume period and says the fallback is to fail over the write path to a warm secondary. Before that sentence becomes your operating plan, you need to inspect the current recovery targets, replica posture, failover-readiness evidence, and the exact post-failover checks that would prove the application can still do useful work.

## Repository Anchors

- [DBA And DBRE Extension Track](../../../../docs/learning/dba-dbre-extension-track.md)
- [Lesson 08: Operational Engineering And Release Safety](../../../../docs/learning/08-operational-engineering-and-release-safety.md)
- [Senior 006: Release Readiness And Rollback Gates](../006-release-readiness-and-rollback-gates/README.md)
- [Senior 007: Backup, Restore, And Recovery Verification](../007-backup-restore-and-recovery-verification/README.md)
- [Operations Track](../../../../docs/operations/README.md)
- [SQL Server infra notes](../../../../infra/sqlserver/README.md)

## Assets

- `failover-brief.md` frames the maintenance window and the recovery targets that matter.
- `replica-posture.md` summarizes the current failover target and the replica facts that matter.
- `failover-readiness.md` summarizes the last recorded failover rehearsal and the gaps that still matter.
- `investigation-template.md` is the learner-editable HA or DR workbook.
- `expected-outcomes.md` defines what a strong failover-posture answer should conclude.
- `hints.md` gives progressively more direct guidance.
- `optional-solution.md` shows one defensible go or no-go failover decision.

## Tasks

1. Decide whether the current HA or DR posture is credible enough for the maintenance window or should block the plan.
2. Identify one recovery-target fact and one replica or failover fact that matter most to that decision.
3. Write the minimum pre-failover checklist you would require before treating failover as credible.
4. Name one post-failover smoke path and one role-stability or recovery-time check that prove the role change is useful, not just technically possible.

## Validation

Use `investigation-template.md` as the main completion artifact.

- the learner makes an explicit go or no-go failover decision instead of only repeating generic HA slogans
- the response ties the decision to RPO or RTO evidence, replica-lag or drill-freshness evidence, and post-failover application validation
- the failover plan includes at least one useful-work smoke path and one stop condition when the recovery targets are already being missed

## Focused Companion Check

When you want a narrow executable check for the pack contract itself, run:

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~HaDrFailoverPostureDrillAssetTests"`

That asset smoke test verifies the pack keeps its recovery targets, failover-readiness gaps, workbook prompts, and optional-solution contract aligned without pretending to automate real replica infrastructure.
