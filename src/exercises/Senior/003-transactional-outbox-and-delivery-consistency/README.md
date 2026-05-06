# Senior 003: Transactional Outbox And Delivery Consistency

## Objective

Practice an idempotent outbox pattern that keeps order-state changes and message dispatch preparation aligned.

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