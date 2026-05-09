# Advanced 004: Query Store And Regression Triage

## Objective

Use Query Store to capture a tagged workload, isolate the exact query and plan history you care about, and decide whether plan forcing is a safe containment move or a distraction.

## Exercise Type

This pack is a guided lab.

Completion is based on the README tasks and `expected-outcomes.md`, not on `answer.sql` or `validation.sql`.

## Scenario

The read path feels slower than it did earlier in the week. Plan cache only shows what is currently in memory, but you need a persisted SQL Server surface that helps you answer what changed, whether more than one plan is involved, and whether forcing a plan would buy safety or hide the real problem.

## Assets

- `starter.sql` enables a local Query Store workflow, runs tagged lab queries, flushes the captured data, and shows the core review query.
- `broken.sql` shows weak Query Store habits that surface noisy data instead of the query you actually need to diagnose.
- `expected-outcomes.md` defines what a strong lab write-up should conclude.
- `hints.md` gives progressively more direct guidance.
- `optional-solution.sql` shows one defensible review sequence.

## Tasks

1. Run `starter.sql` and confirm Query Store is available in `LearningDb` with a clean capture window for the lab.
2. Isolate the `query_store_lab_trade_lookup` query text and record its `query_id`, plan count, execution count, average duration, and average logical reads.
3. Compare that output with the `query_store_lab_post_search` query so you can explain why persisted query history answers a different question than a one-moment plan-cache snapshot.
4. Decide whether plan forcing would be a reasonable temporary containment move for the tagged trade query. Write one falsifiable rule for when you would reverse that decision.

## Manual Check

- the learner filters Query Store to the tagged lab query instead of browsing generic top offenders
- the learner reviews runtime stats by `plan_id`, not only one aggregated number for the whole database
- the learner can explain when Query Store is a better regression surface than plan cache
- the final decision names one disconfirming check before forcing or rejecting a plan

## Completion Checklist

- [ ] you isolated `query_store_lab_trade_lookup` by tagged query text and recorded its `query_id`, plan count, execution count, average duration, and average logical reads
- [ ] you compared that trade query with `query_store_lab_post_search` instead of treating Query Store as one undifferentiated database-wide report
- [ ] you wrote one reversible rule for when plan forcing would be reasonable and when you would back it out
- [ ] you can explain why Query Store answers a different question than a one-moment plan-cache snapshot

## Companion Material

- [Advanced 003: Plan Cache, Memory Grants, And Wait Signals](../003-plan-cache-memory-grants-and-waits/README.md)
- [Query Store Regression Runbook](../../../../docs/operations/query-store-regression-runbook.md)
- [Performance Track](../../../../docs/performance/README.md)

## Focused Companion Check

When you want a narrow executable companion for the tagged Query Store workflow, run:

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryStoreLabSmokeTests"`

That smoke test verifies the starter workflow still captures the tagged lab queries and keeps the persisted-review surface aligned with the pack contract.