# Phase 9: Capstone Projects

## Objectives

- synthesize schema design, SQL tuning, application integration, and operations into end-to-end delivery

## Prerequisites

- completion of the previous phases

## Concepts

- bounded-scope product requirements
- measurable non-functional requirements
- incident response and postmortem discipline
- interview-style system and query reasoning

## Capstone Menu

| Capstone | Use It When | Primary Repo Anchors | What A Strong Answer Must Prove |
| --- | --- | --- | --- |
| reporting slice with performance constraints | you want one endpoint or report that must meet a clear latency budget without hiding the SQL shape | [Lesson 06](../learning/06-indexing-execution-plans-and-parameter-sensitivity.md), [Lesson 09](../learning/09-ef-core-dapper-and-query-shape.md), [Advanced 005](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md), [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) | the learner can defend schema, query shape, projection, pagination, and runtime evidence together |
| release-safe schema evolution | you need an additive change, backfill plan, compatibility window, and explicit go or no-go release posture | [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md), [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md), [Release Runbook](../operations/release-runbook.md) | the learner can connect migration discipline, telemetry gates, useful-work smoke tests, and rollback or roll-forward logic |
| staged incident investigation | you need to narrow a degraded-but-live path without blaming the entire stack first | [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md), [Incident Triage Runbook](../operations/incident-triage-runbook.md), [Senior 001 deadlock graph lab](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/deadlock-graph-lab.md) | the learner can narrow the first boundary, explain the likely SQL or concurrency cause, choose safe containment, and name the durable fix |
| multi-domain delivery and release capstone | you want a final synthesis that combines application integration, operational consistency, and release discipline instead of one isolated query story | [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md), [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md), [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md), [Outbox Delivery Consistency Runbook](../operations/outbox-delivery-runbook.md) | the learner can defend write-side safety, delivery consistency, release sequencing, and post-release proof in one answer |

## Deeper Incident And Release Follow-Ons

Use these when the main Senior 004 or Senior 006 packs already feel predictable and you want a more demanding synthesis round.

### Incident Escalation Follow-On

Start with [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md), then add one concurrency or lock-contention angle from [Senior 001 deadlock graph lab](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/deadlock-graph-lab.md).

The goal is to distinguish query-shape regression from concurrency amplification before choosing containment.

### Release Escalation Follow-On

Start with [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md), then add one downstream or consistency boundary from [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) or one staged-ingestion boundary from [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md).

The goal is to prove that a release can still be unsafe even when the migration itself looks correct in isolation.

## Submission Expectations

For any capstone in this phase, keep all of the following visible:

- the exact problem statement and assumptions
- the workload or useful-work contract that matters most
- the main tradeoff accepted and the strongest rejected alternative
- the validation and telemetry surfaces that support the answer
- the first failure mode, the earliest signal, and the containment plan

## Expected Outcomes

- the learner can defend design and operational choices under scrutiny
- the learner can explain tradeoffs instead of presenting a single "best practice" answer
- the learner can connect at least two domains, such as query shape plus rollout safety or incident triage plus concurrency evidence, without collapsing into vague architecture talk

## Validation Checklist

- functional and non-functional requirements are both met
- observability data supports the learner's tuning claims
- the final write-up includes rejected alternatives and why they were rejected
- the answer names one useful-work smoke test and one failure signal that would stop the plan or redirect the incident response