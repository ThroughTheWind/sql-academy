# Senior 002: EF Core N+1, Optimistic Concurrency, And Bulk Ingestion

## Objective

Connect SQL Server behavior directly to application code by inspecting an N+1 path, handling `rowversion` conflicts, and planning a bulk-ingestion strategy.

## Exercise Type

This pack is a validation pack.

Use `answer.sql` to fill in the structured review tables and `validation.sql` to verify that your N+1 diagnosis, concurrency interpretation, and ingestion steps match the expected shape.

## Scenario

The application is about to ingest a larger trade feed while also serving read traffic. The current approach loads related entities inefficiently and does not clearly separate validation, staging, and publish steps.

## Assets

- `starter.sql` gives baseline SQL surfaces and a staged import fixture to compare with the application code.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the expected review tables in one SQL session.
- `broken.sql` captures the risky or incomplete implementation to review.
- `expected-outcomes.md` defines the completion contract.
- `hints.md` gives progressive guidance.
- `optional-solution.sql` shows one valid SQL-side answer shape.

## Tasks

1. find the N+1 risk in the API or persistence layer and rewrite the query shape
2. simulate an optimistic concurrency conflict on `academy.Orders`
3. design a staged bulk-ingestion workflow that does not overwhelm OLTP tables

## Validation

- the read-path review identifies a set-based fix for the N+1 shape
- the concurrency review captures the current `rowversion` and the correct conflict behavior
- the ingestion plan separates landing, validation, deduplication, and publish stages explicitly
