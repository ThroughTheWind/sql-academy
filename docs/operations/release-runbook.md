# Release Runbook

Use this as a printable checklist for schema-affecting or database-adjacent release windows.

This runbook is a companion to [Lesson 08: Operational Engineering And Release Safety](../learning/08-operational-engineering-and-release-safety.md) and [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md).

It is not a replacement for the investigation pack. It is the compressed operational checklist you should be able to defend after finishing that drill.

## Preflight Gates

- verify the migration sequence is still compatible with the currently deployed application path
- verify the strict or destructive step is blocked until the backfill or compatibility checks are complete
- verify `/health/ready`, logs, metrics, and traces are already trustworthy before the window opens
- verify one small read path and one small write path are known ahead of time as smoke checks
- decide in advance what result causes an immediate stop instead of a vague "watch it closely" response

## Window Start

- confirm the exact deployment order and who is responsible for each step
- confirm the current baseline for readiness, request latency, and error rate
- confirm the fallback posture: rollback if the change has not committed safely, roll forward only if compatibility is still intact and the fix path is clear
- start the window only if the migration-safety and telemetry gates are still green

## First Five Minutes Watch Plan

- check `/health/ready` first so dependency or startup failures are visible immediately
- check request error rate and latency for the path most exposed to the release
- run one small smoke test that proves useful work instead of only process startup
- confirm logs, metrics, and traces are still arriving before declaring the window healthy
- compare the first live signals against the pre-release baseline instead of relying on intuition

## Rollback Versus Roll-Forward

- choose rollback when the unsafe step has not committed cleanly and the fastest safe recovery is to stop and return to the previous state
- choose controlled roll-forward when compatibility is still intact, data risk is understood, and the safer path is to stabilize forward instead of rewinding partially applied work
- do not choose either posture without naming the concrete gate that triggers it

## Post-Release Validation

- verify the application and schema still behave compatibly under live traffic
- verify the smoke path remains healthy after the busiest part of the window begins
- verify telemetry still segments the affected path clearly enough for later incident review
- record the decision, the watch signals, and the final gate outcome while the facts are still fresh

## Drill Links

- [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)
- [Advanced 002: Staged Backfill And Contract Enforcement](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Operations Track](README.md)
