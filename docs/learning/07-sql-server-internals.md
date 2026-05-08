# Lesson 07: SQL Server Internals

## Focus

Build the minimum internal model needed to debug non-obvious database behavior. The goal is not to become a storage-engine specialist. The goal is to stop treating the database as a black box when performance or concurrency symptoms appear.

## Before You Start

- finish [Lesson 06](06-indexing-execution-plans-and-parameter-sensitivity.md) so plans, reads, and parameter-sensitive behavior already feel concrete
- keep [Learning Glossary](glossary.md) open if terms such as cardinality, memory grant, spill, or wait are still slow
- expect hypothesis-driven debugging, not trivia collection

## Suggested Time Budget

- 75 to 120 minutes to read the lesson, inspect one realistic query path, and compare your prediction with the observed plan or waits
- longer if internals is your first exposure to engine-level reasoning

## If You Get Stuck Early

- start with one symptom, one likely internal cause, and one check that could disprove it
- do not try to memorize every DMV before you can explain what kind of evidence you actually need
- use [Advanced 003: Plan Cache, Memory Grants, And Wait Signals](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) only after plan reuse, row estimates, and waits already make sense at a high level

## Why This Lesson Matters

Without internal intuition, performance debugging turns into guesswork. You see “slow query” and jump between random ideas:

- maybe CPU is high
- maybe it is I/O
- maybe it is parameter sniffing
- maybe it is blocking

Internal models let you form and test better hypotheses. You do not need perfect theory. You need enough theory to ask better questions.

## Repository Anchors

- [Phase 6: SQL Server Internals](../phases/phase-6-sql-server-internals.md)
- [Performance lab notes](../../db/performance/README.md)
- [Larger-cardinality workload variant](../../db/performance/001_high_cardinality_workload_variant.sql)
- [Trade Dapper read path](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)
- [Performance comparison tests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs)
- [Plan cache lab smoke test](../../tests/SqlAcademy.PerformanceTests/PlanCacheLabSmokeTests.cs)
- [Advanced 003: Plan Cache, Memory Grants, And Wait Signals](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)

## The Internal Model You Actually Need

You should be able to reason about four layers:

1. storage and access paths
2. optimization and plan cache behavior
3. memory usage and spill risk
4. waits and scheduler pressure

That is enough to explain a large percentage of production symptoms.

## Storage Intuition

SQL Server stores data in pages. The practical consequence is simple: retrieving rows is not a purely abstract operation. Physical layout, page density, and access path shape influence I/O behavior.

Useful mental shortcuts:

- sequential access patterns are often cheaper than random lookups
- clustered key choice influences row organization
- nonclustered lookups can become expensive when many rows must jump back to the base data
- wide rows reduce page density and can increase I/O

You do not need to reason at byte level for most tasks, but you do need to stop imagining data access as costless.

## The Optimizer And Compilation

When you submit a query, SQL Server does not blindly execute the text. It compiles a plan based on the query shape, metadata, and cardinality estimates.

That means performance can be influenced by:

- statistics quality
- parameter values at compile time
- available indexes
- query predicates and joins
- cached plan reuse

This is why two logically identical requests can run very differently depending on compilation context.

## Plan Cache Behavior

The plan cache exists so SQL Server can reuse compiled plans instead of recompiling everything repeatedly. Reuse is usually good, but it creates tradeoffs:

- a reused plan saves compile cost
- a reused plan may fit one parameter pattern poorly

That is the bridge between internals and the previous lesson on parameter sensitivity.

Useful questions:

- is the current plan reused or recompiled often?
- was the plan compiled under unusual parameter values?
- does the estimated row count reflect reality?

## Cardinality Estimates Matter

Many downstream choices depend on estimated row counts:

- join strategy
- memory grant size
- whether a seek or scan looks attractive
- whether a sort or hash operation seems affordable

If estimates are badly wrong, the chosen plan can be bad even when the SQL text is reasonable.

## Memory Grants And Spills

Operations such as sorts and hash joins often need workspace memory. SQL Server estimates how much memory the query will need and grants a budget.

Two failure modes matter:

- too little memory, causing spills to tempdb
- too much memory, causing unnecessary pressure on concurrent workloads

When you see spill symptoms, ask:

- was the row estimate wrong?
- is the sort/hash unavoidable?
- would a better index reduce or eliminate the memory-heavy operator?

## Waits As A Debugging Signal

Wait stats help you ask what the engine was waiting on instead of only how long something took.

At a high level, waits can suggest pressure from:

- locks and blocking
- I/O
- CPU scheduling
- memory
- network or client consumption

Waits are signals, not verdicts. They become useful when combined with plan shape and workload context.

## A Practical Hypothesis Loop

When a query is slow, use this loop:

1. state the symptom precisely
2. propose one likely cause
3. choose one piece of evidence that could disprove it
4. inspect plan shape, waits, and measurements together
5. update the hypothesis instead of defending the first idea emotionally

Example:

