# Deadlock Response Runbook

Use this as a printable checklist when blocking escalates into deadlock errors or when a blocking chain already suggests inconsistent access order.

This runbook is a companion to [Lesson 05: Transactions, Blocking, And Deadlocks](../learning/05-transactions-blocking-and-deadlocks.md), [Senior 001: Concurrency, Blocking, And Deadlocks](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md), and the [deadlock graph lab](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/deadlock-graph-lab.md).

It is not a replacement for the pack. It is the compressed response checklist for deciding what belongs in immediate stabilization, retry policy, and transaction redesign.

## First Symptom

- decide whether you are looking at simple blocking, repeated timeouts, or an actual deadlock victim error
- identify the hottest session, endpoint, or write path before widening the search
- keep one healthy control path in view so you do not treat the whole platform as broken

## Blocking And Graph Evidence

- identify the blocker and the blocked session before talking about fixes
- capture the access order that each side already holds versus what it is waiting on
- if a deadlock graph exists, identify the victim, owner edges, and waiter edges from the graph instead of from guesswork
- map the graph back to the conflicting statements or code paths while the evidence is still fresh

## Retry Versus Redesign

- use retry only when the operation is safe to replay idempotently
- name the transaction-design change that removes the recurring cycle instead of treating retry as the full fix
- prefer shorter transactions and consistent access order when the same objects are updated together
- do not call blind retry a mitigation if it only hides the same unsafe pattern under load

## Safe Stabilization

- reduce contention by narrowing the affected path, decreasing concurrency pressure, or pausing the highest-risk write flow
- avoid emergency schema or index changes unless the evidence is already strong enough to defend them
- keep the next diagnostic step explicit: what result would confirm the deadlock cycle is actually gone?

## Follow-Up Validation

- reproduce the original blocking pattern against the safer access order if the scenario is repeatable
- confirm the deadlock victim no longer appears for the same conflicting flow
- record both the retry rule and the design rule so later incident reviews do not collapse them into one idea

## Drill Links

- [Senior 001: Concurrency, Blocking, And Deadlocks](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
- [Deadlock Graph Lab](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/deadlock-graph-lab.md)
- [Operations Track](README.md)
