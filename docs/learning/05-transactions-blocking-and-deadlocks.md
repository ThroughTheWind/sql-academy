# Lesson 05: Transactions, Blocking, And Deadlocks

## Focus

Learn to reason about system behavior under contention. This is the point where SQL correctness alone stops being enough. A query can be logically correct and still cause stalls, timeouts, or deadlocks in production.

## Why This Lesson Matters

Production systems fail under load in ways that do not show up in single-session testing. The most common concurrency problems are:

- transactions that stay open too long
- lock ordering that varies across code paths
- retry logic that hides bad transaction design
- write paths that silently overwrite concurrent changes

This lesson teaches you to separate three different concepts clearly:

- transaction correctness
- blocking behavior
- deadlock behavior

## Repository Anchors

- [Phase 4: Transactions And Concurrency](../phases/phase-4-transactions-and-concurrency.md)
- [Senior 001: Concurrency, Blocking, And Deadlocks](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
- [Senior 002: EF Core N+1, Optimistic Concurrency, And Bulk Ingestion](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
- [Order mapping with rowversion](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs)
- [Schema definition for `academy.Orders`](../../db/schemas/001_create_learning_db.sql)

## Mental Models To Keep

### A Transaction Is A Time Window, Not Just A Statement Group

The longer the transaction stays open, the longer locks may be held and the more likely it is to interfere with other work.

### Blocking Is Delay, Not Failure

Blocking means one session is waiting because another session holds a conflicting lock.

### Deadlocking Is Cyclic Blocking

Deadlock means sessions form a cycle where each waits on a resource the other holds. SQL Server resolves this by choosing a victim and aborting it.

### Retries Are A Recovery Tool, Not A Design Strategy

Automatic retry can be appropriate for deadlock victims or transient failures. It is not a substitute for fixing poor lock ordering or oversized transactions.

## Transaction Basics You Should Be Able To Explain

A transaction groups work into one atomic unit. In practical terms, that means:

- either all changes commit or none do
- reads and writes inside the transaction happen under a chosen isolation behavior
- locks can live for the duration of the transaction, not only the duration of a single statement

The operational question is not “should I use a transaction?” The real question is “how large should the transaction boundary be?”

## Isolation Levels As Tradeoffs

You do not need to memorize every internal detail on day one, but you do need the correct mental model.

### Read Committed

Default SQL Server posture in many systems. It prevents dirty reads but still allows many kinds of concurrency interaction. Readers and writers can still block each other depending on workload and settings.

### Snapshot-Based Approaches

Snapshot behavior reduces some reader-writer blocking by letting readers see a consistent versioned snapshot instead of waiting on current writers. That helps some workloads, but it is not a universal fix for all concurrency issues.

### Serializable

Stronger isolation can prevent anomalies but often increases locking and contention. Stronger correctness guarantees usually cost more concurrency.

The right question is always: what anomaly must this operation prevent, and what blocking cost are you willing to pay?

## Blocking: What It Is And Why It Happens

Blocking is normal in a locking database. The problem is not that blocking exists. The problem is when the duration or scope becomes harmful.

Common reasons blocking becomes painful:

- a transaction waits on user input or network I/O before commit
- a batch update touches many rows in one long unit of work
- indexes are missing, so a statement scans and locks more than expected
- the application starts a transaction earlier than necessary or commits later than necessary

## Reproduce A Simple Blocking Scenario

Session A:

```sql
BEGIN TRAN;

UPDATE academy.Orders
SET Status = N'Filled',
	UpdatedUtc = SYSUTCDATETIME()
WHERE OrderNumber = N'ORD-2025-0002';

-- Keep the transaction open on purpose.
```

Session B:

```sql
UPDATE academy.Orders
SET Status = N'Cancelled',
	UpdatedUtc = SYSUTCDATETIME()
WHERE OrderNumber = N'ORD-2025-0002';
```

Session B will wait because Session A still holds conflicting locks. That is blocking, not deadlocking.

Now ask:

- which session is blocked?
- which session is the blocker?
- how long has the blocking lasted?
- did the first transaction really need to stay open that long?

## Deadlocks: Cycles, Not Queues

A deadlock requires a cycle. Example shape:

Session A:

1. updates one row in `academy.Orders`
2. then tries to update a second row

Session B:

1. updates the second row first
2. then tries to update the first row

If both sessions hold one resource and wait on the other, SQL Server detects the cycle and chooses a victim.

That is different from plain blocking, where one session eventually completes and the waiting session continues.

## The Simplest Deadlock Prevention Habit

Acquire resources in a consistent order.

If every code path updates rows in the same logical order, the probability of a deadlock drops sharply. This is often the cheapest and most defensible fix.

## What Belongs In Retry Logic

Automatic retry is often appropriate for:

- deadlock victim errors
- short-lived transient connectivity failures
- carefully chosen idempotent operations

Retry is not a complete answer for:

- a consistently oversized transaction
- lock ordering that predictably deadlocks
- application logic that silently replays non-idempotent side effects

The right order is usually:

1. redesign the transaction if the pattern is structurally bad
2. keep retry for the remaining truly transient cases

## Optimistic Concurrency With `rowversion`

The repository models `academy.Orders.RowVersion`, and [Order mapping with rowversion](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs) marks it as a row version in EF Core.

Why that matters:

- the application can detect whether someone else changed the row since it was read
- conflicts become explicit instead of silently overwriting newer data
- this protects correctness without forcing pessimistic locks around user think time

Optimistic concurrency is especially useful when conflicts are relatively rare and long-held write locks would be too expensive.

## Blocking Versus Optimistic Concurrency

These are not the same tool.

- blocking and deadlocks describe lock behavior during concurrent work
- optimistic concurrency detects that a row changed between read and write

You can have optimistic concurrency checks and still suffer blocking from poorly scoped transactions.

## Signs Your Transaction Boundary Is Wrong

- the transaction includes external API calls or user confirmation
- the transaction loads much more data than it changes
- multiple unrelated operations are bundled “for convenience”
- deadlocks occur only on one hot code path that updates resources in mixed order
- retries succeed often enough to hide the root cause

## Practical Debugging Questions

When you hit contention, ask in this order:

1. What rows or objects are involved?
2. Which session is waiting?
3. Which session is holding the conflicting resource?
4. Is this simple blocking or a cycle?
5. Should the fix be smaller transactions, better indexing, consistent ordering, optimistic concurrency, retry logic, or a combination?

## Common Failure Modes

### Treating Every Wait As A Deadlock

If one session is simply waiting behind another, that is blocking.

### Leaving Transactions Open Across Non-Database Work

That inflates lock duration and punishes concurrency.

### Retrying Everything Blindly

Blind retry can hide design defects and amplify load during incidents.

### Ignoring Access Order

Many deadlocks exist because two paths touch the same resources in different sequences.

## A Good Practice Sequence

1. Reproduce a simple blocking case with two sessions.
2. Identify the blocker and the blocked session.
3. Reproduce a two-row deadlock by changing update order between sessions.
4. Propose a consistent lock-order redesign.
5. Explain where retry belongs and where it does not.
6. Inspect the order rowversion mapping and explain what correctness problem it solves.

## Exit Criteria

You are ready for Lesson 06 when you can do all of the following:

- distinguish blocking from deadlocking in one sentence
- explain how transaction duration influences contention
- describe one safe way to reduce deadlock probability
- explain why `rowversion` supports optimistic concurrency rather than lock avoidance
- decide whether a failure should be addressed with retry logic, transaction redesign, or both

## Review Questions

1. What is the difference between blocking and deadlocking?
2. Why does transaction duration matter as much as transaction correctness?
3. Why is consistent lock ordering such an effective deadlock reduction technique?
4. When does retry logic make sense, and when is it a weak substitute for redesign?
5. What correctness problem does `rowversion` help the application detect?

## Suggested Answers

1. Blocking is one session waiting behind another, while deadlocking is a cycle of sessions waiting on each other until SQL Server aborts a victim.
2. A long-running transaction can hold locks much longer, which increases contention and the chance of blocking or deadlocks even if the logic is otherwise correct.
3. Consistent ordering reduces circular waits because every code path tries to acquire the same resources in the same sequence.
4. Retry logic makes sense for transient failures such as deadlock-victim outcomes, but it is a poor substitute when the underlying transaction design is predictably unsafe.
5. It helps detect that another writer changed the row between read and write, so the application can avoid silently overwriting newer data.

## Challenge Questions

1. Design a two-session script sequence that creates blocking but not a deadlock, and explain the exact condition that keeps it from becoming a cycle.
2. Propose a deadlock-safe redesign for a workflow that updates two related rows and justify what belongs in lock ordering versus retry logic.
3. Explain how a transaction can be logically correct and still operationally harmful.

## Interview-Style Prompts

1. Explain to an interviewer why “just add retries” is often a weak answer to concurrency failures.
2. Defend the use of optimistic concurrency with `rowversion` in a system where conflicts are possible but not constant.

## Model Answer Rubrics

### Challenge Questions

A strong set of challenge answers should include:

- a clear separation between blocking behavior and true deadlock cycles
- one fix that belongs in transaction design and one that belongs in retry or recovery behavior
- explicit attention to transaction duration and access order rather than only statement correctness

### Interview-Style Prompts

A strong spoken answer should include:

- why blind retry can hide design defects instead of solving them
- when optimistic concurrency is preferable to holding locks across user or application think time
- a distinction between detecting conflicts and preventing all waiting

## Lesson Checkpoint

- explain the difference between blocking, deadlock, and optimistic concurrency conflict
- draw one deadlock risk caused by inconsistent access order
- say what belongs in retry logic and what must be fixed in the transaction design

## Next Lesson

Move to [Lesson 06](06-indexing-execution-plans-and-parameter-sensitivity.md) when you can reason about correctness under contention and want to tune with evidence instead of folklore.