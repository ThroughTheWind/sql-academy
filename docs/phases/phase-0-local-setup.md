# Phase 0: Local Setup

## Suggested Time Budget

- 30 to 60 minutes if Docker, your SQL client, and local ports already work on your machine
- longer if Docker setup, port conflicts, or SQL client connection setup are still new

## Objectives

- get the full Docker-first platform running locally
- connect to SQL Server with a local tool
- inspect the seeded `academy` schema and baseline API endpoints
- understand where logs, metrics, and migrations are configured

## Prerequisites

- Docker Desktop or equivalent Docker engine
- .NET 10 SDK
- a SQL client such as Azure Data Studio or SSMS
- the ability to use the ports configured in [../../.env](../../.env), or to adjust them before startup

## First Success Markers

- `docker compose config` succeeds before you start the full stack
- `http://localhost:8080/health/ready` returns success after startup
- `SELECT COUNT(*) FROM academy.Users` returns seeded data from `LearningDb`

## If You Get Stuck

- return to [How To Start](../learning/how-to-start.md) and make the first successful query your only goal for this phase
- use [SQL-First Day One](../learning/sql-first-day-one.md) if you need a smaller success target than the full platform walkthrough
- use [SQL Client Connection Guide](../learning/sql-client-connection-guide.md) if the missing step is simply getting your client connected to `LearningDb`
- use [Local Setup Troubleshooting](../learning/local-setup-troubleshooting.md) for Docker, port, readiness, and login failures
- use [Schema Quick Reference](../learning/schema-quick-reference.md) if the seeded tables are still abstract
- use [Learning Glossary](../learning/glossary.md) if terms such as readiness, migrations, or metrics are slowing you down
- treat Prometheus, Grafana, `/metrics`, and `/openapi/v1.json` as optional until the database and API already feel predictable

## Concepts

- Compose service topology and startup ordering
- `.env` driven local configuration
- raw SQL bootstrap scripts versus EF Core migrations
- liveness, readiness, metrics, and traces

## Exercises

- review [../../.env](../../.env) and keep the defaults unless they conflict with your machine
- start the stack with `docker compose up --build`
- wait for `/health/ready` to succeed before browsing deeper endpoints
- connect to `LearningDb` and list all `academy` tables
- treat `/openapi/v1.json`, Prometheus, Grafana, and `/metrics` as follow-up checks after the database and API are already stable

## Expected Outcomes

- the learner can navigate the repository and run the platform without hidden setup
- the learner can explain how the database is created and seeded

## Validation Checklist

- `docker compose config` succeeds
- `SELECT COUNT(*) FROM academy.Users` returns data
- `GET /health/ready` returns success
- Prometheus shows the `sqlacademy-api` target as up after the optional observability check