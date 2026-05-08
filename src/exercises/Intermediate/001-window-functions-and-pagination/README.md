# Intermediate 001: Window Functions And Pagination

## Objective

Use window functions to rank posts and trades without losing row-level detail.

## Before You Start

- read [Lesson 03: Window Functions And Intermediate Querying](../../../../docs/learning/03-window-functions-and-intermediate-querying.md)
- be comfortable with stable ordering, `ROW_NUMBER()`, and the difference between grouped results and row-level outputs
- keep [Learning Glossary](../../../../docs/learning/glossary.md) nearby if terms such as partition, rank, or deterministic order are still slow

## Suggested Workflow

1. Run `starter.sql` unchanged and inspect the raw rowsets before you edit `answer.sql`.
2. Solve ranking first, then running totals, then the stable page window.
3. Verify the ordering and partition logic before you optimize the query shape.
4. Open `hints.md` only after you can explain what the current result is doing wrong.

## Sample Sanity Checks

Your answer should line up with these sample facts before you run the harness.

`#post_volume_ranks` should contain these rows:

| DisplayOrder | UserId | UserName | PostCount | PostRank |
| --- | --- | --- | --- | --- |
| 1 | 1 | `ada` | 1 | 1 |
| 2 | 2 | `grace` | 1 | 1 |
| 3 | 3 | `linus` | 1 | 1 |
| 4 | 4 | `margaret` | 1 | 1 |

`#running_trade_totals` should contain these rows for the current seed data:

| TradeId | UserId | Quantity | RunningQuantity |
| --- | --- | --- | --- |
| 1 | 1 | `100.0000` | `100.0000` |
| 2 | 2 | `25.0000` | `25.0000` |
| 3 | 3 | `25000.0000` | `25000.0000` |
| 4 | 4 | `2.0000` | `2.0000` |

`#stable_trade_page` should begin like this:

| RowNumber | TradeId | UserId | TradedUtc |
| --- | --- | --- | --- |
| 1 | 4 | 4 | `2025-01-21 09:45:00.000` |
| 2 | 3 | 3 | `2025-01-21 09:10:00.000` |

## If You Get Stuck

- go back to the plain rowset and prove the intended sort order before you add `ROW_NUMBER()` or a running total
- verify the partition key before you debug the cumulative quantity
- do not open `optional-solution.sql` until you already have a concrete opinion about which order, partition, or tiebreaker is wrong

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