- symptom: trade query is slow only for broad parameter values
- initial theory: scan-heavy plan due to low selectivity
- disconfirming check: compare plan and estimates for selective versus non-selective input

That is much better than randomly adding indexes.

## Useful Evidence Surfaces

Common practical tools include:

- actual execution plans
- `SET STATISTICS IO, TIME ON`
- DMVs such as `sys.dm_exec_query_stats`
- DMVs such as `sys.dm_exec_cached_plans`
- waits via `sys.dm_os_wait_stats` or live session views

You do not need to memorize every DMV column. What matters is understanding what kind of evidence each surface provides.

## Connect Internals Back To Repository Work

The repository gives you small but realistic read paths and tests. That is enough to practice internal reasoning.

Use the trade query or post query and ask:

- what plan would I expect for a highly selective filter?
- what plan would I expect for a broad scan?
- where would a sort require memory?
- where could row estimation be difficult?

Then use the performance tests as a controlled environment for comparing behavior instead of inventing a synthetic story detached from the codebase.

## Optional Workload Growth

If the baseline seed is too small to make row-estimation, memory-grant, or plan-cache behavior obvious, apply [Larger-cardinality workload variant](../../db/performance/001_high_cardinality_workload_variant.sql) and rerun the same plan, DMV, benchmark, or guided-lab workflow.

The goal is not random volume. The goal is to keep query shape fixed while giving the optimizer and memory-heavy operators a more realistic rowset to reason about.

## Common Failure Modes

### Treating Percentage Cost As Ground Truth

Plan percentages are hints, not a complete performance explanation.

### Looking At Waits Without Workload Context

The same wait type can be benign or problematic depending on volume, duration, and surrounding evidence.

### Explaining Everything As I/O Or Everything As CPU

Real problems often combine estimation, memory, access path, and waiting.

### Changing Multiple Variables At Once

If you change query text, indexes, and server settings simultaneously, you lose the ability to explain causality.

## A Good Practice Sequence

1. Pick a query path from the repo.
2. Predict whether the dominant cost should be seek/scan, sort, join, memory, or waiting.
3. Inspect the plan and compare it with the prediction.
4. Capture reads and time.
5. Write down one hypothesis that survived and one that was disproved.

## Exit Criteria

You are ready for Lesson 08 when you can do all of the following:

- explain how storage layout and access paths influence query cost at a practical level
- describe why compilation and plan reuse can change performance behavior
- explain what a memory grant is and why spills happen
- use waits as a debugging clue instead of a standalone explanation
- form a falsifiable performance hypothesis before changing code or indexes

## Review Questions

1. Why do storage layout and access paths matter even if the SQL text is logically correct?
2. Why can plan reuse be helpful and risky at the same time?
3. What problem does a memory grant try to solve during query execution?
4. Why are waits useful signals but not complete explanations by themselves?
5. What makes a performance hypothesis falsifiable instead of vague?

## Suggested Answers

1. They determine how many pages the engine reads and how efficiently it reaches the needed rows, which directly affects cost beyond logical correctness.
2. Reuse saves compilation work, but a reused plan that was compiled for one parameter pattern may perform badly for a very different one.
3. It reserves working memory for operators such as sorts and hash joins so the query can execute without spilling unnecessarily to tempdb.
4. Waits tell you what the engine was waiting on, but they need context from plans, row counts, and workload behavior to explain the real root cause.
5. A falsifiable hypothesis names a likely cause and identifies evidence that could prove it wrong, rather than just guessing that “SQL Server is slow.”

## Challenge Questions

1. Build a debugging hypothesis for a slow query that alternates between fast and slow runs, and describe one concrete check that could disprove your first theory.
2. Explain how a bad cardinality estimate can lead to both memory and access-path problems in the same execution.
3. Describe a case where wait analysis would point you in the wrong direction if you looked at it without plan context.

## Interview-Style Prompts

1. Explain to an interviewer how much SQL Server internals knowledge an application engineer actually needs, and why.
2. Defend the statement “internal models are valuable because they improve debugging questions, not because they make you memorize implementation trivia.”

## Model Answer Rubrics

### Challenge Questions

A strong set of challenge answers should include:

- one falsifiable hypothesis with a concrete disconfirming check
- a connection between cardinality, memory use, and operator choice instead of treating them as isolated topics
- an explanation of how wait evidence can mislead when divorced from plan context

### Interview-Style Prompts

A strong spoken answer should include:

- a pragmatic boundary for how much internals knowledge is needed to debug real application issues
- the idea that internal models improve diagnosis quality rather than serving as trivia collection
- at least one example of an engine concept leading to a better debugging question or experiment

## Lesson Checkpoint

- explain why stale statistics or bad cardinality estimates can distort an otherwise reasonable query
- distinguish waits, logical reads, and tempdb spills at a high level
- name one internal signal you would inspect before guessing at a fix

## Next Lesson

Move to [Lesson 08](08-operational-engineering-and-release-safety.md) once you can connect symptoms to likely engine behavior and want to manage change safely in a live system.