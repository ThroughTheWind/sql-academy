# Senior 011: Least Privilege And Operational Security Boundaries

## Objective

Decide whether the current runtime and migration security posture is credible beyond local learning by reviewing convenience credentials, startup migration authority, and the minimum separation-of-duties rules needed before calling the platform defensible.

## Exercise Type

This pack is an investigation pack.

Complete `investigation-template.md` as the learner-editable workbook. This pack does not use `answer.sql` or `validation.sql`.

## Scenario

The team wants to promote the current stack shape into a shared environment with minimal changes. The API and worker both connect as `sa`, the API auto-applies migrations on startup, the design-time migration factory has a privileged fallback connection string, and Grafana admin credentials are still treated as simple environment variables. Before you call that acceptable, you need to decide which boundaries are just local-learning convenience and which ones must change to preserve least privilege and operational separation of duties.

## Repository Anchors

- [DBA And DBRE Extension Track](../../../../docs/learning/dba-dbre-extension-track.md)
- [Lesson 04: Schema Design And Migration Safety](../../../../docs/learning/04-schema-design-and-migration-safety.md)
- [Senior 006: Release Readiness And Rollback Gates](../006-release-readiness-and-rollback-gates/README.md)
- [SqlAcademy.Api startup](../../../apps/SqlAcademy.Api/Program.cs)
- [SqlAcademy.Api appsettings](../../../apps/SqlAcademy.Api/appsettings.json)
- [SqlAcademy.Worker appsettings](../../../apps/SqlAcademy.Worker/appsettings.json)
- [DesignTimeLearningDbContextFactory](../../../libs/SqlAcademy.Migrations/DesignTimeLearningDbContextFactory.cs)
- [docker-compose runtime credentials](../../../../docker-compose.yml)
- [Operations Track](../../../../docs/operations/README.md)

## Assets

- `security-brief.md` frames the shared-environment promotion pressure.
- `privilege-surfaces.md` summarizes the current runtime, migration, and admin credential surfaces.
- `boundary-gaps.md` summarizes the least-privilege gaps that still matter.
- `investigation-template.md` is the learner-editable security workbook.
- `expected-outcomes.md` defines what a strong least-privilege answer should conclude.
- `hints.md` gives progressively more direct guidance.
- `optional-solution.md` shows one defensible security-boundary decision.

## Tasks

1. Decide whether the current security posture is acceptable only for local learning or still too permissive for any shared environment.
2. Identify one runtime credential fact and one migration-authority fact that matter most to that decision.
3. Write the minimum separation-of-duties checklist you would require before calling the platform defensible.
4. Name one operational check that proves a boundary really changed instead of remaining a comment or a hope.

## Validation

Use `investigation-template.md` as the main completion artifact.

- the learner makes an explicit boundary decision instead of only repeating generic least-privilege slogans
- the response ties the decision to `sa` convenience access, startup migration authority, and one operator-admin surface
- the proposed plan distinguishes local-learning shortcuts from the minimum controls needed in a shared environment

Use [Least Privilege And Security Boundary Runbook](../../../../docs/operations/least-privilege-security-runbook.md) when you want the same least-privilege and separation-of-duties review compressed into a printable operational checklist.

## Focused Companion Check

When you want a narrow executable check for the pack contract itself, run:

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~LeastPrivilegeSecurityBoundaryDrillAssetTests"`

That asset smoke test verifies the pack keeps its privilege surfaces, least-privilege gaps, workbook prompts, and optional-solution contract aligned without pretending to automate real database role design.
