# Intermediate 002: Cohort Analysis And Pagination Drift

## Objective

Connect analytical SQL to product-style reporting by combining cohort summaries, running totals, and deterministic pagination.

## Scenario

You need a learner-safe reporting slice that summarizes user cohorts and returns a page of trades that will not drift when timestamps tie.

## Assets

- `starter.sql` creates the deterministic trade fixture for the exercise.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the answer in one SQL session.
- `broken.sql` shows common analytical and pagination mistakes.
- `expected-outcomes.md` defines the required outputs.
- `hints.md` offers guidance.
- `optional-solution.sql` shows one valid implementation.

## Tasks

1. Create `#cohort_summary` for user cohorts with counts of total users and users with at least one order.
2. Create `#stable_trade_page` for the first three trades in deterministic descending trade order.
3. Create `#running_trade_quantity` to show cumulative quantity per user.

## Validation

- the cohort summary is correct for the seeded users and orders
- the stable trade page uses a deterministic tiebreaker
- running quantities reset per user and grow in the correct sequence