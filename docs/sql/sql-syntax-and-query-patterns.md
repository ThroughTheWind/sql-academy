# SQL Syntax And Query Patterns

This page is the consolidated SQL syntax and query-pattern handbook for the core academy route.

Start with [Curriculum Map](../learning/curriculum-map.md) if you want the ordered lesson sequence. Use [Learning Glossary](../learning/glossary.md) when the blocker is vocabulary rather than syntax. Keep [Schema Quick Reference](../learning/schema-quick-reference.md) nearby when the seeded tables and relationships are still fuzzy.

Use this page when you need one place to answer five questions quickly:

- what this operation or pattern does
- when to use it
- when not to use it
- what a clean academy-schema example looks like
- what mistake usually breaks it first

Do not use this page as a full SQL Server language manual or as a replacement for the lesson sequence. It is a support reference for the SQL patterns the academy actively teaches and validates.

## Supported Scope

"Supported syntax" in this repository means the SQL operations and query patterns the core route already teaches through lessons, phases, and validation packs.

This page focuses on:

- read queries and shaped projections
- joins, grouping, and counting correctly
- predicates and expression patterns such as `CASE`, `IN`, `EXISTS`, and `NULL` handling
- analytical querying with CTEs, window functions, and stable pagination
- data changes with preview-modify-verify discipline
- transaction boundaries and isolation tradeoffs for multi-statement work
- index-definition patterns that support real filter, join, and sort shapes
- additive schema changes that preserve compatibility during rollout

It does not try to catalog every T-SQL keyword, DBA administration command, or engine-internals feature.

## Query Basics

### `SELECT`, Explicit Projection, And Aliases

`SELECT` returns the columns you ask for. Explicit projection keeps the result shape readable, and aliases keep multi-table queries understandable.

Use it when:

- you need a specific result contract
- you want output columns to stay understandable as queries grow
- you are joining tables and want stable short names such as `u`, `p`, or `c`

Do not use it when:

- `SELECT *` would hide which columns the query actually depends on

```sql
SELECT u.Id, u.UserName, u.Email
FROM academy.Users AS u
ORDER BY u.Id;
```

Common failure mode:

- `SELECT *` makes it harder to reason about query shape and can hide accidental column dependencies.

Best follow-up:

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Beginner 000: SQL Fundamentals And Safe Changes](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)

### `WHERE`

`WHERE` filters rows before later operators such as `ORDER BY`, `GROUP BY`, or `TOP` work on them.

Use it when:

- you need only the rows that match a precise business condition
- you want to narrow a result before sorting, grouping, or modifying data

Do not use it when:

- the condition depends on an aggregate such as `COUNT(...)`; that belongs in `HAVING`

```sql
SELECT o.Id, o.OrderNumber, o.Status, o.UpdatedUtc
FROM academy.Orders AS o
WHERE o.Status <> N'Filled'
ORDER BY o.UpdatedUtc DESC, o.Id DESC;
```

Common failure mode:

- a weak predicate returns more rows than intended, which is how accidental bulk updates and deletes start.

Best follow-up:

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Beginner 002: Filtering, Constraints, And Data Quality](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md)

### `ORDER BY` And Deterministic Tiebreakers

`ORDER BY` is how you ask SQL Server for a predictable output order. A deterministic tiebreaker makes that order stable when the main sort value repeats.

Use it when:

- the result must come back in a meaningful order such as latest first
- you are preparing a query for pagination or top-N selection

Do not use it when:

- you are assuming the table has a natural order without writing one explicitly

```sql
SELECT p.Id, p.Title, p.CreatedUtc
FROM academy.Posts AS p
ORDER BY p.CreatedUtc DESC, p.Id DESC;
```

Common failure mode:

- saying "latest" without a stable tiebreaker means the query still leaves tied rows unspecified.

Best follow-up:

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Lesson 03: Window Functions And Intermediate Querying](../learning/03-window-functions-and-intermediate-querying.md)

### `TOP`

`TOP` limits the number of rows returned after filtering and ordering.

Use it when:

- you need the first `N` rows from a well-defined sorted result
- you are building a review slice such as the most recent orders or latest posts

