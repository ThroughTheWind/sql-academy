# Beginner 002: Filtering, Constraints, And Data Quality

## Objective

Practice safe filtering, basic data-quality triage, and constraint-aware reasoning without jumping to schema changes too early.

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