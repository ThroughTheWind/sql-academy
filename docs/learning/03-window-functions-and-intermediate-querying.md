# Lesson 03: Window Functions And Intermediate Querying

## Focus

Use analytical SQL to keep row-level detail while adding rankings, running totals, and stable pagination. This is the point where you stop choosing between “detail” and “summary” as if they were mutually exclusive.

## Why This Lesson Matters

Grouped aggregates are useful, but they discard row-level context. Real systems often need both:

- a trade row and the running total around it
- a post row and its rank among other posts
- a page of API results that stays stable across repeated requests

Window functions solve those problems cleanly when you understand partitioning and ordering.

## Repository Anchors

- [Phase 2: Intermediate Querying](../phases/phase-2-intermediate-querying.md)
- [Intermediate 001: Window Functions And Pagination](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)
- [Trade Dapper read path](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)
- [Performance comparison tests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs)

## Mental Models To Keep

### A Window Function Does Not Collapse Rows

Unlike `GROUP BY`, a window function returns one result per input row while still letting you compute analytical values.

### The Window Has Two Axes

- `PARTITION BY` decides which rows belong to the same analytical group
- `ORDER BY` decides the sequence inside that group

If either axis is wrong or missing, the analytical answer is wrong.

### Pagination Is Only Safe When Ordering Is Deterministic

If two rows can tie on the sort expression and you provide no stable tiebreaker, page boundaries can drift.

## `ROW_NUMBER`, `RANK`, And `DENSE_RANK`

### `ROW_NUMBER()`

Assigns a unique sequence within each partition, even when values tie.

```sql
SELECT
	t.Id,
	t.UserId,
	t.TradedUtc,
	ROW_NUMBER() OVER (
		PARTITION BY t.UserId
		ORDER BY t.TradedUtc DESC, t.Id DESC) AS TradeSequence
FROM academy.Trades AS t
ORDER BY t.UserId, TradeSequence;
```

Use it for pagination, top-N-per-group, or deterministic sequencing.

### `RANK()`

Rows with tied values get the same rank, and the next rank is skipped.

### `DENSE_RANK()`

Rows with tied values get the same rank, but the next rank is not skipped.

Use `RANK` and `DENSE_RANK` when tied values should have shared analytical position instead of forced uniqueness.

## Running Totals And Partitioning

One of the fastest ways to understand windows is a running total.

```sql
SELECT
	t.Id,
	t.UserId,
	t.TradedUtc,
	t.Quantity,
	SUM(t.Quantity) OVER (
		PARTITION BY t.UserId
		ORDER BY t.TradedUtc, t.Id
		ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS RunningQuantity
FROM academy.Trades AS t
ORDER BY t.UserId, t.TradedUtc, t.Id;
```

Why the query works:

- `PARTITION BY t.UserId` resets the total per user
- `ORDER BY t.TradedUtc, t.Id` defines the event sequence
- the frame says “from the start of the partition through the current row”

What breaks it:

- remove `PARTITION BY` and users get mixed together
- remove a stable tiebreaker and tied timestamps become ambiguous

## Window Functions Versus `GROUP BY`

Suppose you want each trade row plus the total number of trades for that user.

Window function version:

```sql
SELECT
	t.Id,
	t.UserId,
	t.InstrumentId,
	t.TradedUtc,
	COUNT(*) OVER (PARTITION BY t.UserId) AS TradesForUser
FROM academy.Trades AS t
ORDER BY t.UserId, t.TradedUtc, t.Id;
```

This preserves detail.

Grouped version:

```sql
SELECT
	t.UserId,
	COUNT(*) AS TradesForUser
FROM academy.Trades AS t
GROUP BY t.UserId;
```

This discards trade-level detail. Neither query is “better” in the abstract. They answer different questions.

## CTEs As A Readability Tool

CTEs do not magically optimize a query. Their main value in this course is readability and stepwise reasoning.

```sql
WITH RankedTrades AS
(
	SELECT
		t.Id,
		t.UserId,
		t.TradedUtc,
		ROW_NUMBER() OVER (
			PARTITION BY t.UserId
			ORDER BY t.TradedUtc DESC, t.Id DESC) AS RankWithinUser
	FROM academy.Trades AS t
)
SELECT Id, UserId, TradedUtc, RankWithinUser
FROM RankedTrades
WHERE RankWithinUser <= 2
ORDER BY UserId, RankWithinUser;
```

That pattern is common for top-N-per-group answers.

## Stable Pagination

