# Lesson 08: Operational Engineering And Release Safety

## Focus

Treat data-platform changes as operational events. Good engineering here is about deterministic environments, safe rollout, telemetry, and rollback discipline.

## Before You Start

- be comfortable with the change-safety mindset from [Lesson 04](04-schema-design-and-migration-safety.md) and the concurrency risk mindset from [Lesson 05](05-transactions-blocking-and-deadlocks.md)
- keep [Learning Glossary](glossary.md) open if terms such as smoke test, roll-forward, or readiness are still slow
- expect to reason about release sequence and observation, not just SQL syntax or code diff quality

## Suggested Time Budget

- 75 to 120 minutes to read the lesson, inspect the Compose topology, and write at least one watch plan and fallback posture
- longer if release safety is newer than local development or query work

## If You Get Stuck Early

- reduce the problem to one proposed change, one smoke test, and one first-five-minutes watch plan
- start with `/health/live`, `/health/ready`, and one small useful-work check before you open dashboards
- use [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) only after you can explain what would block a deployment or force a rollback-versus-roll-forward decision

## Why This Lesson Matters

The difference between a classroom migration and a production migration is not syntax. It is operational context:

- other sessions are running
- APIs and workers are depending on the database
- dashboards and alerts may or may not catch regressions
- rollback may be harder than rollout

This lesson teaches you to think like the person who would have to own the release if it goes wrong.

## Repository Anchors

- [Phase 7: Operational Engineering](../phases/phase-7-operational-engineering.md)
- [Docker Compose stack](../../docker-compose.yml)
- [API startup and health/metrics endpoints](../../src/apps/SqlAcademy.Api/Program.cs)
- [OpenTelemetry service registration](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs)
- [Prometheus configuration](../../infra/observability/prometheus.yml)
- [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)
- [Senior 004: Posts API Latency And Observability Triage](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)
- [How To Start](how-to-start.md)

## Deterministic Environments Come First

If your local or pre-production environment is not predictable, release reasoning becomes weak. The repository intentionally makes the platform explicit:

- SQL Server container
- database initialization container
- API and worker services
- OpenTelemetry collector
- Prometheus
- Grafana

Read [Docker Compose stack](../../docker-compose.yml) as an operational diagram, not just a startup file. It shows:

- service dependencies
- health checks
- ports and connection strings
- initialization order
- observability components

That is already a release story in miniature.

## Operational Thinking For Schema Changes

Every schema change should be reviewed with these questions:

1. Is the change backward compatible?
2. How long could it block or contend?
3. What needs to be deployed before and after it?
4. How will success be validated?
5. What is the rollback or roll-forward plan?

If the change cannot be rolled back cleanly, the roll-forward plan must be especially clear and tested.

## Release-Readiness Drill

Use [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) when you want one concrete operational drill that forces a go or no-go call before the window opens.

Use [Release Runbook](../operations/release-runbook.md) when you want the same release posture compressed into a printable change-window checklist.

It ties together:

- migration safety gates from the staged backfill route
- first-five-minutes health, metric, log, and trace checks
- rollback versus roll-forward triggers that must be explicit before deployment begins

## Rollout Is A Sequence, Not A Button Press

A safe release usually has distinct phases:

### Preflight

- confirm the environment is healthy
- confirm the target migration or code version is understood
- confirm dashboards and health endpoints are available
- confirm someone knows what signals to watch

### Deployment

- apply additive schema changes first when possible
- deploy compatible application code
- run smoke tests immediately

### Observation Window

- watch health endpoints
- watch request failures and latency
- watch resource pressure
- check that expected background work is still progressing

### Stabilization

- verify the migration or feature behaved under real traffic
- remove temporary compatibility logic only after confidence is earned

## Smoke Tests Should Be Concrete

Good smoke tests are not vague statements like “the API seems up.” They are small proofs that the critical path is functioning.

In this repository, useful smoke surfaces include:

- `/health/live`
- `/health/ready`
- `/metrics`
- a small API read such as the posts endpoint used in integration tests

The point is not exhaustive coverage. The point is to catch obvious release failure quickly.

## Observability Is Part Of Release Design

In [API startup and health/metrics endpoints](../../src/apps/SqlAcademy.Api/Program.cs), the application wires in:

- structured request logging
- OpenTelemetry tracing and metrics
- Prometheus metrics
- health endpoints

In [OpenTelemetry service registration](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs), the service adds ASP.NET Core, HTTP client, and runtime instrumentation. That means you already have a meaningful baseline for release observation.

The operational habit to learn is this:

Define what success and failure would look like before the release starts.

## What To Watch In The First Five Minutes

For a data-centric release, the first five minutes often matter most. Watch for:

