# Phase 6: SQL Server Internals

## Objectives

- build enough internal understanding to debug non-obvious production behavior

## Prerequisites

- phases through indexing and concurrency complete

## Concepts

- storage layout and page organization
- plan cache and compilation behavior
- memory grants and spills
- wait stats and scheduler pressure

## Exercises

- inspect plan cache differences for parameter-sensitive queries
- identify waits during an intentionally degraded workload
- reason about why a plan spills or recompiles unexpectedly
- work through [Advanced 003: Plan Cache, Memory Grants, And Wait Signals](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)

## Expected Outcomes

- the learner can connect surface symptoms to engine-level causes
- the learner can form a debugging hypothesis before changing code or indexes

## Validation Checklist

- waits and plan metadata are captured and interpreted correctly
- learners can explain what evidence would confirm or falsify a hypothesis
- root-cause reasoning is documented alongside the experiment
- the [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) lab notes isolate the tagged query instead of generic server activity