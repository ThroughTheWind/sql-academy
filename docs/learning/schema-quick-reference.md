# Schema Quick Reference

Use this page when you need the seeded table layout quickly without rereading the full fundamentals lesson.

It is a compact first-lessons reference, not a substitute for [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md).

Keep [Field Type Selection Guide](field-type-selection-guide.md) nearby when the table purpose is clear but the column type choice is not.

## The Six Seeded Tables

| Table | Purpose | Most Important Columns |
| --- | --- | --- |
| `academy.Users` | application users | `Id`, `UserName`, `Email`, `CreatedUtc` |
| `academy.Instruments` | tradable instruments | `Id`, `Symbol`, `AssetClass`, `TickSize`, `LotSize` |
| `academy.Posts` | user-authored posts | `Id`, `UserId`, `Title`, `CreatedUtc` |
| `academy.Comments` | comments on posts | `Id`, `PostId`, `UserId`, `CreatedUtc` |
| `academy.Orders` | user orders | `Id`, `UserId`, `OrderNumber`, `Status`, `TotalAmount`, `UpdatedUtc` |
| `academy.Trades` | executed trades | `Id`, `UserId`, `InstrumentId`, `Side`, `Quantity`, `Price`, `TradedUtc` |

## Relationship Map

```text
academy.Users
  -> academy.Posts.UserId
  -> academy.Comments.UserId
  -> academy.Orders.UserId
  -> academy.Trades.UserId

academy.Posts
  -> academy.Comments.PostId

academy.Instruments
  -> academy.Trades.InstrumentId
```

## Seeded Facts Worth Memorizing Early

- users: `ada`, `grace`, `linus`, `margaret`
- instruments: `MSFT`, `AAPL`, `EURUSD`, `CL`
- posts cover clustered indexes, window functions, concurrency, and release playbooks
- one seeded post currently has no comments, which makes it useful for `LEFT JOIN` reasoning
- orders include one `Filled`, one `Submitted`, and one `Pending` example

## First Queries That Build Intuition

```sql
SELECT Id, UserName, Email, CreatedUtc
FROM academy.Users
ORDER BY Id;

SELECT Id, Title, CreatedUtc
FROM academy.Posts
ORDER BY CreatedUtc DESC, Id DESC;

SELECT p.Id, p.Title, c.Id AS CommentId
FROM academy.Posts AS p
LEFT JOIN academy.Comments AS c
    ON c.PostId = p.Id
ORDER BY p.Id, c.Id;
```

## What To Notice

- `academy.Users` and `academy.Instruments` provide the reference entities for most later work
- `academy.Posts` plus `academy.Comments` are the safest early join path because you can reason about the rows by hand
- `academy.Orders` is the easiest early write-safety surface because it includes deterministic ordering and visible status transitions
- `academy.Trades` becomes more important once you reach window functions, pagination, indexing, and Dapper query shape

## Best Companions

1. [SQL-First Day One](sql-first-day-one.md)
2. [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md)
3. [Field Type Selection Guide](field-type-selection-guide.md)
4. [Lesson 02: Joins And Aggregations](02-joins-and-aggregations.md)
5. [Beginner 000: SQL Fundamentals And Safe Changes](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)