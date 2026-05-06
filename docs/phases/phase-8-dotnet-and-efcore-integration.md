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

- walk [From SQL To EF Core And Dapper](../learning/from-sql-to-efcore-and-dapper.md) and restate one EF Core path and one Dapper path in SQL terms
- extend the API with a new query endpoint
- eliminate an EF Core N+1 regression
- orchestrate an EF Core write and Dapper read within a consistent flow

## Expected Outcomes

- the learner can reason from LINQ and Dapper code to the SQL Server behavior underneath
- the learner can decide when an abstraction helps and when it hides too much

## Validation Checklist

- the learner can explain which earlier SQL lessons are being reused in the application query paths
- generated SQL is inspected for at least one EF Core path
- Dapper and EF Core paths return consistent business results
- transaction and retry behavior is explicit in code reviews