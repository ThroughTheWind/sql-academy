# Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity

## Focus

Tune with evidence. This lesson is about rejecting folklore and using workload shape, execution plans, logical reads, and parameter behavior to make decisions you can defend.

## Before You Start

- be comfortable with stable ordering, query shape, and the migration-safety mindset from the earlier lessons
- keep [Learning Glossary](glossary.md) open if terms such as selectivity, covering index, logical reads, or parameter sensitivity are still slow
- expect to explain the current query shape before you propose any index or hint

## Suggested Time Budget

- 75 to 120 minutes to read the lesson, inspect one real query path, and compare at least one before-and-after measurement
- longer if execution plans and logical reads are newer than the SQL syntax itself

## If You Get Stuck Early

- restate the filter, join, sort, and projection shape in plain English before you read the plan
- separate three questions: is the SQL shape weak, is the index support weak, or is the plan unstable for one parameter pattern
- use [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) only after you can name the query shape and one metric you intend to compare

## Why This Lesson Matters

Performance work goes wrong when people jump straight to solutions:

- “add an index”
- “use Dapper”
- “force recompilation”
- “SQL Server picked a bad plan”

Those statements may be directionally true sometimes, but they are not analysis. This lesson teaches the order of operations you should trust:

1. understand the query shape
2. observe the current plan and cost drivers
3. measure reads and duration
4. change one thing for a reason
5. verify the improvement and its tradeoff

## Repository Anchors

