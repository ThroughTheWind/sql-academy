# Query Store Regression Runbook

Use this as a printable checklist when a query feels slower over time and you need persisted plan and runtime history instead of a one-moment plan-cache snapshot.

This runbook is a companion to [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md), [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md), and [Performance Track](../performance/README.md).

It is not a replacement for the guided lab. It is the compressed checklist for isolating the exact query and plan history you care about before deciding whether plan forcing is safe containment or a distraction.

## First Signal

- name the one path that feels slower and the exact workload shape you want to compare
- decide whether the question is about regression over time rather than only one current execution
- keep one tagged or otherwise narrow query target in scope before opening generic top-query views

## Scope The Query Store Window

- confirm Query Store is enabled and capturing the database in a useful state
- choose the smallest time window that still shows the regression you are trying to explain
- avoid browsing the entire database history before you isolate the query text you actually care about

## Isolate The Query And Plan History

- filter Query Store to the tagged query text or other exact identifier instead of generic top offenders
- record the `query_id`, relevant `plan_id` values, execution count, average duration, and average logical reads
- compare the same query across more than one plan when plan instability is the real question

## Containment Decision

- treat plan forcing as temporary containment only when the query is isolated clearly enough that the forced plan is defensible
- name one falsifiable rule for when you would reverse that forcing decision
- do not use plan forcing to avoid understanding a data-shape, index, or parameter-sensitivity problem

## Follow-Up Validation

- rerun the same tagged workload after the containment or tuning change
- compare the updated Query Store history against the same query and plan surface instead of switching tools midstream
- keep one additional validation surface nearby, such as [QueryStoreLabSmokeTests](../../tests/SqlAcademy.PerformanceTests/QueryStoreLabSmokeTests.cs), when you want to confirm the starter workflow still captures the intended tagged queries

## Drill Links

- [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- [Performance Track](../performance/README.md)
- [Operations Track](README.md)
