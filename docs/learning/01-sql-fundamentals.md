# Lesson 01: SQL Fundamentals

## Focus

Build correctness before speed. This lesson gives you a working mental model of the `academy` schema, the seeded rows, and the core SQL operators that later lessons assume you already trust.

## Before You Start

- complete [Phase 0: Local Setup](../phases/phase-0-local-setup.md) so you can reach `LearningDb` reliably
- keep [Learning Glossary](glossary.md) open if terms such as predicate, projection, or deterministic order are not yet automatic
- plan to stay read-only until the result shape of a query feels predictable

## Suggested Time Budget

- 60 to 90 minutes to read the lesson and run the baseline inspection queries
- extra time if SQL syntax itself is still unfamiliar or if you need to stop and inspect the seed data carefully

## If You Get Stuck Early

- stop at read-only `SELECT` queries and explain the expected result before you run anything else
- compare your mental model against the seed files linked below instead of guessing
- use [Beginner 000: SQL Fundamentals And Safe Changes](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) only after the table purposes and basic ordering rules already make sense

## Why This Lesson Matters

Most later SQL mistakes are not advanced mistakes. They usually begin with one of these failures:

- assuming row order without an `ORDER BY`
- updating more rows than intended because the predicate is weak
- inserting data without understanding uniqueness or foreign-key rules
- reading a table as if it were an object collection instead of a set

If you cannot predict what a basic `SELECT`, `INSERT`, `UPDATE`, or `DELETE` will do before you run it, tuning and ORM abstractions will only hide the confusion.

## Repository Anchors

- [Phase 1: SQL Fundamentals](../phases/phase-1-sql-fundamentals.md)
- [LearningDb schema bootstrap](../../db/schemas/001_create_learning_db.sql)
- [Reference seed data](../../db/seed/001_seed_reference_data.sql)
- [Social and order seed data](../../db/seed/002_seed_social_and_orders.sql)
- [How To Start](how-to-start.md)
- [Beginner 000: SQL Fundamentals And Safe Changes](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)

## The Domain You Are Querying

The seeded database is intentionally small enough to reason about by hand. You should know the purpose of each table before you write joins.

| Table | Purpose | Important Columns |
| --- | --- | --- |
| `academy.Users` | application users | `Id`, `UserName`, `Email`, `CreatedUtc` |
| `academy.Instruments` | tradable instruments | `Id`, `Symbol`, `AssetClass`, `TickSize`, `LotSize` |
| `academy.Posts` | user-authored posts | `Id`, `UserId`, `Title`, `Body`, `CreatedUtc` |
| `academy.Comments` | comments on posts | `Id`, `PostId`, `UserId`, `Body`, `CreatedUtc` |
| `academy.Orders` | user orders | `Id`, `UserId`, `OrderNumber`, `Status`, `TotalAmount`, `RowVersion` |
| `academy.Trades` | executed trades | `Id`, `UserId`, `InstrumentId`, `Side`, `Quantity`, `Price`, `TradedUtc` |

The main relationships are:

- one user can author many posts
- one post can have many comments
- one user can place many orders
- one user can execute many trades
- one instrument can appear in many trades

## Mental Models To Keep

### Think In Sets, Not Rows

SQL is declarative. You describe the result you want, not the loop that should produce it. The database engine decides how to retrieve the data.

### Tables Have No Natural Order

Rows are not guaranteed to come back in insertion order, primary-key order, or any order that feels intuitive. If order matters, state it.

### Constraints Are Part Of The Data Model

Primary keys, foreign keys, unique indexes, defaults, and data types are not optional metadata. They are active rules that protect correctness.

### Prediction Comes Before Execution

Before you run a modifying statement, you should be able to answer:

- which rows will be read
- which rows will change
- which constraints could reject the change
- how you will verify the result

## Core Syntax You Should Be Fluent With

### Projection

Projection means choosing which columns to return.

```sql
SELECT Id, UserName, Email
FROM academy.Users;
```

Avoid `SELECT *` while learning. Explicit projection forces you to understand the shape of the result.

### Filtering

Filtering narrows the result set.

```sql
SELECT Id, UserName, CreatedUtc
FROM academy.Users
WHERE UserName = N'ada';
```

Use predicates that match the business question exactly. A weak predicate is how accidental bulk updates happen.

### Stable Ordering

```sql
SELECT Id, Title, CreatedUtc
FROM academy.Posts
ORDER BY CreatedUtc DESC, Id DESC;
```

