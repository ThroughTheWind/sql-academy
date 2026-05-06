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

- walk [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md) and then complete [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
- use [Lab 01](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-01-efcore-posts-read-path.md) to trace and extend the EF Core posts read path
- use [Lab 02](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-02-dapper-trades-read-path.md) to trace and extend the Dapper trades read path
- use [Lab 03](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/lab-03-rowversion-and-staged-ingestion.md) to reproduce optimistic concurrency, exercise a real staged trade-import path, and inspect persisted batch history after publish

## Expected Outcomes

- the learner can reason from LINQ and Dapper code to the SQL Server behavior underneath
- the learner can decide when an abstraction helps and when it hides too much

## Validation Checklist

- the learner can explain which earlier SQL lessons are being reused in the application query paths
- at least one focused posts or trades test is rerun after a deliberate code change
- the Dapper and EF Core read paths stay deterministic under the documented sort contract
- the order-status write path reproduces a `409 Conflict` when the rowversion is stale
- the trade-import path reports clear validation, duplicate, rejection, dry-run preview, and publish outcomes
- the learner can retrieve a completed trade-import batch later by `batchId` instead of relying only on the immediate POST response
- the staged-ingestion review pack remains available as optional reinforcement after the code labs