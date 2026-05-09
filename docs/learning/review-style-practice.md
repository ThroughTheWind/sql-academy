# Review-Style Practice

Use this optional guide after Lessons 08 through 11 when you want to practice reviewer judgment instead of only authoring solutions.

These drills reuse existing repository surfaces, validation packs, and focused companion checks so the review work stays grounded in the real course material.

## How To Use This Guide

1. Read the listed repository anchors as if they were the changed surfaces in a pull request.
2. Write a short review note that makes a decision, names one concrete risk, and points to one validating check.
3. If you think the change is safe with conditions, say what must happen before approval.
4. Run the narrowest validation anchor when you want to confirm the contract you are reviewing still holds.

## Review Note Template

Use this shape for each drill:

1. Review decision: approve, approve with follow-up, or request changes.
2. Main concern: what could fail, drift, or become unsafe?
3. Evidence: which repository anchor or artifact supports that concern?
4. Validation anchor: what is the narrowest check that should pass before confidence increases?
5. Safer path: what change, gate, or follow-up would make the proposal easier to approve?

## Drill 1: Migration Safety Review

### Review Goal

Review a database-affecting change that adds and enforces `ExternalReference` on orders without turning a staged rollout into a risky one-shot migration.

### Repository Anchors

- [Advanced 002: Staged Backfill And Contract Enforcement](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- [Migration strategy notes](../../db/migrations/README.md)
- [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)

### Review Prompts

- Is the rollout additive before it becomes enforcing?
- What proof exists that the backfill is complete before the contract tightens?
- What application or release dependency would make this unsafe even if the migration itself succeeds?

### Narrow Validation Anchor

`powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Advanced/002-staged-backfill-and-contract-enforcement`

Use this when you want to confirm the staged backfill and contract-enforcement review surface still matches the expected pack outcome.

## Drill 2: Indexing Tradeoff Review

### Review Goal

Review an indexing or query-tuning proposal without approving it from plan screenshots, slogans, or one lucky workload sample.

### Repository Anchors

- [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](06-indexing-execution-plans-and-parameter-sensitivity.md)
- [Performance Track](../performance/README.md)

### Review Prompts

- Does the proposed index actually match the filter, sort, and projection shape being defended?
- What write-side or maintenance cost is being accepted in exchange for the read-path gain?
- Could parameter sensitivity or skew make the “fast plan” story less stable than the review claims?

### Narrow Validation Anchor

`powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety`

Use this when you want a deterministic contract check around the indexing and migration-safety review surface before accepting the proposal.

## Drill 3: Release Readiness Review

### Review Goal

Review a release plan as if you were the final approver who must decide whether the migration, telemetry posture, and rollback story are good enough to enter the window.

### Repository Anchors

- [Senior 006: Release Readiness And Rollback Gates](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)
- [SqlAcademy.Api startup and health endpoints](../../src/apps/SqlAcademy.Api/Program.cs)
- [OpenTelemetry service registration](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs)
- [Operations Track](../operations/README.md)

### Review Prompts

- Does the plan name a real go or no-go decision, or only a vague “watch it closely” posture?
- Are the first-five-minutes signals concrete enough to catch bad migrations, unhealthy readiness, or contract drift quickly?
- Is the rollback or roll-forward gate tied to useful work, not only container health?

### Narrow Validation Anchor

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~ReleaseReadinessDrillAssetTests"`

Use this when you want the narrowest executable proof that the release-readiness pack assets and decision contract still line up.

## What Strong Review Practice Looks Like

- the review comment names one concrete risk instead of asking for generic “more testing”
- the evidence comes from repository anchors, expected outcomes, or pack assets instead of intuition alone
- the validation anchor is specific enough that another reviewer could run it immediately
- the recommendation distinguishes blocking risk from acceptable follow-up work

## Next Step

After these drills, return to [Lesson 11: Capstones And Interview Readiness](11-capstones-and-interview-readiness.md) and rehearse the same judgment using the design-defense template instead of the reviewer lens.