# Senior 005: EF Core N+1 And Generated SQL Investigation

## Objective

Prove that the posts read path stays projection-first and one-query, then build a disposable N+1 experiment so you can explain the failure mode with evidence instead of theory.

## Exercise Type

This pack is a guided lab.

Completion is based on the README tasks, `investigation-template.md`, and `expected-outcomes.md`, not on `answer.sql` or `validation.sql`.

## Scenario

The posts endpoint still returns the right shape after a refactor, but a reviewer is worried that the next "small readability change" could silently introduce N+1 behavior or hide expensive generated SQL behind clean-looking LINQ. You need one focused workflow that proves the current query shape and shows exactly what would make it regress.

## Repository Anchors

- [PostsController](../../../apps/SqlAcademy.Api/Controllers/V1/PostsController.cs)
- [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs)
- [PostsEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs)
- [TrackingBehaviorTests](../../../../tests/SqlAcademy.PerformanceTests/TrackingBehaviorTests.cs)
- [Lesson 09: EF Core, Dapper, And Query Shape](../../../../docs/learning/09-ef-core-dapper-and-query-shape.md)

## Assets

- `investigation-template.md` is the learner-editable completion artifact.
- `expected-outcomes.md` defines what a strong investigation should conclude.
- `hints.md` gives progressively more direct guidance.
- `optional-solution.md` shows one defensible investigation path.

## Baseline Run

1. Run `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"`.
2. Run `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~TrackingBehaviorTests"`.
3. Read [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs) and write down the current rowset, filters, sort path, and projection.

## Tasks

1. Capture or describe the generated SQL shape for the current posts read path. If you need a concrete workflow, create a temporary diagnostic query in a scratch test, playground file, or throwaway branch and inspect the generated SQL with EF Core tooling or logging.
2. Explain why the current `AsNoTracking()` plus direct projection shape is not the classic N+1 pattern.
3. Build one disposable N+1 experiment around the posts read path. For example, materialize posts first and then fetch comment counts or author data per row in a loop. Do not leave the risky shape in the mainline code after the investigation.
4. Record what evidence proved the N+1 risk: repeated SQL statements, extra round trips, or a clearly worse generated SQL story.
5. Finish `investigation-template.md` with one concrete regression guard you would keep in the repository, such as a focused test, a logging recipe, or a code-review rule.

## Verification

- the learner can point to the exact projection-first query shape that keeps the current path read-only and compact
- the generated SQL is inspected or described explicitly rather than assumed from the LINQ alone
- the disposable experiment demonstrates how an N+1 regression would appear in practice
- the final write-up names one concrete regression guard that would make the next refactor safer

## Exit Criteria

You are done when you can show the current safe shape, explain the disposable unsafe shape, and defend one concrete way to catch that regression before users do.

## Completion Checklist

- [ ] you captured or described the generated SQL for the current safe posts read path instead of inferring it only from LINQ
- [ ] you demonstrated one disposable N+1 experiment and removed the risky shape from the mainline path afterward
- [ ] `investigation-template.md` records the current safe shape, the unsafe experiment, and one concrete regression guard
- [ ] you can explain why the current projection-first path is not the classic N+1 pattern

## Focused Companion Checks

Use these when you want the narrowest executable evidence around the posts read path while you work:

- `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"`
- `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~TrackingBehaviorTests"`

These checks do not automate the investigation itself, but they keep the current posts contract and tracking behavior anchored while you inspect generated SQL and build the disposable N+1 experiment.