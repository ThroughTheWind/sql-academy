# Advanced 003: Plan Cache, Memory Grants, And Wait Signals

## Objective

Practice a small SQL Server internals workflow by generating a tagged workload, inspecting plan-cache metadata, comparing storage page counts, and deciding which memory or wait signal to inspect next.

## Exercise Type

This pack is a guided lab.

Completion is based on the README tasks and `expected-outcomes.md`, not on `answer.sql` or `validation.sql`.

## Scenario

The trade read path feels inconsistent across runs. Before changing code or indexes, you want one local workflow that shows what was cached, how much data the engine is touching, and which evidence surface should disprove your first theory.

## Assets

- `starter.sql` builds a tagged local workload and shows the core DMV surfaces.
- `broken.sql` shows the kinds of weak diagnostic habits this lab is designed to correct.
- `expected-outcomes.md` defines what a strong lab write-up should conclude.
- `hints.md` gives progressive guidance.
- `optional-solution.sql` shows one defensible diagnostic sequence.

## Tasks

1. Run `starter.sql` and identify the cached `internals_lab_trade_filter` query. Record its execution count, logical-read profile, and whether the plan appears to be reused.
2. Capture a storage snapshot for `academy.Posts`, `academy.Comments`, and `academy.Trades` from `sys.dm_db_partition_stats`. Explain which table is most likely to amplify I/O or lookup cost first as the workload grows.
3. Choose one memory-related signal and one wait-related signal you would inspect next, then write one falsifiable hypothesis before changing code or indexes.

## Manual Check

- the plan-cache review is filtered to the tagged lab query instead of a generic top-cost query
- the storage snapshot uses row counts and used page counts to support an access-path explanation
- the final hypothesis names one disconfirming check instead of treating waits or memory grants as automatic root cause proof

## Focused Companion Check

When you want a narrow executable companion for the starter workflow, run:

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~PlanCacheLabSmokeTests"`

That smoke test executes `starter.sql` and verifies that the tagged `internals_lab_trade_filter` query lands in plan cache while the storage snapshot still covers `academy.Posts`, `academy.Comments`, and `academy.Trades`.