Do not use it when:

- you cannot explain which rows count as the first `N` rows yet

```sql
SELECT TOP (2) o.Id, o.OrderNumber, o.Status, o.UpdatedUtc
FROM academy.Orders AS o
WHERE o.Status <> N'Filled'
ORDER BY o.UpdatedUtc DESC, o.Id DESC;
```

Common failure mode:

- `TOP` without `ORDER BY` answers an incomplete question because SQL Server is free to return any qualifying rows.

Best follow-up:

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Beginner 000: SQL Fundamentals And Safe Changes](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)

## Joins And Aggregates

### `INNER JOIN`

`INNER JOIN` keeps only rows that have a match on both sides of the join condition.

Use it when:

- unmatched rows are not meaningful to the question
- you want only records that satisfy the relationship on both tables

Do not use it when:

- parent rows still matter even if the child row does not exist

```sql
SELECT p.Id, p.Title, u.UserName
FROM academy.Posts AS p
INNER JOIN academy.Users AS u
    ON u.Id = p.UserId
ORDER BY p.CreatedUtc DESC, p.Id DESC;
```

Common failure mode:

- assuming an inner join will preserve unmatched parent rows. It will not.

Best follow-up:

- [Lesson 02: Joins And Aggregations](../learning/02-joins-and-aggregations.md)
- [Beginner 001: Joins And Aggregations](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)

### `LEFT JOIN`

`LEFT JOIN` keeps every row from the left table and adds matching right-side data when it exists.

Use it when:

- the parent row must survive even when no child row exists
- you need to inspect missing relationships or count optional child rows

Do not use it when:

- you immediately reject the preserved `NULL` child rows in `WHERE`

```sql
SELECT p.Id, p.Title, c.Id AS CommentId
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
    ON c.PostId = p.Id
ORDER BY p.Id, c.Id;
```

Common failure mode:

- adding `WHERE c.Id IS NOT NULL` or another child-side predicate after the join collapses the outer-join behavior you were trying to preserve.

Best follow-up:

- [Lesson 02: Joins And Aggregations](../learning/02-joins-and-aggregations.md)
- [Beginner 001: Joins And Aggregations](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)

### `GROUP BY` And `HAVING`

`GROUP BY` reduces many rows into grouped answers. `HAVING` filters those groups after the aggregation is computed.

Use it when:

- the question is about summaries per key such as per post, per user, or per status
- you need to filter on an aggregated value such as comment count or total quantity

Do not use it when:

- you still need row-level detail; consider a window function instead

```sql
SELECT p.Id, p.Title, COUNT(c.Id) AS CommentCount
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
    ON c.PostId = p.Id
GROUP BY p.Id, p.Title
HAVING COUNT(c.Id) >= 2
ORDER BY CommentCount DESC, p.Id DESC;
```

Common failure mode:

- trying to put `COUNT(c.Id) >= 2` in `WHERE` instead of `HAVING`.

Best follow-up:

- [Lesson 02: Joins And Aggregations](../learning/02-joins-and-aggregations.md)
- [Beginner 001: Joins And Aggregations](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)

### `COUNT(*)` Versus `COUNT(column)`

`COUNT(*)` counts rows after the join. `COUNT(column)` counts only the rows where that column is not `NULL`.

Use it when:

- you need to be explicit about whether you are counting joined rows or only real child rows

Do not use it when:

- you are counting children through a `LEFT JOIN` but have not decided whether preserved parent rows should count

```sql
SELECT
    p.Id,
    p.Title,
    COUNT(*) AS JoinedRowCount,
    COUNT(c.Id) AS CommentCount
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
    ON c.PostId = p.Id
GROUP BY p.Id, p.Title
ORDER BY p.Id;
```

Common failure mode:

- `COUNT(*)` over a left-joined child table counts the preserved parent row even when no child exists.

Best follow-up:

- [Lesson 02: Joins And Aggregations](../learning/02-joins-and-aggregations.md)
- [Beginner 001: Joins And Aggregations](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)

## Predicate And Expression Patterns

### `CASE`

`CASE` derives a value from conditions inside the query result.

Use it when:

