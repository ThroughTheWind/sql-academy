# Performance Labs

Use this folder for optional SQL Server workload and diagnostics scripts that support the formal exercises without changing the default baseline seed used by tests and validation packs.

## Current Scripts

- [001_high_cardinality_workload_variant.sql](001_high_cardinality_workload_variant.sql): adds a repeatable larger-cardinality workload for posts, comments, and trades so Lessons 06 and 07 can reason about statistics, plan stability, memory grants, and tracking cost with more realistic row counts.

## Usage

1. Initialize the baseline database first with [../seed](../seed) or [../../infra/scripts/init-database.sh](../../infra/scripts/init-database.sh).
2. Run [001_high_cardinality_workload_variant.sql](001_high_cardinality_workload_variant.sql) only when you want more local volume for tuning, Query Store, or benchmark work.
3. Rerun the same validation surface you were already using, such as [../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs), [../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs), or the relevant guided lab.

The workload variant is additive and idempotent for a given parameter set. Running it again with the same counts does not duplicate rows, and running it later with larger counts only adds the missing synthetic rows.

## Suggested Next Additions

- plan cache inspection scripts
- missing-index and duplicate-index review helpers
- blocking chain snapshots
- wait stats sampling queries
- parameter sniffing comparison harnesses