Add a tiebreaker when timestamps or other sort columns can repeat. That habit matters even in simple lessons because it becomes essential in pagination later.

### Top-N Queries

```sql
SELECT TOP (2) Id, OrderNumber, Status, CreatedUtc
FROM academy.Orders
ORDER BY CreatedUtc DESC, Id DESC;
```

`TOP` without `ORDER BY` answers an incomplete question.

## Learn The Schema By Querying It

Start with read-only inspection.

```sql
SELECT Id, UserName, Email, CreatedUtc
FROM academy.Users
ORDER BY Id;

SELECT Id, Symbol, AssetClass, TickSize, LotSize
FROM academy.Instruments
ORDER BY Id;

SELECT Id, UserId, Title, CreatedUtc
FROM academy.Posts
ORDER BY CreatedUtc, Id;
```

Then ask yourself:

- which columns look like natural identifiers to a human?
- which columns are surrogate identifiers for the database?
- which columns are probably searched or sorted often?
- which columns are controlled by the application, and which have defaults?

## Keys, Uniqueness, And Referential Integrity

### Primary Keys

Every table in this schema has an integer identity primary key. That gives the database a stable internal identifier even when business values change.

### Unique Constraints And Indexes

The schema enforces uniqueness where duplicate business values would be invalid:

- `academy.Users.UserName` is unique
- `academy.Users.Email` is unique
- `academy.Instruments.Symbol` is unique
- `academy.Orders.OrderNumber` is unique

That means the database, not just the application, prevents duplicates.

### Foreign Keys

Foreign keys tie dependent rows to parent rows. For example:

- `academy.Posts.UserId` must reference an existing user
- `academy.Comments.PostId` must reference an existing post
- `academy.Trades.InstrumentId` must reference an existing instrument

If you insert a child row that points to a non-existent parent, SQL Server rejects it.

## Safe Data Modification Basics

### Inserts

```sql
INSERT INTO academy.Users (UserName, Email, CreatedUtc)
VALUES (N'edith', N'edith@sqlacademy.local', SYSUTCDATETIME());
```

Predict first:

- will the unique indexes allow it?
- do required columns have values?
- does a default exist for omitted columns?

Now try an insert that should fail.

```sql
INSERT INTO academy.Users (UserName, Email, CreatedUtc)
VALUES (N'ada', N'ada-duplicate@sqlacademy.local', SYSUTCDATETIME());
```

That should be rejected because `UserName` is unique.

### Updates

Start by previewing the target rows.

```sql
SELECT Id, OrderNumber, Status, UpdatedUtc
FROM academy.Orders
WHERE OrderNumber = N'ORD-2025-0002';
```

Then perform the update.

```sql
UPDATE academy.Orders
SET Status = N'Filled',
	UpdatedUtc = SYSUTCDATETIME()
WHERE OrderNumber = N'ORD-2025-0002';
```

Then verify:

```sql
SELECT Id, OrderNumber, Status, UpdatedUtc
FROM academy.Orders
WHERE OrderNumber = N'ORD-2025-0002';
```

The discipline is simple: preview, modify, verify.

### Deletes

Delete statements deserve the same care as updates. Preview the exact rows first.

```sql
SELECT Id, Title
FROM academy.Posts
WHERE Title = N'Temporary post';

DELETE FROM academy.Posts
WHERE Title = N'Temporary post';
```

In this schema, deleting a post will cascade to its comments because `academy.Comments.PostId` is defined with `ON DELETE CASCADE`. That is a model rule, not an application convention.

## Read The Seed Data Like A Test Fixture

The seed scripts tell you what facts later exercises are built on.

- users include `ada`, `grace`, `linus`, and `margaret`
- instruments include `MSFT`, `AAPL`, `EURUSD`, and `CL`
- posts already cover clustered indexes, window functions, concurrency, and operational playbooks
- orders and trades give you enough variation to practice filtering and sorting

This matters because you should be able to predict at least part of the result set without guessing.

For example:

```sql
SELECT UserName, Email
FROM academy.Users
WHERE UserName IN (N'ada', N'grace')
ORDER BY UserName;
```

If you do not already expect two rows, you have not yet internalized the seed data.

## Common Beginner Failure Modes

### Missing `ORDER BY`

If you say “latest orders” but do not sort by `CreatedUtc` and a stable tiebreaker, the query does not actually express “latest.”

### Updating Too Broadly

This is wrong unless you intend to update every row:

```sql
UPDATE academy.Orders
SET Status = N'Filled';
```