- you need a reporting bucket or readable classification in the result
- the output depends on row values but should still remain inside one query

Do not use it when:

- the logic is so large that the query stops being readable; consider breaking the shape into steps

```sql
SELECT
    o.OrderNumber,
    o.Status,
    CASE
        WHEN o.Status = N'Filled' THEN N'Closed'
        WHEN o.Status = N'Submitted' THEN N'Open'
        ELSE N'NeedsReview'
    END AS WorkflowBucket
FROM academy.Orders AS o
ORDER BY o.Id;
```

Common failure mode:

- forgetting `ELSE` and then being surprised when unmatched rows return `NULL`.

Best follow-up:

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Beginner 002: Filtering, Constraints, And Data Quality](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md)

### `IN`

`IN` checks whether a value matches one of several explicit candidates.

Use it when:

- you have a small known set of acceptable values
- the filter is clearer as a list than as several `OR` predicates

Do not use it when:

- the list is really coming from another table and an existence check or join would express the intent better

```sql
SELECT u.UserName, u.Email
FROM academy.Users AS u
WHERE u.UserName IN (N'ada', N'grace')
ORDER BY u.UserName;
```

Common failure mode:

- building very large hard-coded lists when the real problem is relational and should be expressed through another query.

Best follow-up:

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Beginner 002: Filtering, Constraints, And Data Quality](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md)

### `EXISTS`

`EXISTS` answers "does at least one related row exist?" without forcing a duplicated parent rowset into the result.

Use it when:

- you only care whether a related row exists, not which specific related row it is
- a join would create row multiplication that you do not actually need

Do not use it when:

- you need columns from the related row in the final projection

```sql
SELECT p.Id, p.Title
FROM academy.Posts AS p
WHERE EXISTS
(
    SELECT 1
    FROM academy.Comments AS c
    WHERE c.PostId = p.Id
)
ORDER BY p.Id;
```

Common failure mode:

- using a join just to prove existence and then needing extra `DISTINCT` or grouping to repair the duplicated parents.

Best follow-up:

- [Lesson 02: Joins And Aggregations](../learning/02-joins-and-aggregations.md)
- [Beginner 001: Joins And Aggregations](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)

### `NULL`, `IS NULL`, And `IS NOT NULL`

`NULL` means the value is absent or unknown. It is not the same thing as zero, `false`, or an empty string.

Use it when:

- you need to find unmatched rows preserved by an outer join
- you are checking whether a column is present or missing

Do not use it when:

- you are comparing with `= NULL` or `<> NULL`; use `IS NULL` or `IS NOT NULL`

```sql
SELECT p.Id, p.Title
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
    ON c.PostId = p.Id
WHERE c.Id IS NULL
ORDER BY p.Id;
```

Common failure mode:

- confusing `NULL` with empty text or writing `= NULL`, which does not behave as intended.

Best follow-up:

- [Lesson 02: Joins And Aggregations](../learning/02-joins-and-aggregations.md)
- [Lesson 04: Schema Design And Migration Safety](../learning/04-schema-design-and-migration-safety.md)

## Analytical Query Patterns

### CTEs

A common table expression splits a query into named steps. In this academy, its main value is readability and staged reasoning, not magical performance.

Use it when:

- a query becomes easier to validate in two stages than in one long statement
- you want to name an intermediate ranked, filtered, or aggregated rowset

Do not use it when:

- the only reason is "CTEs are faster"; that is not the contract

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

Common failure mode:

- assuming a CTE is automatically an optimization instead of a readability tool that still needs correct logic.

Best follow-up:

- [Lesson 03: Window Functions And Intermediate Querying](../learning/03-window-functions-and-intermediate-querying.md)
- [Intermediate 001: Window Functions And Pagination](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)

### `ROW_NUMBER()`

`ROW_NUMBER()` assigns a unique sequence within the window order you define.

Use it when:

- every row needs a unique position
- you need top-N-per-group logic or deterministic numbered pagination

Do not use it when:

- tied rows should share the same analytical position; that is where `RANK()` or `DENSE_RANK()` fits better

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

Common failure mode:

- omitting a deterministic tiebreaker from the window order, which makes the sequence unstable across tied values.

