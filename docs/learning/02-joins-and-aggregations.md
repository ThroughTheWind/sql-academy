# Lesson 02: Joins And Aggregations

## Focus

Learn how relationships change row counts, how grouped answers differ from detail answers, and why accidental duplication is one of the most common SQL mistakes in real systems.

## Why This Lesson Matters

Most incorrect reporting queries are not syntactically broken. They run and return believable numbers, but the numbers are wrong because the author did not reason about join cardinality.

You should leave this lesson with one habit that prevents a large class of bugs:

Before you aggregate, inspect the joined rowset you are about to aggregate.

## Repository Anchors

- [Phase 2: Intermediate Querying](../phases/phase-2-intermediate-querying.md)
- [Beginner 001: Joins And Aggregations](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)
- [Seed data for posts and comments](../../db/seed/002_seed_social_and_orders.sql)
- [Post EF Core read path](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs)

## Mental Models To Keep

### A Join Does Not “Attach” Data, It Multiplies Rows

If one post has two comments, joining posts to comments produces two rows for that post before aggregation.

### The Correct Join Can Still Produce The Wrong Aggregate

The join may be logically correct, but the aggregate can still be wrong if you count the wrong thing.

### `LEFT JOIN` Preserves Parent Rows Only Until You Filter Them Away

A `LEFT JOIN` followed by a `WHERE` predicate on the nullable child columns often collapses back into inner-join behavior.

## Know The Relationships First

For this lesson, the most useful path is:

- `academy.Users` -> `academy.Posts` via `Posts.UserId`
- `academy.Posts` -> `academy.Comments` via `Comments.PostId`

That means:

- one user can have many posts
- one post can have many comments
- one user can comment on many posts

The key consequence is that `Users -> Posts -> Comments` is a one-to-many-to-many path. Row multiplication is expected.

## Inner Join Versus Left Join

### Inner Join

Use an inner join when you only want rows that have a match on both sides.

```sql
SELECT p.Id, p.Title, u.UserName
FROM academy.Posts AS p
INNER JOIN academy.Users AS u
	ON u.Id = p.UserId
ORDER BY p.CreatedUtc DESC, p.Id DESC;
```

This is safe when every post must have a user and unmatched posts are not meaningful.

### Left Join

Use a left join when you need to keep the parent row even if no child row exists.

```sql
SELECT p.Id, p.Title, c.Id AS CommentId
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
	ON c.PostId = p.Id
ORDER BY p.Id, c.Id;
```

If a post has no comments, you still get the post row, but the comment columns are `NULL`.

## Always Inspect The Pre-Aggregate Rowset

Before you count comments per post, inspect the raw joined result.

```sql
SELECT
	p.Id AS PostId,
	p.Title,
	c.Id AS CommentId,
	c.UserId AS CommentUserId
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
	ON c.PostId = p.Id
ORDER BY p.Id, c.Id;
```

Look for these facts:

- does each post appear once or multiple times?
- which posts have `NULL` comment columns?
- are you comfortable explaining the duplicated parent rows?

If you cannot explain that rowset, do not add `GROUP BY` yet.

## Aggregations That Commonly Go Wrong

### `COUNT(*)`

`COUNT(*)` counts rows after the join, including rows where child columns are `NULL` because of a left join.

```sql
SELECT
	p.Id,
	p.Title,
	COUNT(*) AS JoinedRowCount
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
	ON c.PostId = p.Id
GROUP BY p.Id, p.Title
ORDER BY p.Id;
```

This answers “how many rows exist in the joined rowset per post,” not “how many comments does the post have.”

### `COUNT(c.Id)`

```sql
SELECT
	p.Id,
	p.Title,
	COUNT(c.Id) AS CommentCount
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
	ON c.PostId = p.Id
GROUP BY p.Id, p.Title
ORDER BY p.Id;
```

This works because `COUNT(column)` ignores `NULL`, so posts with no comments count as zero instead of one.

### `COUNT(DISTINCT ...)`

Use `COUNT(DISTINCT ...)` when multiple joins can duplicate the same entity and you need distinct entity counts.

```sql
SELECT
	u.Id,
	u.UserName,
	COUNT(DISTINCT p.Id) AS PostCount
FROM academy.Users AS u
LEFT JOIN academy.Posts AS p
	ON p.UserId = u.Id
GROUP BY u.Id, u.UserName
ORDER BY u.Id;
```

## Grouped Answers Versus Detail Answers

Grouped queries intentionally collapse detail into summary.

```sql
SELECT
	u.UserName,
	COUNT(DISTINCT p.Id) AS PostsWritten,
	COUNT(c.Id) AS CommentsAcrossOwnedPosts
FROM academy.Users AS u
LEFT JOIN academy.Posts AS p
	ON p.UserId = u.Id
LEFT JOIN academy.Comments AS c
	ON c.PostId = p.Id
GROUP BY u.UserName
ORDER BY u.UserName;
```

