# Senior 006: Release Readiness And Rollback Gates

## Objective

Decide whether a database-affecting release is ready to ship by combining migration safety, telemetry checks, smoke surfaces, and explicit rollback or roll-forward gates.

## Exercise Type

This pack is an investigation pack.

Complete `investigation-template.md` as the learner-editable workbook. This pack does not use `answer.sql` or `validation.sql`.

## Scenario

A release will enforce `ExternalReference` on `academy.Orders` and deploy application code that assumes every order carries that contract. The staged backfill plan already exists, but you still need a go or no-go decision, a first-five-minutes watch plan, and explicit rollback gates before the release window starts.

## Repository Anchors

- [Lesson 08: Operational Engineering And Release Safety](../../../../docs/learning/08-operational-engineering-and-release-safety.md)
- [Advanced 002: Staged Backfill And Contract Enforcement](../../Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Senior 004: Posts API Latency And Observability Triage](../004-posts-api-latency-and-observability-triage/README.md)
- [API startup and health endpoints](../../../apps/SqlAcademy.Api/Program.cs)
- [OpenTelemetry service registration](../../../libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs)
- [Migration strategy notes](../../../../db/migrations/README.md)

## Assets

- `release-brief.md` frames the change window, target change, and known risks.
- `migration-review.md` summarizes the migration sequence and the preflight facts that matter for go or no-go.
- `telemetry-snapshot.md` summarizes readiness, latency, and error-rate baselines for the release watch window.
- `investigation-template.md` is the learner-editable release workbook.
- `expected-outcomes.md` defines what a strong release-readiness answer should conclude.
- `hints.md` gives progressively more direct guidance.
- `optional-solution.md` shows one defensible release decision and gate design.

## Tasks

1. Decide whether the release is ready to start or should be blocked, and cite the migration or telemetry gates that support that decision.
2. Write the first five minutes of the watch plan using concrete health, metric, log, and trace signals.
3. Define one rollback gate and one roll-forward gate, and explain why each is safer than a vague "watch it closely" posture.
4. Name one post-release validation check that proves the migration and the application remained compatible under live traffic.

## Validation

Use `investigation-template.md` as the main completion artifact.

- the learner makes an explicit go or no-go decision instead of only listing concerns
- the response includes at least one migration safety gate, one telemetry gate, and one rollback or roll-forward trigger
- the watch plan names concrete repo signals rather than generic monitoring slogans
- the post-release validation step proves both application usefulness and schema compatibility