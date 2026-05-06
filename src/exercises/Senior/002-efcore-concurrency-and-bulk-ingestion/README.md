# Senior 002: EF Core N+1, Optimistic Concurrency, And Bulk Ingestion

## Objective

Connect SQL Server behavior directly to application code by inspecting an N+1 path, handling `rowversion` conflicts, and planning a bulk-ingestion strategy.

## Scenario

The application is about to ingest a larger trade feed while also serving read traffic. The current approach loads related entities inefficiently and does not clearly separate validation, staging, and publish steps.

## Tasks

1. find the N+1 risk in the API or persistence layer and rewrite the query shape
2. simulate an optimistic concurrency conflict on `academy.Orders`
3. design a staged bulk-ingestion workflow that does not overwhelm OLTP tables

## Validation

- the learner can show the SQL difference before and after fixing the N+1 path
- the concurrency conflict is detected rather than silently overwritten
- the ingestion design separates landing, validation, and publish phases