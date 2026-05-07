# Phase 5: Indexing And Performance

## Objectives

- connect workload shape to indexing, statistics, and execution plans
- compare EF Core and Dapper behavior with evidence rather than preference

## Prerequisites

- Phases 2 through 4 complete

## Concepts

- clustered and nonclustered indexes
- covering indexes and lookup tradeoffs
- statistics freshness and parameter sensitivity
- tracking versus `AsNoTracking`

## Exercises

- tune a slow join with an evidence-backed index change
- inspect a parameter sniffing scenario and test mitigations
- compare EF Core and Dapper on the same read path
- work through [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) when the question is about persisted plan and runtime history rather than one cache snapshot

## Reference Tracks

- use [Performance Track](../performance/README.md) for Query Store workflow, performance tests, benchmarks, and tuning-oriented repo anchors
- use [EF Core Track](../efcore/README.md) when the comparison shifts from SQL plans into tracking, projection, or generated-SQL behavior

## Expected Outcomes

- the learner can explain why a plan changed, not just that it changed
- the learner can reject cargo-cult tuning advice with measurement

## Validation Checklist

- before-and-after plans are captured
- logical reads and duration are compared explicitly
- index changes are justified against write cost and maintenance tradeoffs
- the learner can explain when Query Store is the right follow-up surface for regression history