That query is useful for reporting, but it does not preserve per-post detail. If you need both summary and detail, later lessons will show window functions and staged query shapes.

## The Classic `LEFT JOIN` Trap

This query looks like a left join, but it behaves like an inner join:

```sql
SELECT p.Id, p.Title, c.Id AS CommentId
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
	ON c.PostId = p.Id
WHERE c.CreatedUtc >= '2025-01-17';
```

Why it breaks:

- unmatched rows have `NULL` in `c.CreatedUtc`
- the `WHERE` predicate rejects those `NULL` rows
- posts without matching comments disappear

Safer alternatives:

- move the filter into the `ON` clause when you still want unmatched parents preserved
- or write the query as an inner join if unmatched parents truly are irrelevant

Example:

```sql
SELECT p.Id, p.Title, c.Id AS CommentId
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
	ON c.PostId = p.Id
   AND c.CreatedUtc >= '2025-01-17';
```

## How This Shows Up In Application Code

In [Post EF Core read path](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), the projection includes `post.Comments.Count`. That means the application wants a per-post comment count while keeping one result row per post.

The design intent is important:

- the result is post-shaped, not comment-shaped
- comment count is a derived value, not a reason to duplicate posts
- stable sorting is applied with explicit tie-breakers

When you read ORM code later, translate it back into the same SQL questions:

- what is the base rowset?
- what relationships are traversed?
- where could duplication happen?
- what shape does the final projection promise?

## A Good Practice Sequence

1. Query `Users`, `Posts`, and `Comments` separately.
2. Join `Posts` to `Users` and explain why row counts do not increase unexpectedly.
3. Join `Posts` to `Comments` and explain why row counts can increase.
4. Count comments with `COUNT(*)` and with `COUNT(c.Id)`.
5. Explain the difference without hand-waving.
6. Build a per-user post count.
7. Build a per-post comment count with stable ordering.

## Common Failure Modes

### Aggregating Too Early

If you aggregate before you understand the join rowset, you are hiding the evidence you need.

### Grouping By Too Many Columns

If you accidentally include detail columns in the `GROUP BY`, you can defeat the summary you were trying to build.

### Counting The Wrong Side Of The Join

For outer joins, `COUNT(*)` and `COUNT(child.Id)` answer different questions. Choose deliberately.

### Assuming “One Query” Means “One Correct Query”

Sometimes the cleanest answer is a staged approach: inspect a detail rowset first, then summarize it.

## Exit Criteria

You are ready for Lesson 03 when you can do all of the following:

- explain what rows exist before a `GROUP BY` executes
- choose between inner join and left join based on the business question
- explain when a left join silently becomes inner-join behavior
- justify `COUNT(*)` versus `COUNT(child.Id)` versus `COUNT(DISTINCT ...)`
- produce correct per-user and per-post summaries from the seeded data

## Review Questions

1. Why should you inspect the joined rowset before adding `GROUP BY`?
2. When does a `LEFT JOIN` stop behaving like a true outer join?
3. Why can `COUNT(*)` and `COUNT(c.Id)` produce different answers on the same left-joined query?
4. When is `COUNT(DISTINCT ...)` the right repair for a duplicated aggregate?
5. What is the practical difference between a grouped answer and a detail answer?

## Suggested Answers

1. The joined rowset shows where row multiplication is happening, which is the evidence you need before deciding how to aggregate correctly.
2. It effectively becomes inner-join behavior when a `WHERE` predicate rejects the nullable child-side rows that the outer join was meant to preserve.
3. `COUNT(*)` counts all joined rows, including the preserved parent row created by the left join, while `COUNT(c.Id)` ignores the `NULL` child values.
4. It is the right fix when the same logical entity is duplicated by the join path and you need to count unique entities rather than raw joined rows.
5. A grouped answer collapses rows into summaries, while a detail answer preserves row-level context such as one row per post or one row per comment.

## Challenge Questions

1. Write a query that returns every post exactly once together with its comment count and explain why your aggregate is safe against duplication.
2. Show one query where a `LEFT JOIN` is required and then describe the exact mistake that would silently turn it into inner-join behavior.
3. Explain how you would debug a report whose totals are too high without changing the SQL yet.

## Interview-Style Prompts

1. A teammate says “the join is correct, so the aggregate must be correct.” Explain why that reasoning is weak.
2. Explain to an interviewer how you decide between `COUNT(*)`, `COUNT(column)`, and `COUNT(DISTINCT ...)` in a real report query.

## Lesson Checkpoint

- choose the correct join type for one report and explain what row shape it guarantees
- show one aggregate that stays correct when comments are joined to posts
- translate the posts read path into the SQL row shape it promises to callers

## Next Lesson

Move to [Lesson 03](03-window-functions-and-intermediate-querying.md) when join correctness is no longer guesswork and you want analytical queries that keep row-level detail.