# Senior 010: Change Capture, Provenance, And Reconciliation

## Objective

Decide whether the current data-movement posture is credible enough to support downstream replay or reconciliation by reviewing import-batch provenance, outbox-style consistency boundaries, and the minimum checks needed to prove captured work can be traced and challenged later.

## Exercise Type

This pack is an investigation pack.

Complete `investigation-template.md` as the learner-editable workbook. This pack does not use `answer.sql` or `validation.sql`.

## Scenario

An upstream trade feed is now imported through the API, and the downstream reporting team wants to rely on batch provenance and operational consistency checks instead of asking for a full CDC rollout immediately. Before that becomes your platform story, you need to inspect the current import-batch metadata, the existing outbox-style consistency contract, and the exact reconciliation proof that would show captured work can be replayed or audited without guessing.

## Repository Anchors

- [DBA And DBRE Extension Track](../../../../docs/learning/dba-dbre-extension-track.md)
- [Senior 002: EF Core, Dapper, Concurrency, And Bulk Ingestion Labs](../002-efcore-concurrency-and-bulk-ingestion/README.md)
- [Senior 003: Transactional Outbox And Delivery Consistency](../003-transactional-outbox-and-delivery-consistency/README.md)
- [Outbox Delivery Consistency Runbook](../../../../docs/operations/outbox-delivery-runbook.md)
- [TradesController import and batch endpoints](../../../apps/SqlAcademy.Api/Controllers/V1/TradesController.cs)
- [TradeImportBatch metadata migration](../../../libs/SqlAcademy.Migrations/Migrations/20260506210831_AddTradeImportBatchMetadata.cs)
- [Operations Track](../../../../docs/operations/README.md)

## Assets

- `movement-brief.md` frames the new downstream replay and audit expectation.
- `capture-surfaces.md` summarizes the current import-batch, provenance, and consistency surfaces.
- `reconciliation-gaps.md` summarizes what still blocks a credible replay or audit story.
- `investigation-template.md` is the learner-editable workbook.
- `expected-outcomes.md` defines what a strong change-capture and reconciliation answer should conclude.
- `hints.md` gives progressively more direct guidance.
- `optional-solution.md` shows one defensible platform-data-movement decision.

## Tasks

1. Decide whether the current capture and provenance posture is already credible for downstream replay or whether the workflow should be treated as insufficient without stronger reconciliation.
2. Identify one provenance fact and one consistency or replay fact that matter most to that decision.
3. Write the minimum reconciliation checklist you would require before treating captured work as auditable.
4. Name one post-movement query, batch endpoint, or operational check that proves the captured work is useful instead of only present.

## Validation

Use `investigation-template.md` as the main completion artifact.

- the learner makes an explicit credible or not-credible decision instead of only repeating generic CDC advice
- the response ties the decision to import-batch provenance, deterministic movement boundaries, and at least one reconciliation proof
- the proposed plan distinguishes traceability metadata from actual evidence that downstream work can be replayed or challenged safely

## Focused Companion Check

When you want a narrow executable check for the pack contract itself, run:

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~ChangeCaptureReconciliationDrillAssetTests"`

That asset smoke test verifies the pack keeps its provenance surfaces, reconciliation gaps, workbook prompts, and optional-solution contract aligned without pretending to automate SQL Server CDC.
