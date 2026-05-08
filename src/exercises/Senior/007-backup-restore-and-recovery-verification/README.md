# Senior 007: Backup, Restore, And Recovery Verification

## Objective

Decide whether the current backup and restore posture is credible enough to support a schema-affecting release by reviewing recovery targets, backup-chain evidence, restore-drill freshness, and post-restore validation.

## Exercise Type

This pack is an investigation pack.

Complete `investigation-template.md` as the learner-editable workbook. This pack does not use `answer.sql` or `validation.sql`.

## Scenario

The release plan says the fallback is to restore `LearningDb` if the deployment goes wrong. Before you treat that sentence as a real safety net, you need to inspect the current backup inventory, the most recent restore verification evidence, and the exact post-restore checks that would prove useful work still functions.

## Repository Anchors

- [DBA And DBRE Extension Track](../../../../docs/learning/dba-dbre-extension-track.md)
- [Lesson 08: Operational Engineering And Release Safety](../../../../docs/learning/08-operational-engineering-and-release-safety.md)
- [Senior 006: Release Readiness And Rollback Gates](../006-release-readiness-and-rollback-gates/README.md)
- [SQL Server infra notes](../../../../infra/sqlserver/README.md)
- [Operations Track](../../../../docs/operations/README.md)

## Assets

- `recovery-brief.md` frames the release window and the recovery targets that matter.
- `backup-inventory.md` summarizes the current full, differential, and log backup evidence.
- `restore-verification.md` summarizes the last restore drill and the gaps that still matter.
- `investigation-template.md` is the learner-editable recovery workbook.
- `expected-outcomes.md` defines what a strong recovery-verification answer should conclude.
- `hints.md` gives progressively more direct guidance.
- `optional-solution.md` shows one defensible release-blocking decision and recovery-verification plan.

## Tasks

1. Decide whether the current recovery posture is credible enough for the release window or should block the change.
2. Identify one backup-chain fact and one restore-verification fact that matter most to that decision.
3. Write the minimum restore drill sequence you would require before trusting restore as a fallback.
4. Name one post-restore smoke path and one integrity check that prove the restore is more than a file-level success.

## Validation

Use `investigation-template.md` as the main completion artifact.

- the learner makes an explicit go or no-go recovery decision instead of only repeating generic backup advice
- the response ties the decision to RPO or RTO evidence, backup-chain evidence, and restore-drill freshness
- the restore drill includes a concrete restore sequence plus at least one post-restore smoke or integrity check

## Focused Companion Check

When you want a narrow executable check for the pack contract itself, run:

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~BackupRestoreRecoveryDrillAssetTests"`

That asset smoke test verifies the pack keeps its recovery targets, backup-chain evidence, restore-verification prompts, and optional-solution contract aligned without pretending to automate a real restore workflow.