- health endpoint failures
- request error rates
- latency spikes
- abnormal restarts
- blocked startup or failed initialization
- sudden database contention or timeouts

Do not wait for users to become your alerting system.

## Rollback Versus Roll-Forward

Some changes are easy to roll back. Others are not.

Examples of easier rollback:

- a bad read-only API deployment with no contract change

Examples of harder rollback:

- destructive schema change
- backfill that has already mutated data
- contract change consumed by multiple deployed services

When rollback is hard, a controlled roll-forward plan is often the safer posture. That plan should exist before deployment, not after the incident begins.

## Incident-Oriented Thinking

Treat operational work as preparation for incident response. Ask:

- what is the most likely way this release fails?
- how would I detect it quickly?
- what dashboard, log, or metric would I check first?
- what is the first safe mitigation if the release misbehaves?

This mindset changes release quality more than any amount of ceremony.

## Read The Compose Stack As A Readiness Checklist

The Compose file already teaches good questions:

- is SQL Server healthy before init runs?
- does the API wait for initialization to complete?
- is telemetry collector reachable?
- does Prometheus depend on upstream services?
- do health checks prove real readiness or only process existence?

Those questions transfer directly to larger production systems.

## Common Failure Modes

### Treating Migrations As The Only Risky Part

Application startup behavior, initialization order, and missing telemetry can sink a release even when the SQL is fine.

### Shipping Without A Watch Plan

If nobody knows what metrics or endpoints to watch, the team is flying blind.

### Confusing “Container Running” With “System Ready” 

Readiness means the service can do useful work, not just that the process exists.

### Removing Compatibility Too Early

Safe migrations often need a period where both old and new paths coexist.

## A Good Practice Sequence

1. Read the Compose stack from top to bottom.
2. Work through [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) for one schema-affecting change.
3. Identify which health, metric, and log signals would confirm success.
4. Write one rollback or roll-forward note for a migration scenario.
5. Explain what would stop you from deploying today.

## Exit Criteria

You are ready for Lesson 09 when you can do all of the following:

- review a change as an operational sequence instead of only a code diff
- define concrete smoke tests for the most important system paths
- name the metrics, logs, traces, and health signals you would watch during release
- explain when roll-forward is safer than rollback
- identify what would block a deployment before you start it

## Review Questions

1. Why should a data-platform change be treated as an operational sequence rather than only a code change?
2. What makes a smoke test useful during a release?
3. Why is a running container not the same thing as a ready system?
4. When can roll-forward be safer than rollback?
5. What should you decide before deployment begins about telemetry and observation?

## Suggested Answers

1. Because the change interacts with live services, dependencies, health checks, and rollback constraints, so release behavior matters as much as code correctness.
2. A useful smoke test quickly proves that the most important user or system path still works, rather than only confirming that a process started.
3. A container can be alive while the service is still uninitialized, failing health checks, unable to reach dependencies, or not yet able to serve real work.
4. Roll-forward can be safer when data or schema changes are hard to reverse cleanly but a compatible forward fix can restore stability with less risk.
5. You should know which health checks, logs, metrics, and traces will confirm success or reveal regression in the first minutes after release.

## Challenge Questions

1. Write a release checklist for a schema change that includes smoke tests, telemetry checks, and a fallback plan.
2. Explain how you would decide whether a failed release should be rolled back immediately or stabilized with a roll-forward fix.
3. Read the Compose stack as an on-call engineer and identify one dependency chain that could cause a misleadingly “healthy” local startup.

## Interview-Style Prompts

1. Explain to an interviewer what you would watch in the first five minutes after deploying a database-affecting API change.
2. Defend the idea that release safety is part of engineering quality, not a separate operations concern.

## Model Answer Rubrics

### Challenge Questions

A strong set of challenge answers should include:

- a release checklist that names concrete smoke tests, telemetry checks, and fallback posture
- one decision frame for choosing between rollback and stabilizing roll-forward
- evidence that the learner can read dependency ordering and readiness risk from the actual platform shape

### Interview-Style Prompts

A strong spoken answer should include:

- the first-minute signals that matter most: readiness, latency, errors, and dependency health
- the idea that safe release design begins before deployment, not after something breaks
- a clear link between application quality and operational quality instead of treating them as separate domains

## Lesson Checkpoint

- write a first-five-minutes watch plan for a release using health checks, metrics, and logs
- name one change where roll-forward is safer than rollback
- describe one smoke test that proves useful work instead of only process startup

## Next Lesson

Move to [Lesson 09](09-ef-core-dapper-and-query-shape.md) once operational concerns are part of your normal design process and you want to connect SQL thinking directly to application code.