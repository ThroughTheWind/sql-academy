# Incident Triage Runbook

Use this as a printable checklist for slow-but-healthy API incidents and other problems where the system is degraded rather than fully down.

This runbook is a companion to [Lesson 10: Observability, Testing, And Performance Engineering](../learning/10-observability-testing-and-performance-engineering.md) and [Senior 004: Posts API Latency And Observability Triage](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md).

It is not a replacement for the investigation pack. It is the compressed checklist for narrowing the boundary before the incident grows wider than the evidence.

## First Signal

- name the one signal that made the incident real: latency, error rate, timeouts, or a failed smoke path
- confirm the problem is current instead of relying on stale dashboards or hearsay
- separate "service unavailable" from "service healthy but slow" before choosing the next check

## Narrowed Boundary

- pick one endpoint, service, or query path as the first boundary
- use one healthy control path to avoid treating the whole platform as suspect
- name the most likely owner surface before proposing a fix
- prefer a concrete boundary such as `GET /api/v1/posts` or `PostReadService` over a vague "database issue"

## Logs, Metrics, And Traces

- use logs to confirm request shape, timing, and obvious failures
- use metrics to see whether p95 latency, request rate, or dependency health changed materially
- use traces to find the hottest span or query boundary before opening a broad code hunt
- record which signal ruled out a full-stack outage

## Immediate Containment

- choose the smallest safe containment that reduces impact without improvising a risky schema or deployment change
- keep a healthy default path available when a more expensive query shape or feature flag is the likely trigger
- state what you will watch immediately after containment to decide whether it actually helped

## Durable Fix And Follow-Up Validation

- name the code, SQL, index, telemetry, or contract change that should prevent recurrence
- add one focused test, benchmark, or lab rerun that would make the next regression easier to detect
- add one telemetry improvement if the current incident was slower to narrow than it should have been
- record the final hypothesis together with one disconfirming check so the next engineer can challenge it

## Drill Links

- [Senior 004: Posts API Latency And Observability Triage](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)
- [Performance Track](../performance/README.md)
- [Operations Track](README.md)