- [Phase 5: Indexing And Performance](../phases/phase-5-indexing-and-performance.md)
- [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- [Trade Dapper read path](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)
- [Post EF Core read path](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs)
- [Query performance comparison tests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs)
- [Query Store smoke test](../../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs)
- [Tracking benchmarks](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs)
- [Larger-cardinality workload variant](../../db/performance/001_high_cardinality_workload_variant.sql)

## Mental Models To Keep

### Indexes Serve Workloads, Not Aesthetic Preferences

The best index is the one that supports real filters, joins, and orderings at acceptable write cost.

### Execution Plans Tell You How SQL Server Chose To Satisfy The Query

The plan is evidence about the engine’s chosen strategy, not a moral judgment about your SQL.

### A Fast Plan For One Parameter Can Be Terrible For Another

Queries with very different selectivity across parameter values are common sources of unstable performance.

### Wide Indexes Are Not Free

Every extra indexed column increases storage, maintenance work, and write cost.

## Start With Query Shape

Before you design an index, write down:

- which columns are used for filtering
- which columns are used for joining
- which columns determine sort order
- which columns are only needed for projection
- whether the query returns many rows or only a small page

Example from the trade read path:

- optional filters on `UserId` and instrument symbol
- joins from trades to users and instruments
- dynamic ordering by traded time, price, quantity, or symbol
- pagination via `OFFSET/FETCH`

That is enough to predict that no single index will be ideal for every possible sort and filter combination.

## Clustered And Nonclustered Indexes

You should understand the distinction conceptually.

### Clustered Index

Defines the row storage order for the table. SQL Server stores the table data according to the clustered key.

### Nonclustered Index

Stores a separate structure keyed by indexed columns and points back to the base row.

Practical implication:

- a seek on a selective nonclustered index can be very efficient
- but if many extra columns are needed, lookups back to the base table can dominate cost

## Index Design By Filter And Order

An index tends to help most when it matches how the query narrows and orders rows.

For example, the schema already includes indexes such as:

- `IX_Trades_UserId_TradedUtc`
- `IX_Trades_InstrumentId_TradedUtc`
- `IX_Posts_UserId_CreatedUtc`

Those are not random. They encode likely access patterns such as “recent trades for a user” or “recent posts for an author.”

## Covering Indexes And Tradeoffs

A covering index contains enough information for the query to avoid extra lookups to the base row, either through key columns or included columns.

That can be powerful, but ask first:

- how often is the query executed?
- how many rows are returned?
- what is the write penalty?
- can projection be narrowed instead?

Do not default to giant “cover everything” indexes. They often help one read path while making writes and maintenance worse.

## Sargability Matters More Than Many People Admit

A query is easier to optimize when the engine can use indexed values directly.

Patterns that commonly reduce sargability include:

- wrapping indexed columns in functions
- non-selective optional predicate patterns
- mismatched data types that force implicit conversion

You should always ask whether the predicate shape is helping or hindering the optimizer before blaming the engine.

## Read The Existing Query Shapes

In [Post EF Core read path](../../src/libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), the query:

- starts from `Posts`
- optionally filters by author
- optionally searches title, body, or author username
- applies stable sorting
- pages results
- projects to a lightweight read model

The teaching point is not “EF Core is slow” or “EF Core is fast.” The point is that query shape still matters even when written in LINQ.

In [Trade Dapper read path](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs), the SQL is explicit, but the same questions apply:

- which parameters can radically change selectivity?
- which sort paths are common enough to optimize directly?
- what happens to the plan when optional filters are null versus highly selective?

## Execution Plans: What To Look For First

Do not start by chasing every icon in the plan. Start with a few high-value questions:

1. Is the engine scanning a large structure when a seek would be more appropriate?
2. Is there an explicit sort that a better index could avoid?
3. Are key lookups dominating cost because projection is wider than necessary?
4. Are estimated rows very different from actual rows?
5. Is the query memory-intensive or spilling because of sort/hash work?

These questions usually tell you more than staring at percentage costs.

## Measure Reads And Duration

Useful tuning requires measurement. Common signals include:

- elapsed time
- CPU time
- logical reads
- returned row count
- plan shape before and after the change

A change that reduces runtime by chance on one run but doubles logical reads is not automatically a win.

## Parameter Sensitivity

Parameter-sensitive behavior appears when one cached plan fits one parameter value well but fits another badly.

Typical example:

- a rare `UserId` value benefits from a selective seek plan
- an unfiltered or very common value benefits from a scan-oriented plan
- one parameter set compiles the plan for both cases

This is why “it is fast for me but slow for production traffic” is such a common report.

## Query Store Adds Persisted Regression Evidence

Execution plans, runtime measurements, and one local run are still the starting point.

But some performance questions are temporal rather than momentary:

- did this query have more than one plan over time?
- did the same query regress after a deploy or data-shape change?
- am I looking at a current in-memory plan, or at a persisted history of runtime behavior?

That is where Query Store becomes useful. It complements plan reading and parameter-sensitivity reasoning by preserving query and plan history long enough to compare regressions instead of guessing at them.

Use [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) after the main indexing pack when you want persisted evidence for one tagged query rather than only the current in-memory story.

## Optional Larger-Cardinality Workload

The baseline seed keeps setup fast, but some statistics, parameter-sensitivity, and regression questions become clearer when the posts and trades tables are materially larger.

Use [Larger-cardinality workload variant](../../db/performance/001_high_cardinality_workload_variant.sql) after the main pack when you want more realistic row counts without changing the default seed used by tests and validation packs.

Then rerun the same surface you were already using, such as [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md), [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md), [Query performance comparison tests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs), or [Tracking benchmarks](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs), so the main variable that changed is data shape.

## How The Repository Encourages This Topic

The trade query accepts optional filters and multiple sort modes. That is realistic and useful for learning because different parameter shapes can naturally lead to different optimal strategies.

When testing parameter sensitivity, change one input at a time:

- `@UserId` null versus a specific user
- `@InstrumentSymbol` null versus a specific instrument
- common sort mode versus less common sort mode

Then compare:

- did the plan shape change?
- if not, should it have?
- if yes, was the new plan better only for the current parameter?

## Index Design Checklist

Before adding an index, ask:

- what query or family of queries is this for?
- which predicates and orderings does it support?
- is the table write-heavy or read-heavy?
- will this index duplicate another index too closely?
- can I justify the maintenance cost?

If you cannot answer those questions, the index is probably premature.

## Common Failure Modes

### Adding Wide Indexes Without A Workload Story

An index should have a named consumer, not just a vague hope.

### Confusing ORM Choice With Query Quality

Dapper does not rescue a poor SQL shape. EF Core does not guarantee a poor one.

### Measuring Only Duration

Short-term runtime can fluctuate. Reads and plan shape give more durable evidence.

### Treating Parameter Sensitivity As Myth Or Excuse

It is real, but you still have to demonstrate it with parameter-specific evidence.

## A Good Practice Sequence

1. Take one query from the trade or post read path.
2. Write down its filters, joins, sort order, and projection.
3. Inspect the current plan and capture reads and duration.
4. Propose the narrowest useful index.
5. Re-test with different parameter values.
6. Explain whether the improvement is robust or parameter-specific.
7. If the question is about regression over time rather than one run, use Query Store to compare persisted plan and runtime history for the same tagged query.

## Exit Criteria

You are ready for Lesson 07 when you can do all of the following:

- explain why an index helps a query in terms of filter, join, or sort shape
- read an execution plan at a practical first-pass level
- compare before-and-after results with evidence instead of intuition
- describe why a very wide index is often a poor default answer
- demonstrate what makes a query parameter-sensitive or plan-unstable
- explain when Query Store is a better regression surface than plan cache or one-off runtime measurements

## Review Questions

1. Why should index design begin with query shape instead of with a generic indexing rule?
2. Why is a very wide covering index often the wrong first answer?
3. What makes a query parameter-sensitive?
4. Why are logical reads and plan shape more trustworthy than raw runtime alone?
5. What is the first practical question to ask when reading an execution plan?

## Suggested Answers

1. Indexes are only useful when they support real filters, joins, and orderings in the workload, so query shape tells you what access path is actually needed.
2. It may help one read path, but it also increases storage, write cost, and maintenance burden, so its total tradeoff can be worse than a narrower targeted index.
3. A query is parameter-sensitive when different parameter values make very different plan shapes optimal, so one cached plan fits some inputs badly.
4. Runtime can fluctuate because of noise, while logical reads and plan shape tell you more directly how much work the engine performed and why.
5. Start by asking whether the engine is scanning, sorting, or performing lookups in a way that does not match the intended filter and order pattern of the query.

## Challenge Questions

1. Take a paged read query from this repository and propose one index that helps it; then argue against your own proposal from a write-cost perspective.
2. Describe how you would prove parameter sensitivity with evidence instead of with intuition.
3. Explain when a query rewrite is a better answer than another index.

## Interview-Style Prompts

1. Explain to an interviewer how you would decide whether a slow query needs a new index, a query rewrite, or neither.
2. Defend the statement “performance tuning without before-and-after evidence is not engineering.”

## Model Answer Rubrics

### Challenge Questions

A strong set of challenge answers should include:

- a workload-shaped explanation of why the proposed index helps and what it costs
- one evidence plan for proving parameter sensitivity rather than asserting it vaguely
- a case where query simplification or projection narrowing could beat another index

### Interview-Style Prompts

A strong spoken answer should include:

- an ordered diagnostic approach that starts with query shape and evidence before proposing fixes
- deliberate use of reads, plan shape, and workload frequency as decision inputs
- a refusal to equate tool choice or folklore with proof of improvement

## Lesson Checkpoint

- identify the filter, sort, and projection columns in one read path before suggesting an index
- defend one narrow index and one reason not to make it wider
- explain how one cached plan can hurt a different parameter value
- explain when persisted query history changes the next debugging step

## Next Lesson

Move to [Lesson 07](07-sql-server-internals.md) when you can defend a tuning choice with evidence and want to connect symptoms to engine behavior.