The repository’s [Trade Dapper read path](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs) sorts by a chosen column and then adds `t.Id DESC` as a tiebreaker. That is not cosmetic. It is what makes pagination deterministic when the primary sort value ties.

Offset pagination example:

```sql
SELECT
	t.Id,
	t.UserId,
	t.InstrumentId,
	t.TradedUtc
FROM academy.Trades AS t
ORDER BY t.TradedUtc DESC, t.Id DESC
OFFSET 0 ROWS FETCH NEXT 2 ROWS ONLY;
```

If you sort only by `TradedUtc`, two rows with the same timestamp can move between pages across executions or plan shapes.

Window-function pagination example:

```sql
WITH NumberedTrades AS
(
	SELECT
		t.Id,
		t.UserId,
		t.InstrumentId,
		t.TradedUtc,
		ROW_NUMBER() OVER (ORDER BY t.TradedUtc DESC, t.Id DESC) AS RowNum
	FROM academy.Trades AS t
)
SELECT Id, UserId, InstrumentId, TradedUtc
FROM NumberedTrades
WHERE RowNum BETWEEN 1 AND 2
ORDER BY RowNum;
```

## Analytical Patterns You Should Practice

### Top-N Per Group

“Return the latest trade per user” is a classic use case.

### Running Totals

“Show each row and the cumulative quantity so far.”

### Leaderboards

“Rank users by post count or trade activity.”

### Gap Detection

Window functions can compare one row to the previous row using `LAG` and `LEAD` when you later want change detection or interval analysis.

## Common Failure Modes

### Missing Partition

If the running total should reset per user and it does not, your partition is wrong.

### Missing Tiebreaker

If pagination or row numbering depends on a non-unique sort column, add a stable tiebreaker such as `Id`.

### Confusing Presentation Order With Window Order

The `ORDER BY` inside `OVER (...)` defines the analytical sequence. The final `ORDER BY` defines the displayed result sequence. They can differ.

### Using Windows When A Simple Aggregate Would Do

If you only need one row per group, a grouped aggregate is often simpler than a windowed query.

## A Good Practice Sequence

1. Rank trades per user with `ROW_NUMBER()`.
2. Re-run the query after removing the tiebreaker and explain the risk.
3. Build a running trade quantity per user.
4. Compare a grouped aggregate with a windowed aggregate and explain the shape difference.
5. Implement stable pagination with both `OFFSET/FETCH` and a numbered CTE.

## Exit Criteria

You are ready for Lesson 04 when you can do all of the following:

- explain the difference between `ROW_NUMBER`, `RANK`, and `DENSE_RANK`
- build a running total that resets correctly per user
- explain why a query needs `PARTITION BY`, `ORDER BY`, or both
- demonstrate deterministic pagination with a stable tiebreaker
- choose between `GROUP BY` and a window function based on the required result shape

## Review Questions

1. Why is a window function useful when you need analytics without losing row-level detail?
2. What breaks when a running-total query is missing the correct `PARTITION BY`?
3. Why is a stable tiebreaker required for safe pagination?
4. When is `ROW_NUMBER()` a better fit than `RANK()` or `DENSE_RANK()`?
5. Why can a CTE help even when it does not change the physical work of the query?

## Suggested Answers

1. A window function computes analytical values over related rows while still returning one output row per input row.
2. The running total crosses logical group boundaries, so rows from different users or categories get mixed into the same sequence.
3. Without a deterministic tiebreaker, tied rows can move between pages across executions, which makes paging unstable.
4. `ROW_NUMBER()` is the better fit when every row needs a unique sequence position, such as for pagination or picking the latest row per group.
5. A CTE can make the reasoning clearer by separating stages of the query, which helps you verify logic before optimizing shape or performance.

## Challenge Questions

1. Write a stable pagination query for trades and explain how you would prove that rows cannot drift between repeated calls.
2. Compare a grouped solution and a window-function solution for “total trades per user” and explain which one better fits an API list response.
3. Design a query that returns the latest trade per user and defend your choice of ranking function.

## Interview-Style Prompts

1. Explain to an interviewer why window functions are often a better fit than grouped aggregates when building leaderboard or running-total features.
2. Defend the statement “unstable ordering is a correctness bug, not just a UX issue.”

## Lesson Checkpoint

- choose between `ROW_NUMBER()`, `RANK()`, and `DENSE_RANK()` for one concrete output and defend it
- explain why running totals need both a partition and a deterministic order
- defend one pagination `ORDER BY` that will stay stable across repeated calls

## Next Lesson

Move to [Lesson 04](04-schema-design-and-migration-safety.md) once analytical querying feels dependable and you can keep detail without losing correctness.