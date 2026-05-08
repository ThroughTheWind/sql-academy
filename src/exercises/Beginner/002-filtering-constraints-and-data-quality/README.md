# Beginner 002: Filtering, Constraints, And Data Quality

## Objective

Practice safe filtering, basic data-quality triage, and constraint-aware reasoning without jumping to schema changes too early.

## Before You Start

- complete [Beginner 000](../000-sql-fundamentals-and-safe-changes/README.md)
- be comfortable filtering ordered result sets and explaining basic uniqueness or required-value failures
- keep [Learning Glossary](../../../../docs/learning/glossary.md) nearby if terms such as predicate, selectivity, or constraint enforcement are still slow

## Suggested Workflow

1. Run `starter.sql` unchanged and inspect the staged import rows before you edit `answer.sql`.
2. Solve `#recent_high_value_orders` first, then duplicate-email detection, then rejected-row triage.
3. Use small read-only checks to prove the filter or normalization logic before you create the temp table output.
4. Open `hints.md` only after you can explain which rows are still being included or excluded incorrectly.

## Sample Sanity Checks

Your answer should line up with these sample facts before you run the harness.

`#recent_high_value_orders` should contain only these rows:

| OrderNumber | UserName | TotalAmount |
| --- | --- | --- |
| `ORD-2025-0001` | `ada` | `512.40` |
| `ORD-2025-0002` | `grace` | `980.10` |

`#duplicate_import_emails` should normalize and count duplicates like this:

| NormalizedEmail | DuplicateCount |
| --- | --- |
| `ada@sqlacademy.local` | 2 |
| `margaret@sqlacademy.local` | 2 |

`#rejected_import_rows` should contain these rows:

| RowId | ReasonCode |
| --- | --- |
| 3 | `MissingEmail` |
| 4 | `MissingUserName` |

## If You Get Stuck

- slow down and prove each filter with a read-only `SELECT` before you build the final temp table
- normalize the staged email values in a scratch query before you try to detect duplicates
- use `broken.sql` and `expected-outcomes.md` to name the mistake before you open `optional-solution.sql`

## Scenario

You have a small staging import with duplicates and invalid rows, and you need to produce a clean filtered output before the data is allowed anywhere near a production table.

## Assets

- `starter.sql` sets up the working fixtures for the exercise.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the answer in the same SQL session.
- `broken.sql` shows the kinds of mistakes this exercise is designed to catch.
- `expected-outcomes.md` describes the required end state.
- `hints.md` offers progressively more direct guidance.
- `optional-solution.sql` shows one valid answer.

## Tasks

1. Create `#recent_high_value_orders` for recent orders worth at least 500.00 with user names attached.
2. Create `#duplicate_import_emails` from the staged import using trimmed, lower-cased emails.
3. Create `#rejected_import_rows` for staged rows that should be rejected because a required business value is missing.

## Validation

- only the correct high-value orders appear
- duplicate emails are detected after normalization
- invalid staging rows are rejected explicitly instead of being silently ignored