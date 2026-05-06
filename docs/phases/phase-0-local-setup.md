# Phase 0: Local Setup

## Objectives

- get the full Docker-first platform running locally
- connect to SQL Server with a local tool
- inspect the seeded `academy` schema and baseline API endpoints
- understand where logs, metrics, and migrations are configured

## Prerequisites

- Docker Desktop or equivalent Docker engine
- .NET 10 SDK
- a SQL client such as Azure Data Studio or SSMS

## Concepts

- Compose service topology and startup ordering
- `.env` driven local configuration
- raw SQL bootstrap scripts versus EF Core migrations
- liveness, readiness, metrics, and traces

## Exercises

- start the stack with `docker compose up --build`
- connect to `LearningDb` and list all `academy` tables
- call `/api/v1/posts`, `/health/ready`, and `/metrics`
- inspect Prometheus scrape targets and Grafana datasource wiring

## Expected Outcomes

- the learner can navigate the repository and run the platform without hidden setup
- the learner can explain how the database is created and seeded

## Validation Checklist

- `docker compose config` succeeds
- `SELECT COUNT(*) FROM academy.Users` returns data
- `GET /health/ready` returns success
- Prometheus shows the `sqlacademy-api` target as up