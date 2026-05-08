# Beginner 000: SQL Fundamentals And Safe Changes

## Objective

Build confidence with projection, filtering, deterministic ordering, and constraint-aware reasoning before later lessons introduce joins, windows, or performance tuning.

## Before You Start

- complete [Phase 0: Local Setup](../../../../docs/phases/phase-0-local-setup.md)
- read [Lesson 01: SQL Fundamentals](../../../../docs/learning/01-sql-fundamentals.md)
- keep [Learning Glossary](../../../../docs/learning/glossary.md) nearby if terms such as projection, predicate, or deterministic order are still slow

## Suggested Workflow

1. Run `starter.sql` unchanged and inspect the staging rows before you edit `answer.sql`.
2. Read `expected-outcomes.md` so you know what proof the validation script expects.
3. Solve one temp-table task at a time instead of trying to finish all three in one pass.
4. Open `hints.md` only after you can explain what your current query is doing wrong.

## Sample Sanity Checks

Your answer should line up with these sample facts before you run the harness.

`#lesson01_user_directory` should begin like this:

| DisplayOrder | UserName | Email |
| --- | --- | --- |
| 1 | `ada` | `ada@sqlacademy.local` |
| 2 | `grace` | `grace@sqlacademy.local` |
| 3 | `linus` | `linus@sqlacademy.local` |
| 4 | `margaret` | `margaret@sqlacademy.local` |

`#lesson01_recent_actionable_orders` should contain only these two rows in this review order:

| ReviewRank | OrderNumber | Status | TotalAmount |
| --- | --- | --- | --- |
| 1 | `ORD-2025-0003` | `Pending` | `143.00` |
| 2 | `ORD-2025-0002` | `Submitted` | `980.10` |

`#lesson01_candidate_user_decisions` should classify the staging rows like this:

| CandidateRowId | DecisionCode |
| --- | --- |
| 1 | `ReadyToInsert` |
| 2 | `DuplicateUserName` |
| 3 | `DuplicateEmail` |
| 4 | `MissingUserName` |
| 5 | `MissingEmail` |

## If You Get Stuck

- go back to the read-only inspection queries in [Lesson 01](../../../../docs/learning/01-sql-fundamentals.md) if table purpose or row ordering still feels unclear
- use `broken.sql` to name the failure before you try to repair it
- do not open `optional-solution.sql` until you already have a concrete opinion about the fix

## Scenario

You are reviewing the seeded academy data and a small candidate-user import before anything is allowed to touch production tables.

## Assets

- `starter.sql` sets up the candidate-user staging fixture.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the expected result tables in one SQL session.
- `broken.sql` shows the kinds of mistakes this exercise is designed to catch.
- `expected-outcomes.md` describes the required end state.
- `hints.md` offers progressively more direct guidance.
- `optional-solution.sql` shows one valid answer.

## Tasks

1. Create `#lesson01_user_directory` with `DisplayOrder`, `UserId`, `UserName`, `Email`, and `CreatedUtc` for every user ordered by `CreatedUtc`, `Id`.
2. Create `#lesson01_recent_actionable_orders` with `ReviewRank`, `OrderNumber`, `Status`, `TotalAmount`, and `UpdatedUtc` for the two most recently updated orders that are not already filled.
3. Create `#lesson01_candidate_user_decisions` with `CandidateRowId` and `DecisionCode` so each staged row is marked as `ReadyToInsert`, `MissingUserName`, `MissingEmail`, `DuplicateUserName`, or `DuplicateEmail`.

## Validation

- the user directory uses explicit projection and deterministic ordering
- the order review slice filters before taking the top two rows
- staged user decisions respect missing values and existing uniqueness rules