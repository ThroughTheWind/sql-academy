# Exercise System

Exercises are organized by difficulty under `src/exercises`.

If you are following the curriculum in order, start with [Learning Docs](../learning/README.md) and return here when you need the exercise map or validation workflow.

Every exercise folder includes:
- a narrative README
- starter material
- an intentionally broken query, script, or investigation surface
- expected outcomes
- hints
- an optional solution file

Exercises currently ship in three supported modes:

- `validation pack`: includes `starter.sql`, `answer.sql`, and `validation.sql` so the learner can self-check with the harness.
- `guided lab`: includes starter assets and explicit completion criteria, but the learner validates the work manually against the README and expected outcomes.
- `investigation pack`: includes evidence snapshots or workbooks and expects a written analysis instead of a single SQL answer file.

Design rules:
- exercises should validate a specific learning objective
- broken states should resemble real engineering mistakes
- learners should be able to verify results locally against `LearningDb`
- hints should help without fully solving the exercise too early

## Exercise Ladder

| Pack | Mode | Primary Focus |
| --- | --- | --- |
| [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) | validation pack | SQL fundamentals and safe changes |
| [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md) | validation pack | joins and aggregations |
| [Beginner 002](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md) | validation pack | filtering, constraints, and data quality |
| [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md) | validation pack | window functions and stable pagination |
| [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md) | validation pack | cohort analysis and pagination drift |
| [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) | validation pack | indexing, parameter sensitivity, and rollout safety |
| [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md) | validation pack | staged backfill and contract enforcement |
| [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) | guided lab | plan cache, memory grants, and wait signals |
| [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) | validation pack | concurrency, blocking, deadlocks, and deadlock-graph interpretation |
| [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) | guided lab | runnable EF Core, Dapper, and rowversion labs plus a supplemental ingestion review pack |
| [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) | validation pack | transactional outbox and delivery consistency |
| [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | investigation pack | posts API latency and observability triage |
| [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) | guided lab | EF Core generated-SQL inspection and disposable N+1 regression investigation |
| [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) | investigation pack | release readiness, migration safety, telemetry checks, and rollback gates |
| [Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md) | investigation pack | backup posture, restore verification, and recovery proof |
| [Senior 008](../../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md) | investigation pack | SQL Server Agent placement, job safety, and scheduling boundaries |
| [Senior 009](../../src/exercises/Senior/009-ha-dr-failover-posture-and-verification/README.md) | investigation pack | HA or DR posture, failover readiness, and recovery-target verification |
| [Senior 010](../../src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md) | investigation pack | change capture provenance, reconciliation, and replay-proof reasoning |
| [Senior 011](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md) | investigation pack | least privilege, migration authority, and operational security boundaries |

## Validation Workflow For Validation Packs

1. Edit the exercise `answer.sql` file if the pack includes one.
2. Start the Docker stack so SQL Server is available.
3. Run `infra/scripts/run-exercise-validation.ps1 -Exercise <relative-exercise-path>`.
4. Fix the answer until the validation harness returns success.

## Guided Lab Workflow

1. Read the exercise README and identify the repository anchors you need to inspect.
2. Work through the starter material and broken example before looking at the optional solution.
3. Use `expected-outcomes.md` and the README validation criteria as the completion contract.
4. If the guided lab also ships a supplemental validation pack, run it after the code lab work to confirm the review tables or workbook outputs.

Guided labs such as [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) do not ship with `answer.sql` plus `validation.sql`.

[Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) is the main exception: it is now a guided lab bundle for Phase 8, but it keeps `answer.sql` and `validation.sql` as a supplemental review pack after the code labs.

[Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) remains a validation pack, but it now also includes a supplemental deadlock-graph walkthrough so the learner can connect the validated wait cycle to a concrete victim and retry-policy discussion.

[Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) is the focused guided-lab follow-up when you want explicit generated-SQL inspection and a disposable N+1 experiment around the posts read path.

## Investigation Pack Workflow

1. Read the incident or scenario brief.
2. Fill in the workbook or written response template.
3. Compare the write-up with `expected-outcomes.md` and the optional solution.

Investigation packs such as [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) use Markdown evidence snapshots and narrative output instead of a single SQL answer file.

[Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) is the release-readiness investigation pack when you need a go or no-go decision, a first-five-minutes watch plan, and explicit rollback or roll-forward gates before deployment begins.

[Senior 007](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md) is the optional DBA or DBRE recovery-verification investigation pack when backup posture, restore drills, and post-restore proof need to be defended from evidence instead of assumptions.

[Senior 008](../../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md) is the optional DBA or DBRE scheduling investigation pack when SQL Server Agent placement, overlap control, and job-safety rules need to be defended from the current worker and release boundaries.

[Senior 009](../../src/exercises/Senior/009-ha-dr-failover-posture-and-verification/README.md) is the optional DBA or DBRE HA or DR investigation pack when failover posture, replica lag, and post-failover useful-work checks need to be defended from recovery targets instead of assumptions.

[Senior 010](../../src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md) is the optional DBA or DBRE change-capture investigation pack when provenance metadata, replay boundaries, and reconciliation proof need to be defended from the existing import and outbox surfaces instead of generic CDC slogans.

[Senior 011](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md) is the optional DBA or DBRE security investigation pack when `sa` convenience access, startup migration authority, and operator-admin boundaries need to be defended from the current runtime and deployment surfaces instead of generic least-privilege slogans.