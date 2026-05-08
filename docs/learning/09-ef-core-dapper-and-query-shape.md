# Lesson 09: EF Core, Dapper, And Query Shape

## Focus

Use EF Core and Dapper intentionally. The goal is not to pick a side. The goal is to understand the SQL behavior underneath each abstraction and choose the right tool for the job.

## Why This Lesson Matters

Application developers often argue about data-access libraries at the wrong level. The real questions are:

- what SQL shape does this code produce?
- does the query match the business result shape?
- how much control do I need over the generated SQL?
- does the code make concurrency, pagination, and projection explicit enough?

If you can answer those questions, the EF Core versus Dapper debate becomes much more practical and much less ideological.

## Before You Start

If Phase 8 feels like a context shift, read [From SQL To EF Core And Dapper](from-sql-to-efcore-and-dapper.md) first.

That bridge maps the earlier SQL lessons to the application query paths in this repository so you can keep reasoning from query shape rather than from library preference.

## Suggested Time Budget

- 75 to 120 minutes to read the lesson, inspect one EF Core path and one Dapper path, and restate the SQL contract each one is trying to express
- longer if generated SQL inspection and optimistic concurrency are newer than the earlier pure-SQL lessons

## If You Get Stuck Early

- translate the code back into base rowset, filters, sort order, pagination, and projection before you argue about libraries
- inspect one concern at a time: read shape, tracking, paging, or concurrency, rather than all of them at once
- return to [From SQL To EF Core And Dapper](from-sql-to-efcore-and-dapper.md) and use [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) only after the query contract is already clear

## Repository Anchors

