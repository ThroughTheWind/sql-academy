# Deadlock Graph Lab

Use this walkthrough after you complete the validation-pack tables in `answer.sql`.

The goal is to move from "I know there was a cycle" to "I can read one deadlock graph, map it to the conflicting code paths, and decide what belongs in retry policy versus transaction redesign."

## Inputs

- [deadlock-graph-sample.xml](deadlock-graph-sample.xml)
- [starter.sql](starter.sql)
- [Lesson 05: Transactions, Blocking, And Deadlocks](../../../../docs/learning/05-transactions-blocking-and-deadlocks.md)

## What To Inspect First

1. `victim-list`: which process SQL Server chose to abort
2. `process-list`: the statements each process was running, especially `inputbuf` and `logused`
3. `resource-list`: which session owned each key lock and which session was waiting on it

## Tasks

1. Identify the deadlock victim and cite the graph field that proves it.
2. Map each process node back to Session A or Session B from `starter.sql` by using the `inputbuf` text and the resource it already owns.
3. For each resource node, write down the object name, the owner process, the waiter process, and the requested lock mode.
4. Explain why this graph is a cycle instead of a simple blocking queue.
5. Decide whether the victim choice looks more consistent with rollback-cost reasoning or with an explicit priority difference. Support the answer from the graph rather than from guesswork.
6. Write one retry rule that is safe for this scenario and one transaction-design rule that removes the recurring deadlock risk.

## Completion Criteria

- you can map the victim and both wait edges back to the orders-then-trades versus trades-then-orders access pattern
- you can defend one retry rule that assumes idempotent replay instead of blindly retrying every failure
- you can state the transaction redesign before describing the retry policy