Best follow-up:

- [Lesson 03: Window Functions And Intermediate Querying](../learning/03-window-functions-and-intermediate-querying.md)
- [Intermediate 001: Window Functions And Pagination](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)

### `RANK()` And `DENSE_RANK()`

`RANK()` and `DENSE_RANK()` assign analytical positions where tied values share the same rank. `RANK()` leaves gaps after ties. `DENSE_RANK()` does not.

Use them when:

- tied values should share position instead of being forced into unique row numbers
- you are building leaderboard-style or grouped comparison outputs

Do not use them when:

- every row needs a unique sequence for pagination or latest-row selection

```sql
WITH PostCommentCounts AS
(
    SELECT
        p.Id,
        p.Title,
        COUNT(c.Id) AS CommentCount
    FROM academy.Posts AS p
    LEFT JOIN academy.Comments AS c
        ON c.PostId = p.Id
    GROUP BY p.Id, p.Title
)
SELECT
    Id,
    Title,
    CommentCount,
    RANK() OVER (ORDER BY CommentCount DESC) AS CommentRank,
    DENSE_RANK() OVER (ORDER BY CommentCount DESC) AS DenseCommentRank
FROM PostCommentCounts
ORDER BY CommentCount DESC, Id DESC;
```

Common failure mode:

- choosing `RANK()` when the real requirement is a unique row position.

Best follow-up:

- [Lesson 03: Window Functions And Intermediate Querying](../learning/03-window-functions-and-intermediate-querying.md)
- [Intermediate 001: Window Functions And Pagination](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)

### `PARTITION BY` And Running Totals

`PARTITION BY` resets the analytical calculation for each logical group inside the same query.

Use it when:

- the same calculation should restart per user, post, instrument, or other grouping key
- you need a running total without collapsing the result to one row per group

Do not use it when:

- you actually want one aggregated row per group; that is a `GROUP BY` question instead

```sql
SELECT
    t.Id,
    t.UserId,
    t.Quantity,
    t.TradedUtc,
    SUM(t.Quantity) OVER (
        PARTITION BY t.UserId
        ORDER BY t.TradedUtc ASC, t.Id ASC
        ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS RunningQuantity
FROM academy.Trades AS t
ORDER BY t.UserId, t.TradedUtc ASC, t.Id ASC;
```

Common failure mode:

- missing or wrong partition keys cause one user's running total to bleed into another user's sequence.

Best follow-up:

- [Lesson 03: Window Functions And Intermediate Querying](../learning/03-window-functions-and-intermediate-querying.md)
- [Intermediate 002: Cohort Analysis And Pagination Drift](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)

### Stable Pagination

Stable pagination means the page boundary stays predictable across executions. The academy treats a deterministic tiebreaker as mandatory for paged queries.

Use it when:

- you are returning slices of a sorted result to an API or report
- repeated executions must return the same page contract when the underlying data has not changed

Do not use it when:

- the sort key alone is not unique and you have not added a stable tiebreaker

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

Common failure mode:

- sorting only by a non-unique timestamp lets tied rows move between pages.

Best follow-up:

- [Lesson 03: Window Functions And Intermediate Querying](../learning/03-window-functions-and-intermediate-querying.md)
- [Intermediate 001: Window Functions And Pagination](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)
- [Intermediate 002: Cohort Analysis And Pagination Drift](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)

## Data Change Patterns

These examples change data. Use them in a scratch session or the matching validation packs, not as casual copy-paste into a shared environment.

### `INSERT`

`INSERT` adds a new row.

Use it when:

- you have all required values or safe defaults
- the new row satisfies uniqueness and foreign-key constraints

Do not use it when:

- you have not yet checked whether the row violates a unique or referential rule

```sql
INSERT INTO academy.Users (UserName, Email, CreatedUtc)
VALUES (N'edith', N'edith@sqlacademy.local', SYSUTCDATETIME());
```

Common failure mode:

- attempting an insert that duplicates a unique business value such as `UserName` or `Email`.

Best follow-up:

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Beginner 000: SQL Fundamentals And Safe Changes](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)

### `UPDATE`

