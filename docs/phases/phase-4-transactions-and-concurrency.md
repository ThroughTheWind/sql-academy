# Phase 4: Transactions And Concurrency

## Objectives

- understand isolation, blocking, deadlocks, and optimistic concurrency
- practice transaction scoping in both SQL and EF Core

## Prerequisites

- relational model fluency and basic schema change experience

## Concepts

- ACID tradeoffs in real OLTP systems
- isolation levels and lock behavior
- blocking chains and deadlock graphs
- optimistic concurrency with `rowversion`

## Exercises

- reproduce a blocking scenario with two sessions
- analyze and retry a deadlock scenario
- use [Senior 001 deadlock graph lab](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/deadlock-graph-lab.md) to map victim, owner, and waiter edges back to the session order before you settle on retry policy
- trigger an optimistic concurrency conflict in EF Core and recover safely

## Reference Tracks

- use [Operations Track](../operations/README.md) when blocking or deadlock work needs telemetry, smoke-test, or incident-triage context
- use [EF Core Track](../efcore/README.md) when the concurrency question is really about `rowversion`, conflict handling, or application write-path design

## Expected Outcomes

- the learner can explain when a transaction boundary is too large or too small
- the learner can distinguish blocking from deadlocking and choose the right mitigation

## Validation Checklist

- a learner can capture the blocked session and the blocker
- a learner can explain the deadlock victim and the retry strategy
- a learner can interpret one deadlock graph without confusing a cycle with a simple queue
- EF Core concurrency conflicts are handled without hidden retries