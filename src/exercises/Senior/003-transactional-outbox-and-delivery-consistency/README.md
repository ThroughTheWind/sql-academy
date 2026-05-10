# Senior 003: Transactional Outbox And Delivery Consistency

## Objective

Practice an idempotent outbox pattern that keeps order-state changes and message dispatch preparation aligned.

## Before You Start

- read [Lesson 11: Capstones And Interview Readiness](../../../../docs/learning/11-capstones-and-interview-readiness.md) if you are using this pack as a synthesis drill instead of as an isolated SQL exercise
- keep [Outbox Delivery Consistency Runbook](../../../../docs/operations/outbox-delivery-runbook.md) nearby if you want the same contract compressed into an operational checklist after the SQL work
- expect to separate three concerns clearly: idempotent insert, deterministic dispatch order, and final consistency proof

## Suggested Workflow

1. Run `starter.sql` unchanged and identify which ready-to-ship orders already have pending outbox rows versus which ones still need one.
2. Write the idempotent insert first so you can explain why order 4 must not receive a duplicate message.
3. Build `#dispatch_batch` only after the pending set is correct, so dispatch ordering does not hide an earlier insert mistake.
4. Finish `#delivery_consistency_check` last and use it to prove both completeness and duplicate avoidance.
5. Run `validation.sql` only after you can explain the contract in plain language without reading the hints.

## If You Get Stuck

- separate missing-message bugs from duplicate-message bugs before you change the insert logic
- keep the dispatch ordering deterministic with an explicit earliest-first rule rather than assuming current row order is safe
- use `broken.sql` to name whether the current defect is idempotency, dispatch selection, or final consistency proof before you open `optional-solution.sql`

## Scenario

Orders are ready to ship, but downstream delivery depends on outbox messages being created exactly once and dispatched in deterministic batches.

## Assets

- `starter.sql` creates the outbox and order fixtures.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies delivery consistency assumptions.
- `broken.sql` shows unsafe outbox behavior.
- `expected-outcomes.md` defines the required result.
- `hints.md` offers guidance.
- `optional-solution.sql` shows one valid answer.

## Tasks

1. Insert pending outbox messages for ready-to-ship orders that do not already have one.
2. Create `#dispatch_batch` for the next two messages in deterministic dispatch order.
3. Create `#delivery_consistency_check` to prove no ready order is missing a pending message and no duplicates were introduced.

## Validation

- idempotent outbox inserts avoid duplicates
- dispatch order is deterministic
- the consistency check proves the outbox matches the ready-to-ship order set

Use [Outbox Delivery Consistency Runbook](../../../../docs/operations/outbox-delivery-runbook.md) when you want the same delivery-consistency contract compressed into a printable operational checklist.

## Focused Companion Check

When you want the executable completion check for the SQL pack itself, run:

`powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Senior/003-transactional-outbox-and-delivery-consistency`

That validation script checks the idempotent insert, deterministic dispatch batch, and final consistency proof in one SQL session.