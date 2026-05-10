# Senior 001: Concurrency, Blocking, And Deadlocks

## Objective

Reproduce production-style contention, inspect the blocking behavior, and reason about a deadlock-safe redesign.

## Before You Start

- read [Lesson 05: Transactions, Blocking, And Deadlocks](../../../../docs/learning/05-transactions-blocking-and-deadlocks.md)
- keep [deadlock-graph-lab.md](deadlock-graph-lab.md) nearby if wait cycles still feel more abstract than plain blocking
- keep [Deadlock Response Runbook](../../../../docs/operations/deadlock-response-runbook.md) nearby if you want the same reasoning compressed into an incident checklist after the lab

## Suggested Workflow

1. Run `starter.sql` unchanged and open two query windows so Session A and Session B stay separate.
2. Reproduce the blocking sequence first and name the blocker before you try to explain the deadlock.
3. Complete the validation-pack tables in `answer.sql` so the lock order and mitigation reasoning are explicit.
4. Work through [deadlock-graph-lab.md](deadlock-graph-lab.md) with [deadlock-graph-sample.xml](deadlock-graph-sample.xml) only after the session order already makes sense.
5. Compare your redesign with `optional-solution.sql` only after you have a concrete opinion about lock ordering and retry scope.

## If You Get Stuck

- separate simple blocking from a true wait cycle before you change the SQL or the retry story
- draw the Session A versus Session B resource order from `starter.sql` on paper before you read the graph again
- use `broken.sql` to name whether the problem is transaction duration, inconsistent access order, or retry misuse before you open `optional-solution.sql`

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

Use [Deadlock Response Runbook](../../../../docs/operations/deadlock-response-runbook.md) when you want the same deadlock-response loop compressed into a printable incident checklist.

## Focused Companion Check

When you want a narrow executable check for the supplemental graph assets, run:

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~DeadlockGraphLabAssetTests"`

That asset smoke test verifies that the README, graph lab, and sample deadlock XML stay aligned without pretending to automate the actual two-session concurrency exercise.