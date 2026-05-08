# Phase 7: Operational Engineering

## Objectives

- treat data changes, deployments, and telemetry as engineering work rather than ceremony

## Prerequisites

- all previous SQL-centric phases

## Concepts

- migration rollout safety
- smoke tests and rollback planning
- metrics, logs, and traces during incidents
- release checklists and failure modes

## Exercises

- review a migration for operational risk
- work through [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) to make a go or no-go call before a schema-affecting release window begins
- trace an API regression through logs and metrics
- capture a release playbook for a schema change with backfill concerns

## Reference Tracks

- use [Operations Track](../operations/README.md) for release checklists, smoke surfaces, telemetry anchors, and CI guardrails
- use [Performance Track](../performance/README.md) when the incident hypothesis becomes latency, Query Store evidence, or query-shape regression

## Expected Outcomes

- the learner can describe what makes a migration safe in production
- the learner can use observability data to narrow an incident quickly

## Validation Checklist

- rollout and rollback steps are written down
- key runtime signals are identified before release
- learners can explain which gate would block the release before deployment begins
- learners can explain what would stop a deployment