The absence of a `WHERE` clause is the problem, not SQL Server.

### Confusing `NULL` With Empty Text

The schema uses required columns heavily. Do not assume nullable behavior where the model says values are mandatory.

### Assuming Constraints Are Only In Application Code

If the database rejects a write, that is usually evidence that the schema is protecting something real. Read the error and identify which rule fired.

## A Good Study Loop For This Lesson

1. Inspect all six tables with small `SELECT` queries.
2. Write one filtered query per table.
3. Write one explicitly ordered query per table where ordering matters.
4. Insert a valid row into a safe practice target.
5. Attempt one invalid insert to trigger a uniqueness or foreign-key failure.
6. Update one known order and verify the exact row changed.
7. Explain every result before moving on.

## Exit Criteria

You are ready for Lesson 02 when you can do all of the following without guesswork:

- explain why `academy.Users.Email` and `academy.Users.UserName` are unique
- describe the relationship between posts and users, and between trades and instruments
- write a stable `ORDER BY` instead of assuming row order
- preview the target rows of an `UPDATE` before executing it
- identify whether an `INSERT` would fail because of a required value, uniqueness, or a foreign key

## Review Questions

1. Why is `SELECT *` a weak default while learning SQL fundamentals?
2. Why is `ORDER BY CreatedUtc DESC` often still incomplete on its own?
3. What is the practical difference between a primary key and a unique business column such as `Email`?
4. Why should you preview rows with a `SELECT` before running an `UPDATE` or `DELETE`?
5. What kind of error should you expect if you insert a post with a `UserId` that does not exist in `academy.Users`?

## Suggested Answers

1. `SELECT *` hides the real shape of the result, pulls columns you may not need, and makes it harder to reason about what the query is actually returning.
2. Timestamps can tie, so ordering only by `CreatedUtc` may still leave the final row order unstable; adding a tiebreaker such as `Id` makes the result deterministic.
3. The primary key gives the database a stable internal identity, while a unique business column prevents invalid duplicates in a real-world field that users or systems care about.
4. Previewing first lets you confirm the exact target set before changing data, which reduces the chance of broad accidental writes.
5. SQL Server should reject the insert with a foreign-key violation because the child row would reference a parent row that does not exist.

## Challenge Questions

1. Write a query that returns the two most recent orders with deterministic ordering and explain why your `ORDER BY` is complete.
2. Design a safe insert test that proves both uniqueness and foreign-key enforcement without damaging the seeded data.
3. Explain why a table can be logically correct even when a beginner still cannot query it safely.

## Interview-Style Prompts

1. Explain to a new teammate why `SELECT *` and missing `ORDER BY` are not harmless shortcuts in a production codebase.
2. Defend the statement “constraints are part of application correctness, not just database decoration” using examples from this repository.

## Model Answer Rubrics

### Challenge Question 1

A strong answer should include:

- an explicit `ORDER BY` on the business sort column and a stable tiebreaker such as `Id`
- a short explanation of why tied timestamps make the result unstable without the tiebreaker
- evidence that the query is limited after ordering, not before it

### Challenge Question 2

A strong answer should include:

- one valid insert attempt and one invalid attempt that targets a real uniqueness or foreign-key rule
- a safety note about using temporary or rolled-back test rows instead of damaging seed expectations
- a prediction of the exact rule each insert is expected to satisfy or violate

### Challenge Question 3

A strong answer should include:

- a distinction between schema correctness and query fluency
- at least one example where a correct table design can still be queried unsafely through weak predicates or unstable ordering
- a statement that correctness depends on both model rules and operator discipline

### Interview Prompt 1

A strong answer should include:

- why `SELECT *` hides shape and can encourage over-fetching
- why missing `ORDER BY` is a correctness problem rather than only a style preference
- at least one concrete production consequence from this repository context, such as unstable pagination or ambiguous “latest” results

### Interview Prompt 2

A strong answer should include:

- at least one uniqueness example and one foreign-key example from the seeded schema
- the idea that database constraints protect correctness even when application code is wrong or incomplete
- a clear distinction between schema rules and optional developer convention

## Lesson Checkpoint

- explain why deterministic ordering needs a tiebreaker before you trust a result
- point to one uniqueness rule and one foreign-key rule in the seeded model and describe how each would fail
- write a preview `SELECT` for any row set you would change with `UPDATE` or `DELETE`

## Next Lesson

Move to [Lesson 02](02-joins-and-aggregations.md) once single-table reasoning feels routine and you can predict row shape before you run the query.