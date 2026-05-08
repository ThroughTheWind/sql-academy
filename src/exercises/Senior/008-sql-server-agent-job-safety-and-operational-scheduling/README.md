# Senior 008: SQL Server Agent, Job Safety, And Operational Scheduling

## Objective

Decide which recurring operational work belongs in SQL Server Agent, which should stay in `SqlAcademy.Worker`, and which should remain an explicit release-step or operator workflow instead of a blind schedule.

## Exercise Type

This pack is an investigation pack.

Complete `investigation-template.md` as the learner-editable workbook. This pack does not use `answer.sql` or `validation.sql`.

## Scenario

The team wants to push more recurring operational work into SQL Server Agent before the next release window. Three candidate tasks are on the table, but their execution boundaries, overlap risk, and observability requirements are different enough that a careless scheduling decision could hide failures or duplicate work.

## Repository Anchors

- [DBA And DBRE Extension Track](../../../../docs/learning/dba-dbre-extension-track.md)
- [Repository Architecture](../../../../README.md#initial-architecture)
- [SqlAcademy.Worker startup](../../../apps/SqlAcademy.Worker/Program.cs)
- [DatabaseTelemetryWorker](../../../apps/SqlAcademy.Worker/DatabaseTelemetryWorker.cs)
- [SqlAcademy.Worker appsettings](../../../apps/SqlAcademy.Worker/appsettings.json)
- [Senior 003: Transactional Outbox And Delivery Consistency](../003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 006: Release Readiness And Rollback Gates](../006-release-readiness-and-rollback-gates/README.md)
- [Operations Track](../../../../docs/operations/README.md)

## Assets

- `scheduling-brief.md` frames the proposed move toward SQL Server Agent.
- `job-catalog.md` summarizes the candidate workloads and the questions they raise.
- `worker-notes.md` summarizes the current worker-host behavior that already exists in the repo.
- `investigation-template.md` is the learner-editable scheduling workbook.
- `expected-outcomes.md` defines what a strong scheduling and job-safety answer should conclude.
- `hints.md` gives progressively more direct guidance.
- `optional-solution.md` shows one defensible placement and job-safety plan.

## Tasks

1. Decide which candidate workload belongs in SQL Server Agent, which should stay in `SqlAcademy.Worker`, and which should stay an explicit release-step or operator-controlled workflow.
2. Name one overlap, retry, or duplication risk for the workload you would schedule.
3. Define the minimum job-safety rules for concurrency, failure signaling, and observability.
4. Name one post-run validation check that proves the scheduled work did what it was supposed to do.

## Validation

Use `investigation-template.md` as the main completion artifact.

- the learner chooses at least one workload to keep in `SqlAcademy.Worker` and explains why
- the response distinguishes database-local scheduled work from application-bound orchestration or release gating
- the scheduled-job rule set includes overlap control, failure visibility, and one post-run validation check

## Focused Companion Check

When you want a narrow executable check for the pack contract itself, run:

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~SqlServerAgentSchedulingDrillAssetTests"`

That asset smoke test verifies the pack keeps its candidate workloads, worker-boundary notes, job-safety prompts, and optional-solution contract aligned without pretending to automate SQL Server Agent itself.
