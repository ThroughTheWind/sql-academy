# Senior 001: Concurrency, Blocking, And Deadlocks

## Objective

Reproduce production-style contention, inspect the blocking behavior, and reason about a deadlock-safe redesign.

## Scenario

Two sessions update related rows in different orders while a long-running transaction holds locks open. The system begins to stall and eventually throws deadlock errors.

## Assets

- `starter.sql` contains the manual two-session script plus a triage fixture for validation.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the blocking-cycle and mitigation summaries.
- `deadlock-graph-lab.md` walks through one representative deadlock graph and ties the graph evidence back to retry-policy reasoning.
- `deadlock-graph-sample.xml` captures the same orders-then-trades versus trades-then-orders cycle as a concrete deadlock graph artifact.
- `broken.sql` contains the unsafe access-order example.
- `expected-outcomes.md` lists the checks the result must satisfy.
- `hints.md` gives progressive guidance.
- `optional-solution.sql` shows one valid mitigation direction.

## Tasks

1. reproduce a blocking chain with two sessions
2. capture the lock ordering that creates the deadlock
3. work through [deadlock-graph-lab.md](deadlock-graph-lab.md) against [deadlock-graph-sample.xml](deadlock-graph-sample.xml) and map the graph back to Session A and Session B
4. propose a retry-safe and ordering-safe fix

## Validation

- the learner can identify the blocker and the blocked session
- the learner can explain the deadlock victim choice
- the learner can map the graph owner and waiter edges back to the inconsistent access order in the scenario
- the learner can describe what belongs in retry logic versus what belongs in transaction redesign