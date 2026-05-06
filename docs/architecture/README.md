# Architecture Overview

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