# Advanced 002: Staged Backfill And Contract Enforcement

## Objective

Practice a safe contract-evolution pattern by backfilling missing values, batching the work deterministically, and proving the data is ready for enforcement.

## Scenario

An order table needs a new external reference contract. Existing rows are inconsistent, and you need to prove you can migrate them safely before you enforce anything stricter.

## Assets

- `starter.sql` creates the migration fixture.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the staged backfill answer.
- `broken.sql` shows unsafe migration instincts.
- `expected-outcomes.md` defines the required end state.
- `hints.md` offers guidance.
- `optional-solution.sql` shows one valid rollout.

## Tasks

1. Backfill missing `ExternalReference` values without overwriting values that already exist.
2. Create `#backfill_batches` that groups only the backfilled rows into deterministic batches of two.
3. Create `#contract_enforcement_check` to prove the contract is ready for a later `NOT NULL` enforcement step.

## Validation

- only missing contract values are backfilled
- batching is deterministic and limited to rows that needed backfill
- the final check proves there are no null or duplicate external references