# Advanced 002: Staged Backfill And Contract Enforcement

## Objective

Practice a safe contract-evolution pattern by backfilling missing values, batching the work deterministically, and proving the data is ready for enforcement.

## Before You Start

- read [Lesson 04: Schema Design And Migration Safety](../../../../docs/learning/04-schema-design-and-migration-safety.md)
- keep [Migration strategy notes](../../../../db/migrations/README.md) nearby if rollout sequencing still feels less clear than the SQL itself
- inspect [Order EF mapping](../../../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs) if you want one concrete reminder of how schema intent, constraints, and ORM mapping stay aligned in this repository

## Suggested Workflow

1. Run `starter.sql` unchanged and inspect which rows actually need backfill before you edit `answer.sql`.
2. Read `expected-outcomes.md` so you know the exact contract the validation harness will verify.
3. Backfill only the missing values first and confirm you did not overwrite the existing external reference.
4. Build the deterministic batch plan only after the repaired data looks correct.
5. Create the enforcement check last so you prove the table is ready for a later `NOT NULL` step.

## If You Get Stuck

- go back to the add, backfill, enforce sequence in [Lesson 04](../../../../docs/learning/04-schema-design-and-migration-safety.md) before you change the SQL again
- separate the data repair question from the enforcement-readiness question and solve them one at a time
- use `broken.sql` to name whether the current mistake is overwriting existing values, batching the wrong rows, or proving the contract incorrectly before you open `optional-solution.sql`

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

## Sample Output Cues

You do not need byte-for-byte formatting, but your answer should leave the validation surfaces showing these shapes:

### Backfilled Order Target

```text
Id  ExternalReference
--  ---------------------
1   LEGACY-ORD-2025-0001
2   EXT-2025-0002
3   LEGACY-ORD-2025-0003
4   LEGACY-ORD-2025-0004
```

### `#backfill_batches`

```text
BatchNumber  OrderId  ExternalReference
-----------  -------  ---------------------
1            1        LEGACY-ORD-2025-0001
1            3        LEGACY-ORD-2025-0003
2            4        LEGACY-ORD-2025-0004
```

### `#contract_enforcement_check`

```text
NullExternalReferenceCount  DuplicateExternalReferenceCount  CanEnforceNotNull
--------------------------  -------------------------------  -----------------
0                           0                                1
```