- [From SQL To EF Core And Dapper](from-sql-to-efcore-and-dapper.md)
- [Phase 8: .NET And EF Core Integration](../phases/phase-8-dotnet-and-efcore-integration.md)
- [API startup](../../src/apps/SqlAcademy.Api/Program.cs)
- [EF Core post queries](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs)
- [Dapper trade queries](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)
- [Order mapping with rowversion](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs)
- [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
- [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md)

## The Right Mental Model

EF Core and Dapper are not database alternatives. They are different ways of expressing database work.

- EF Core gives you change tracking, mapping, LINQ composition, and convenient unit-of-work behavior.
- Dapper gives you explicit SQL with minimal abstraction and predictable text-level control.

Neither removes the need to understand:

- joins
- filtering
- projection
- sorting
- pagination
- concurrency
- transaction scope

## What This Repository Demonstrates

The repository uses a hybrid style for a reason:

- EF Core is used where entity mapping and composable LINQ are helpful
- Dapper is used where explicit read SQL and tight query-shape control are valuable

That is a realistic architecture. Mature systems often mix tools instead of forcing every workload through one abstraction.

## Read The EF Core Query Path Carefully

In [EF Core post queries](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), the service:

- starts from `dbContext.Posts`
- uses `AsNoTracking()` for a read-only query
- composes optional filters
- applies explicit sorting with deterministic tie-breakers
- projects directly to `PostListItem`

That is already a strong example of good query shape.

Important ideas in that code:

### `AsNoTracking()` For Read Models

If you are not updating the entities, tracking often adds overhead without benefit. The repository’s post read path models this correctly.

### Projection Matters

The query projects to a dedicated read model instead of materializing full entities and then reshaping them later. That reduces unnecessary data movement and keeps intent clear.

### Stable Sorting Matters In Application Code Too

The query uses explicit tie-breakers such as `ThenBy(post => post.Id)` or descending equivalents. That mirrors the stable-ordering lessons from earlier SQL work.

## Read The Dapper Query Path Carefully

In [Dapper trade queries](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), the SQL text is explicit.

That makes several things obvious at a glance:

- exact joins
- selected columns
- optional filters
- pagination strategy
- count query shape

The tradeoff is that you carry more manual responsibility for:

- SQL correctness
- parameter usage
- sorting safety
- duplication avoidance across similar queries

## EF Core Versus Dapper: Practical Decision Frame

Choose EF Core by default when:

- you are performing entity-centric writes
- the query is reasonably expressible in LINQ
- change tracking or relationship mapping helps more than it hurts
- consistency with the rest of the write model matters

Choose Dapper or explicit SQL when:

- the read path needs full control over SQL shape
- you want to hand-author joins, filters, and paging behavior directly
- the query is reporting-oriented and entity tracking adds no value
- the SQL is already easier to reason about than its LINQ equivalent

The key point is that “easier to reason about” is the real benchmark, not library loyalty.

## Query Shape Still Wins Or Loses Performance

The abstraction does not matter as much as the shape. Bad patterns stay bad across tools.

Examples:

- over-fetching columns
- missing stable sort order for pagination
- N+1 relationship access
- overly broad filters or weak search predicates
- hidden client-side work after materialization

The right habit is to translate code into SQL questions:

- what is the base rowset?
- what joins or relationship traversals occur?
- what columns are really needed?
- is paging performed in the database or in memory?

## The N+1 Problem

N+1 appears when the application issues one query for a list and then additional queries per item, often because related data is loaded lazily or fetched in a loop.

Why it hurts:

- more round trips
- more latency amplification
- more unpredictable scaling under larger result sets

How to recognize it:

- logs show repeated similar queries for one request
- response time grows disproportionately with item count
- code loops through entities and fetches related data one row at a time

The fix is usually to reshape the query, not to just add caching on top.

## Generated SQL Must Be Inspectable

For EF Core, you should inspect the generated SQL for important paths, especially when:

- joins are non-trivial
- search predicates are involved
- paging and sorting are combined
- aggregation or relationship counts are projected

Do not assume LINQ that looks elegant produces SQL that is appropriate for the workload.

For Dapper, inspection is simpler because the SQL is already explicit. The discipline then becomes reviewing the text with the same rigor you would apply to hand-written SQL.

## Guided Follow-Up Lab

Use [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) when you want to prove the current posts read path stays one-query, inspect the generated SQL directly, and build a disposable N+1 regression experiment with focused test coverage already in the repository.

## Concurrency Still Belongs In The Application Conversation

In [Order mapping with rowversion](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs), the `RowVersion` column is configured for optimistic concurrency.

That means application code has to respect concurrency explicitly:

- read the row
- attempt an update with the expected version
- detect when another writer changed it first
- resolve or surface the conflict intentionally

That is a perfect example of why database knowledge and application knowledge should not be separated.

## Pagination, Filtering, And Result Contracts

Look at both read paths in this repository and ask what contract they promise:

- a page number and page size
- deterministic sorting
- total count
- lightweight result shape

That contract drives the query implementation. If the query shape cannot support the contract efficiently, the contract should be reconsidered or the query redesigned.

## Common Failure Modes

### Arguing About Libraries Instead Of Query Shape

Library choice matters less than whether the query is well designed.

### Tracking Read Models Needlessly

Tracking adds cost when no update is planned.

### Hiding SQL Complexity Behind LINQ Fluency

Readable C# is not proof of efficient SQL.

### Treating Explicit SQL As Automatically Better

Hand-written SQL can still be unstable, over-broad, or poorly indexed.

## A Good Practice Sequence

1. Read the EF Core post query and restate its SQL intent in plain English.
2. Read the Dapper trade query and identify its filter, sort, and paging contract.
3. Explain why `AsNoTracking()` is correct for the post read path.
4. Use [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) to identify one location where an N+1 risk could appear in a future refactor and capture the generated-SQL evidence.
5. Explain how rowversion changes the write-path conversation.

## Exit Criteria

You are ready for Lesson 10 when you can do all of the following:

- choose between EF Core and Dapper based on query and update shape rather than preference
- explain how an application query maps to SQL behavior underneath
- identify where `AsNoTracking()` and direct projection are appropriate
- spot the shape of an N+1 problem before it becomes a production issue
- reason about optimistic concurrency as part of application design

## Review Questions

1. Why is EF Core versus Dapper the wrong first question for most data-access decisions?
2. Why is `AsNoTracking()` a good default for many read-only queries?
3. What does direct projection protect you from in a read path?
4. What is the practical shape of an N+1 problem?
5. Why does optimistic concurrency belong in the application design discussion, not only in the database discussion?

## Suggested Answers

1. The first question should be about query and update shape, because both tools can be good or bad depending on how clearly they express the required SQL behavior.
2. It avoids tracking overhead when the application does not intend to modify the returned entities, which keeps read models lighter and clearer.
3. It protects you from over-fetching and from materializing full entities when only a small result contract is actually needed.
4. One query loads a list, and then additional queries are triggered per item for related data, causing extra round trips and latency growth.
5. The application must detect and handle concurrent changes intentionally, so concurrency tokens only help if the write path respects them and reacts correctly to conflicts.

## Challenge Questions

1. Compare one read path in this repository as if you had to rewrite it in the other data-access style, and explain what would get clearer versus what would get harder.
2. Describe how you would detect an N+1 regression before users reported it.
3. Defend a hybrid EF Core plus Dapper architecture to a reviewer who wants one tool used everywhere.

## Interview-Style Prompts

1. Explain to an interviewer how you decide when explicit SQL is worth the extra ceremony.
2. Defend the statement “the right abstraction is the one that keeps SQL behavior understandable, not the one with the least code on screen.”

## Model Answer Rubrics

### Challenge Questions

A strong set of challenge answers should include:

- a concrete comparison of what becomes clearer or riskier when switching data-access style
- one realistic detection path for spotting N+1 behavior before user-visible failure
- a defense of hybrid architecture based on workload shape rather than tool loyalty

### Interview-Style Prompts

A strong spoken answer should include:

- a clear threshold for when explicit SQL control is worth the extra maintenance cost
- the idea that understandability includes generated SQL shape, paging behavior, projection, and concurrency handling
- a refusal to judge abstractions only by line count or fashion

## Lesson Checkpoint

- justify EF Core or Dapper from query shape instead of tool preference
- explain how projection and `AsNoTracking()` reduce read-path cost
- describe how the application should surface and handle a `rowversion` conflict

## Next Lesson

Move to [Lesson 10](10-observability-testing-and-performance-engineering.md) once you can reason about code and SQL together and want to validate those decisions with telemetry and tests.