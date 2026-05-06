# Architecture Overview

If your goal is to learn the repository in order, start with [Documentation Hub](../README.md) and [Learning Docs](../learning/README.md) first. This page is the structural reference once you want to understand why the runtime is arranged this way.

## Structural Principles

- prefer explicit boundaries over clever abstractions
- keep the domain model simple and relationally grounded
- let SQL Server behavior stay visible in the codebase
- use EF Core where state management matters and Dapper where query control matters

## Current Runtime Topology

- `SqlAcademy.Api` exposes read-focused HTTP endpoints and applies migrations on startup.
- `SqlAcademy.Worker` demonstrates background operational behavior against the same database.
- `SqlAcademy.Persistence` is the only project that knows the database model, mappings, and query services.
- `SqlAcademy.Observability` centralizes shared telemetry and logging behavior.

## Extension Rules

- add new bounded examples by extending the existing domain and persistence layers first
- avoid introducing new frameworks when EF Core, Dapper, and plain ASP.NET Core are enough
- keep exercise assets close to the scenarios they teach
- document every non-obvious operational decision where the learner encounters it

## Related Learner Entry Points

- [How To Start](../learning/how-to-start.md)
- [Lesson 09: EF Core, Dapper, And Query Shape](../learning/09-ef-core-dapper-and-query-shape.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](../learning/10-observability-testing-and-performance-engineering.md)