`UPDATE` modifies existing rows.

Use it when:

- you can preview the exact target rows first
- the new values preserve the table's rules and downstream meaning

Do not use it when:

- you have not already proven the `WHERE` clause is scoped to the intended rows

```sql
SELECT o.Id, o.OrderNumber, o.Status, o.UpdatedUtc
FROM academy.Orders AS o
WHERE o.OrderNumber = N'ORD-2025-0002';

UPDATE academy.Orders
SET Status = N'Filled',
    UpdatedUtc = SYSUTCDATETIME()
WHERE OrderNumber = N'ORD-2025-0002';
```

Common failure mode:

- broad updates start with a missing or weak predicate, not with a SQL Server bug.

Best follow-up:

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Beginner 000: SQL Fundamentals And Safe Changes](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)

### `DELETE`

`DELETE` removes rows that match the predicate.

Use it when:

- you have previewed the exact target rows first
- you understand whether child rows will also be removed through cascade behavior

Do not use it when:

- you are guessing about the predicate or the relationship impact

```sql
SELECT p.Id, p.Title
FROM academy.Posts AS p
WHERE p.Title = N'Temporary post';

DELETE FROM academy.Posts
WHERE Title = N'Temporary post';
```

Common failure mode:

- forgetting that deletes can affect related rows through cascade rules or through application expectations that still depend on the row.

Best follow-up:

- [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md)
- [Lesson 05: Transactions, Blocking, And Deadlocks](../learning/05-transactions-blocking-and-deadlocks.md)

## Transaction And Isolation Patterns

These patterns matter when one correct statement is no longer enough and the real question becomes what happens across multiple statements or sessions.

### `BEGIN TRAN`, `COMMIT`, And `ROLLBACK`

An explicit transaction defines one atomic unit of work. `COMMIT` makes the change durable. `ROLLBACK` abandons it.

Use it when:

- multiple statements must succeed or fail together
- the write sequence needs one deliberate transaction boundary

Do not use it when:

- the transaction would stay open across user input, network calls, or other non-database work
- one statement already gives you the atomic behavior you need

```sql
BEGIN TRAN;

UPDATE academy.Orders
SET Status = N'Filled',
    UpdatedUtc = SYSUTCDATETIME()
WHERE OrderNumber = N'ORD-2025-0002';

SELECT o.Id, o.OrderNumber, o.Status, o.UpdatedUtc
FROM academy.Orders AS o
WHERE o.OrderNumber = N'ORD-2025-0002';

ROLLBACK TRAN;
```

Common failure mode:

- opening the transaction too early or closing it too late, which inflates lock duration and makes blocking worse.

Best follow-up:

- [Lesson 05: Transactions, Blocking, And Deadlocks](../learning/05-transactions-blocking-and-deadlocks.md)
- [Senior 001: Concurrency, Blocking, And Deadlocks](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)

### `SET TRANSACTION ISOLATION LEVEL`

This statement changes how the session balances correctness guarantees against concurrency cost.

Use it when:

- you can name the anomaly or concurrency behavior the operation must prevent
- you need to reason deliberately about default versus stronger isolation

Do not use it when:

- stronger isolation is just a reflex to hide a blocking or design problem you have not explained yet

```sql
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

BEGIN TRAN;

SELECT o.Id, o.OrderNumber, o.Status
FROM academy.Orders AS o
WHERE o.UserId = 1
ORDER BY o.Id;

UPDATE academy.Orders
SET UpdatedUtc = SYSUTCDATETIME()
WHERE OrderNumber = N'ORD-2025-0002';

ROLLBACK TRAN;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
```

Common failure mode:

- jumping to stronger isolation before naming the anomaly you need to prevent, then paying for the extra contention without solving the real design issue.

Best follow-up:

- [Lesson 05: Transactions, Blocking, And Deadlocks](../learning/05-transactions-blocking-and-deadlocks.md)
- [Senior 001: Concurrency, Blocking, And Deadlocks](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)

## Index Definition Patterns

These patterns are about supporting a real query shape, not about adding indexes because tuning feels unfinished.

### `CREATE INDEX`

