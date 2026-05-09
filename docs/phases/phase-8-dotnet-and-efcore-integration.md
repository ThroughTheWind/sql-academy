# Phase 8: .NET And EF Core Integration

## Objectives

- connect SQL knowledge directly to application behavior in ASP.NET Core, EF Core, and Dapper
- preserve the same query-shape reasoning from the earlier SQL phases while moving into application code

## Prerequisites

- strong SQL fundamentals plus performance and concurrency fluency

## Concepts

- DbContext lifetime and mapping
- generated SQL inspection and result-contract thinking
- pagination, filtering, and sorting in APIs
- hybrid EF Core plus Dapper architecture
- N+1 detection, transaction handling, and retries

## Exercises

- walk [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md) and then complete [Advanced 005: EF Core, Dapper, And Query-Contract Read Paths](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md)
- use [Lab 01](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/lab-01-efcore-posts-read-path.md) to trace and extend the EF Core posts read path
- use [Lab 02](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/lab-02-dapper-trades-read-path.md) to trace and extend the Dapper trades read path
- use [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) as the focused generated-SQL and N+1 follow-up on the posts read path when you need stronger evidence than code inspection alone
- use [Senior 002: Optimistic Concurrency And Staged Trade Ingestion](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) as the later follow-up for `rowversion` conflicts, staged trade import, and persisted batch inspection

## Reference Tracks

- use [EF Core Track](../efcore/README.md) for code and test anchors around projections, tracking, concurrency, migrations, and staged imports
- use [Performance Track](../performance/README.md) when the concern becomes generated SQL, tracking cost, or a fair EF Core versus Dapper comparison
- use [Operations Track](../operations/README.md) when the same application change needs rollout, telemetry, or incident-triage context

## Expected Outcomes

- the learner can reason from LINQ and Dapper code to the SQL Server behavior underneath
- the learner can decide when an abstraction helps and when it hides too much

## Validation Checklist

- the learner can explain which earlier SQL lessons are being reused in the application query paths
- at least one focused posts or trades test is rerun after a deliberate code change in the read-path bundle
- the Dapper and EF Core read paths stay deterministic under the documented sort contract
- the later order-status follow-up reproduces a `409 Conflict` when the rowversion is stale
- the trade-import path reports clear validation, duplicate, rejection, dry-run preview, and publish outcomes
- the learner can retrieve a completed trade-import batch later by `batchId` instead of relying only on the immediate POST response
- the staged-ingestion review pack remains available as optional reinforcement after the code labs
- the learner can capture or explain the generated SQL shape behind the posts read path and use that evidence to argue against an N+1 refactor