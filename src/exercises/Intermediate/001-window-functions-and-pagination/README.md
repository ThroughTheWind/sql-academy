# Intermediate 001: Window Functions And Pagination

## Objective

Use window functions to rank posts and trades without losing row-level detail.

## Scenario

You need leaderboard-style outputs and stable pagination for an API report, but the current query uses grouped aggregates where row-level context is still required.

## Assets

- `starter.sql` contains the baseline query shapes.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the expected result tables in one SQL session.
- `broken.sql` contains the incorrect implementation.
- `expected-outcomes.md` lists the checks the result must satisfy.
- `hints.md` gives progressive guidance.
- `optional-solution.sql` shows one valid solution.

## Tasks

1. Rank users by post volume.
2. Produce a running trade total per user ordered by `TradedUtc`.
3. Build a stable pagination query for trades using `ROW_NUMBER()`.

## Validation

- ties are deterministic
- running totals reset per user
- the pagination window is stable across repeated calls