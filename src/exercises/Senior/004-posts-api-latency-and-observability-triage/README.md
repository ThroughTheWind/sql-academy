# Senior 004: Posts API Latency And Observability Triage

## Objective

Trace a production-style API regression across application code, SQL query shape, and telemetry instead of blaming one layer in isolation.

## Exercise Type

This pack is an investigation pack.

Complete `investigation-template.md` as the learner-editable workbook. This pack does not use `answer.sql` or `validation.sql`.

## Scenario

After a release, `GET /api/v1/posts` stays healthy but becomes slow for requests that sort by comment count while also using search. You need to narrow the boundary, explain the likely SQL shape behind the slowdown, and propose a safe containment and follow-up plan.

## Assets

- `incident-brief.md` frames the timeline and known facts.
- `starter.sql` gives diagnostic SQL you can run against `LearningDb`.
- `broken.sql` captures the risky query shape to review.
- `logs-snapshot.md` summarizes the request logs during the incident.
- `metrics-snapshot.md` summarizes the most useful metrics.
- `trace-snapshot.md` summarizes the most useful trace clues.
- `investigation-template.md` is the learner-editable response workbook.
- `expected-outcomes.md` defines what a strong investigation should conclude.
- `hints.md` gives progressive guidance.
- `optional-solution.md` shows one defensible triage answer.

## Tasks

1. Build the first five minutes of the investigation using logs, metrics, traces, and one API boundary.
2. Identify the most likely read path and SQL shape behind the latency increase.
3. Propose one immediate containment step and one durable fix that preserve safe rollout discipline.
4. Define what additional telemetry or tests should exist before the next release of this path.

## Validation

Use `investigation-template.md` as the main completion artifact.

- the learner narrows the issue to the posts read path instead of blaming the whole platform
- the learner connects `commentCount` sorting plus search to a heavier grouped and sorted SQL path
- the response includes containment, durable remediation, and follow-up validation signals

## Focused Companion Check

When you want a narrow executable check for the pack contract itself, run:

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~PostsApiObservabilityDrillAssetTests"`

That asset smoke test verifies the incident pack keeps its logs, metrics, traces, workbook prompts, and expected-outcomes contract aligned without pretending to automate the actual investigation.
