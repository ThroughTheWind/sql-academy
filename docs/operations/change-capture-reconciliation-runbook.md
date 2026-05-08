# Change Capture And Reconciliation Runbook

Use this as a printable checklist when imported data needs to be traced, challenged, or replayed without pretending that provenance metadata alone makes the workflow audit-ready.

This runbook is a companion to [Senior 010: Change Capture, Provenance, And Reconciliation](../../src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md), [Senior 003: Transactional Outbox And Delivery Consistency](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md), and [Operations Track](README.md).

It is not a replacement for the investigation pack. It is the compressed checklist for separating traceability from reconciliation proof before you claim an imported batch is replay-safe or audit-ready.

## First Check

- name the one disputed batch, replay request, or missing downstream outcome you are trying to explain
- decide whether the problem is about provenance, reconciliation, or an already duplicated replay path
- keep one concrete batch identifier in scope before browsing broad history tables or logs

## Provenance Boundary

- confirm the batch has queryable `Source` and `CorrelationId` values before you treat it as traceable
- use `import-batches` and batch-row history first so you know exactly which rows are being challenged
- do not treat metadata columns as proof that downstream work happened correctly

## Reconciliation Proof

- prove one imported batch maps to one reviewable downstream outcome or one explicit missing-work result
- name the one deterministic reconciliation query or consistency check that can falsify your current assumption
- treat skipped, duplicated, or ambiguous movement as a contract failure instead of as harmless operational noise

## Containment And Replay Decision

- stop widening replays or downstream retries until the disputed batch boundary is clear
- decide whether the safer path is targeted replay, explicit reconciliation, or pausing the consuming workflow
- do not call the workflow audit-ready if the operator still has to guess which rows moved or were reviewed

## Follow-Up Validation

- rerun the same reconciliation proof after the fix instead of declaring success from metadata alone
- record the disputed batch, the proving query or endpoint, and the replay decision while the facts are still narrow
- keep [Senior 010: Change Capture, Provenance, And Reconciliation](../../src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md) nearby when the argument is really about replay boundaries rather than only transport mechanics

## Drill Links

- [Senior 010: Change Capture, Provenance, And Reconciliation](../../src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md)
- [Senior 003: Transactional Outbox And Delivery Consistency](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Operations Track](README.md)
