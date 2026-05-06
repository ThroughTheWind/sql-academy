# Senior 001: Concurrency, Blocking, And Deadlocks

## Objective

Reproduce production-style contention, inspect the blocking behavior, and reason about a deadlock-safe redesign.

## Scenario

Two sessions update related rows in different orders while a long-running transaction holds locks open. The system begins to stall and eventually throws deadlock errors.

## Tasks

1. reproduce a blocking chain with two sessions
2. capture the lock ordering that creates the deadlock
3. propose a retry-safe and ordering-safe fix

## Validation

- the learner can identify the blocker and the blocked session
- the learner can explain the deadlock victim choice
- the learner can describe what belongs in retry logic versus what belongs in transaction redesign