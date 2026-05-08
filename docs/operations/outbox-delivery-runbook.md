# Outbox Delivery Consistency Runbook

Use this as a printable checklist when order-state changes and downstream delivery preparation must stay aligned without duplicate dispatch.

This runbook is a companion to [Senior 003: Transactional Outbox And Delivery Consistency](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) and the operational consistency material already routed through [Lesson 08: Operational Engineering And Release Safety](../learning/08-operational-engineering-and-release-safety.md).

It is not a replacement for the pack. It is the compressed checklist for proving that ready work has one pending outbox message, deterministic dispatch order, and an explicit consistency check.

## First Check

- identify the ready-to-ship or ready-to-process set you believe should have pending outbox messages
- separate missing messages from duplicate messages before changing any dispatch logic
- confirm whether the failure is about enqueueing, dispatch batching, or post-dispatch reconciliation

## Idempotent Insert Contract

- insert pending outbox messages only for ready rows that do not already have one
- treat duplicate creation as a contract failure, not as harmless noise
- keep the insert logic aligned with the exact ready-state predicate you mean to serve

## Dispatch Batch Discipline

- choose the next dispatch batch in deterministic order rather than from an undefined scan
- keep the batch small enough that you can reason about exactly which messages should move next
- do not mark messages as delivered until the dispatch boundary is explicit and reviewable

## Consistency Proof

- prove that every ready row has exactly one pending outbox message when it should
- prove that rows with an existing pending message do not receive duplicates
- keep the consistency check query close enough to the source data that it can challenge your assumptions directly

## Containment And Follow-Up Validation

- if delivery is drifting, stop the duplication path before widening throughput or retries
- rerun the same consistency proof after the fix instead of declaring success from code review alone
- record the deterministic ordering rule and the no-duplicates rule as part of the operational contract

## Drill Links

- [Senior 003: Transactional Outbox And Delivery Consistency](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Release Runbook](release-runbook.md)
- [Operations Track](README.md)
