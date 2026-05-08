# Intermediate 002: Cohort Analysis And Pagination Drift

## Objective

Connect analytical SQL to product-style reporting by combining cohort summaries, running totals, and deterministic pagination.

## Before You Start

- complete [Intermediate 001](../001-window-functions-and-pagination/README.md) or be comfortable with stable pagination and running totals already
- read [Lesson 03: Window Functions And Intermediate Querying](../../../../docs/learning/03-window-functions-and-intermediate-querying.md)
- keep [Learning Glossary](../../../../docs/learning/glossary.md) nearby if terms such as cohort, deterministic order, or running total are still slow

## Suggested Workflow

1. Run `starter.sql` unchanged and inspect the trade fixture before you edit `answer.sql`.
2. Solve the cohort summary first, then the stable page, then the running trade quantity output.
3. Use small read-only checks to confirm the order and grouping logic before you create the final temp tables.
4. Open `hints.md` only after you can explain whether the mistake is in the cohort grouping, the page order, or the running total sequence.

## Sample Sanity Checks

Your answer should line up with these sample facts before you run the harness.

`#cohort_summary` should contain this row:

| CohortMonth | UserCount | OrderingUserCount |
| --- | --- | --- |
| `2025-01-01` | 4 | 3 |

`#stable_trade_page` should contain these rows in this exact order:

| RowNum | TradeId | UserId | TradedUtc | Quantity |
| --- | --- | --- | --- | --- |
| 1 | 21 | 2 | `2025-01-20 09:15:00.000` | `6.0000` |
| 2 | 12 | 1 | `2025-01-20 09:10:00.000` | `3.0000` |
| 3 | 20 | 2 | `2025-01-20 09:05:00.000` | `4.0000` |

`#running_trade_quantity` should contain these rows:

| TradeId | UserId | RunningQuantity |
| --- | --- | --- |
| 10 | 1 | `5.0000` |
| 11 | 1 | `12.0000` |
| 12 | 1 | `15.0000` |
| 20 | 2 | `4.0000` |
| 21 | 2 | `10.0000` |

## If You Get Stuck

- verify the deterministic page order first because one weak sort clause can invalidate the rest of the exercise
- prove the running totals on paper for one user before you debug the SQL
- use `broken.sql` and `expected-outcomes.md` to name the failure before you open `optional-solution.sql`

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