In this course, a new index should have a named consumer and a clear filter, join, or sort pattern behind it.

Use it when:

- a known read path needs help on selective filtering, join access, or ordered retrieval
- you can explain why the key columns match the query shape

Do not use it when:

- the index has no named workload consumer
- the proposed index mostly duplicates an existing one without a clear gain

```sql
CREATE INDEX IX_Orders_Status_UpdatedUtc
ON academy.Orders (Status, UpdatedUtc DESC);
```

Common failure mode:

- adding an index because "indexes help performance" without naming the exact query family it is supposed to improve.

Best follow-up:

- [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md)
- [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)

### `CREATE INDEX ... INCLUDE (...)`

`INCLUDE` adds non-key columns so the index can satisfy the projection without making every projected column part of the sort or seek key.

Use it when:

- the key columns already support the filter or order pattern
- a few extra projected columns would otherwise force expensive base-row lookups

Do not use it when:

- you are turning the index into a wide copy of the table just to avoid thinking about query shape or projection

```sql
CREATE INDEX IX_Orders_Status_UpdatedUtc_Covering
ON academy.Orders (Status, UpdatedUtc DESC)
INCLUDE (OrderNumber, TotalAmount);
```

Common failure mode:

- building giant covering indexes that help one read path a little while making writes and maintenance noticeably worse.

Best follow-up:

- [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md)
- [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)

## Safe Schema Change Patterns

### Add, Backfill, Enforce

For schema evolution in this academy, the safest default is additive change first, then controlled backfill, then later contract enforcement.

Use it when:

- you need a new required column or stricter rule on a populated table
- the application and schema must stay compatible across a deployment window

Do not use it when:

- you are tempted to add a required column in one step to save time

Migration-shape example from the Lesson 04 rollout pattern:

```sql
ALTER TABLE academy.Orders
ADD ExternalReference NVARCHAR(64) NULL;

UPDATE academy.Orders
SET ExternalReference = OrderNumber
WHERE ExternalReference IS NULL;

ALTER TABLE academy.Orders
ALTER COLUMN ExternalReference NVARCHAR(64) NOT NULL;
```

Common failure mode:

- a direct `NOT NULL` change on populated data turns a schema edit into an avoidable release incident.

Best follow-up:

- [Lesson 04: Schema Design And Migration Safety](../learning/04-schema-design-and-migration-safety.md)
- [Advanced 002: Staged Backfill And Contract Enforcement](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)

## Common Cross-Cutting Mistakes

- using `SELECT *` when a stable result shape matters
- treating `TOP` without `ORDER BY` as if it returned the newest or highest rows
- forgetting that `LEFT JOIN` plus a child-side `WHERE` filter can behave like an inner join
- using `COUNT(*)` when the real question is child-count after a left join
- paginating on a non-unique sort key without a deterministic tiebreaker
- running `UPDATE` or `DELETE` without previewing the exact target set first
- leaving a transaction open longer than the database work actually requires
- treating stronger isolation as a blanket fix instead of a deliberate tradeoff
- adding indexes without naming the exact filter, join, or sort pattern they support
- introducing a breaking schema change before the backfill and compatibility window are ready

## Where To Go Next

- use [SQL Track](README.md) when you need the full SQL topic matrix, repo anchors, and executable validation surfaces
- use [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md) and [Lesson 02: Joins And Aggregations](../learning/02-joins-and-aggregations.md) when the basics still need slower, guided explanation
- use [Lesson 03: Window Functions And Intermediate Querying](../learning/03-window-functions-and-intermediate-querying.md) when the main question is pagination, ranking, or analytical query shape
- use [Lesson 04: Schema Design And Migration Safety](../learning/04-schema-design-and-migration-safety.md) when the question becomes schema rollout safety rather than query syntax
- use [Lesson 05: Transactions, Blocking, And Deadlocks](../learning/05-transactions-blocking-and-deadlocks.md) when the main question becomes transaction boundaries, blocking, deadlocks, or isolation tradeoffs across sessions
- use [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md) when the main question becomes index design, plan evidence, or parameter-sensitive performance behavior
- use [Exercise System](../exercises/README.md) when you want hands-on validation instead of reference reading