# Statistics Drift And Skew Follow-Ons

Use this optional guide after [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md), [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md), or [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) when the main tuning route already feels clear and you want more realistic plan-instability questions.

This is not a new required pack. It is a routed follow-on that reuses the existing larger-cardinality workload variant, Query Store workflow, and performance-track anchors instead of widening the default seed path or pretending one deterministic exercise can cover every skew story honestly.

## What This Guide Is For

- selective versus broad parameter values that should not be treated as one identical workload shape
- data growth that changes estimates, logical reads, or memory pressure enough to make an older plan story less trustworthy
- before-and-after comparisons that need persisted evidence from Query Store instead of one current plan-cache snapshot

## Start Here

Keep one baseline validation surface nearby before you begin the manual follow-on work:

- [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) when the main question is query shape, indexing, and parameter sensitivity
- [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) when the main question is persisted regression evidence across more than one run
- [Advanced 003: Plan Cache, Memory Grants, And Wait Signals](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) when estimate quality starts changing grants, spills, or engine-behavior symptoms

## Common Setup Loop

1. Capture a baseline first with the current row counts and one stable validation surface.
2. Apply [001_high_cardinality_workload_variant.sql](../../db/performance/001_high_cardinality_workload_variant.sql) only after you know what “before” looks like.
3. Rerun the same query, lab, or validation anchor instead of switching to a different surface midstream.
4. Record what changed in estimated rows, actual rows, logical reads, duration, plan count, or memory-grant behavior.
5. Decide whether the problem is really skew, statistics drift, query shape, indexing, or a combination instead of defaulting to “add one more index.”

## Scenario 1: Skew-Driven Parameter Sensitivity

### Goal

Show why one cached or recently chosen plan can be a poor fit for a different parameter profile when the data distribution is not uniform.

### Best Starting Surfaces

- [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- [Query Store Regression Runbook](../operations/query-store-regression-runbook.md)
- [TradeReadService](../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs)

### Suggested Loop

1. Use the larger-cardinality workload so the trade path has enough rows for selectivity differences to matter.
2. Pick one selective and one broad parameter profile for the same trade-oriented lookup or filter shape.
3. Compare estimated versus actual rows and note whether the same plan shape handles both values well.
4. Use Query Store to compare the same tagged query across the capture window instead of relying only on the plan currently in memory.
5. Decide whether the right answer is a query or index change, a parameter-sensitivity explanation, or a temporary containment move such as reversible plan forcing.

### Keep These Narrow Anchors Nearby

- `powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety`
- `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryStoreLabSmokeTests"`

### A Strong Outcome

You can explain why the workload is skew-sensitive, what evidence shows the mismatch, and why “one fast plan” is not enough of a conclusion.

## Scenario 2: Statistics Drift After Additive Growth

### Goal

Show how a once-reasonable estimate or runtime story can become less trustworthy after the data shape grows, even when the query text looks unchanged.

### Best Starting Surfaces

- [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Advanced 003: Plan Cache, Memory Grants, And Wait Signals](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)
- [Advanced 004: Query Store And Regression Triage](../../src/exercises/Advanced/004-query-store-and-regression-triage/README.md)
- [Performance Labs](../../db/performance/README.md)

### Suggested Loop

1. Capture the baseline plan, estimates, logical reads, and Query Store history before you expand the local workload.
2. Re-run the larger-cardinality workload variant with materially larger counts so the data shape changes without changing the default repo baseline.
3. Rerun the same query or lab surface and compare estimated rows, actual rows, memory-grant behavior, and persisted Query Store metrics.
4. Write down whether the changed evidence points to stale or less-useful statistics, a missing index, a query-shape problem, or a broader workload change.
5. Decide whether refreshing statistics, changing the query or index, or using a temporary containment move is the honest next step.

### Keep These Narrow Anchors Nearby

- `powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety`
- `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~PlanCacheLabSmokeTests"`
- `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryStoreLabSmokeTests"`

### A Strong Outcome

You can defend why the regression story changed after additive growth and why the fix is about evidence, not just another index recommendation.

## What Not To Do

- do not widen the default seed or validation baseline just to make one local scenario more interesting
- do not compare different query texts between the before and after passes if the point is plan or estimate drift
- do not switch from Query Store to plan cache to benchmarks in one uncontrolled story and call that one measurement
- do not claim statistics drift if the evidence really points to blocking, deadlocks, or a changed application query path instead

## Next Step

Return to [Performance Track](README.md) when you need the broader routing matrix again, or use [Query Store Regression Runbook](../operations/query-store-regression-runbook.md) when you want the compact persisted-regression checklist instead of the full follow-on workflow.