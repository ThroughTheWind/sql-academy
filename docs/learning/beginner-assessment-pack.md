# Beginner Assessment Pack

Use this pack after Lessons 01 and 02 and their companion validation exercises. This pack checks whether you can still reason correctly when fundamentals, joins, and aggregation questions are mixed together.

## Covers

- [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md)
- [Lesson 02: Joins And Aggregations](02-joins-and-aggregations.md)
- [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)
- [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)

## How To Use This Pack

1. Answer the short questions without opening the lesson docs.
2. For the scenarios, write the likely issue, the first query or check you would run, and the fix you would test.
3. Use the answer key only after you have committed to your own explanation.

## Short Questions

1. Why is `ORDER BY` required for a query that claims to return the latest rows?
2. Why is a preview `SELECT` good practice before an `UPDATE` or `DELETE`?
3. What does a foreign key protect in the data model?
4. What is the practical difference between an inner join and a left join?
5. Why can `COUNT(*)` be wrong for comment counts on a left-joined query?
6. When should `COUNT(DISTINCT ...)` be considered?
7. Why is it dangerous to assume a grouped result is correct just because the join path is correct?

## Scenarios

1. A query called “latest orders” returns different row order on repeated executions. What is the likely problem, and what is the cheapest safe fix?
2. A beginner changes `academy.Orders` with an `UPDATE` statement and accidentally modifies every row. What habit would have prevented that, and what was structurally missing from the statement?
3. A report joins posts to comments and the post counts inflate. What rowset should you inspect before rewriting the aggregate?
4. A learner writes a left join but then filters on child columns in the `WHERE` clause. What behavior likely changed?

## Answer Key

1. Without `ORDER BY`, SQL does not guarantee any result order, so “latest” is not actually expressed.
2. It confirms exactly which rows will change before you perform a potentially destructive write.
3. It protects referential integrity by ensuring child rows only reference valid parent rows.
4. An inner join returns only matched rows, while a left join preserves the parent row even when there is no child match.
5. `COUNT(*)` counts joined rows, including the preserved parent row from the left join, while `COUNT(child.Id)` ignores `NULL` child rows.
6. Consider it when the join path duplicates the same logical entity and you need to count unique entities rather than raw rows.
7. Because the join may multiply rows before aggregation, which can make the final totals wrong even when the relationship path is valid.
8. The likely problem is unstable ordering on ties; add a deterministic tiebreaker such as `Id` to the `ORDER BY`.
9. Previewing with a `SELECT` would likely have prevented it, and the missing structure was a sufficiently selective `WHERE` clause.
10. Inspect the pre-aggregate joined rowset of posts and comments first.
11. The query likely stopped behaving like a true outer join and effectively became inner-join behavior for unmatched parents.

## Ready To Advance When

- you can explain why deterministic ordering needs a stable tiebreaker
- you can predict how a join will change row counts before adding `GROUP BY`
- you can distinguish safe counting patterns on left joins without guessing
- you can point to one uniqueness rule and one foreign-key rule in the schema and explain how each would fail