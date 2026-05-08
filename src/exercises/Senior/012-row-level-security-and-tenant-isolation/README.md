# Senior 012: Row-Level Security And Tenant Isolation

## Objective

Use a real shared-database sample to trace how an incoming tenant header becomes SQL Server session context, prove that row-level security filters rows for both EF Core and Dapper paths, and identify which parts of the repo are still sample-only rather than globally multitenant.

## Exercise Type

This pack is a guided lab.

Completion is based on the README tasks, `investigation-template.md`, and `expected-outcomes.md`, not on `answer.sql` or `validation.sql`.

## Scenario

The repo now includes a dedicated `academy.TenantOrders` sample protected by a SQL Server security policy, plus app-side middleware that turns `X-Tenant-Id` into `SESSION_CONTEXT`. Your job is to prove that the sample works, understand why it works, and avoid overgeneralizing it into a claim that the whole academy schema is now multitenant.

## Repository Anchors

- [DBA And DBRE Extension Track](../../../../docs/learning/dba-dbre-extension-track.md)
- [Row-Level Security And Tenant Isolation](../../../../docs/learning/row-level-security-and-tenant-isolation.md)
- [Lesson 04: Schema Design And Migration Safety](../../../../docs/learning/04-schema-design-and-migration-safety.md)
- [Senior 011: Least Privilege And Operational Security Boundaries](../011-least-privilege-and-operational-security-boundaries/README.md)
- [Schema bootstrap SQL](../../../../db/schemas/001_create_learning_db.sql)
- [TenantSessionContextMiddleware](../../../apps/SqlAcademy.Api/Infrastructure/TenantSessionContextMiddleware.cs)
- [TenantOrdersController](../../../apps/SqlAcademy.Api/Controllers/V1/TenantOrdersController.cs)
- [TenantOrderReadService](../../../libs/SqlAcademy.Persistence/Queries/TenantOrders/TenantOrderReadService.cs)
- [SqlSessionContextApplier](../../../libs/SqlAcademy.Persistence/MultiTenancy/SqlSessionContextApplier.cs)
- [SqlAcademy.Api.http](../../../apps/SqlAcademy.Api/SqlAcademy.Api.http)
- [TenantOrdersEndpointTests](../../../../tests/SqlAcademy.IntegrationTests/Api/TenantOrdersEndpointTests.cs)
- [RowLevelSecuritySampleTests](../../../../tests/SqlAcademy.PerformanceTests/RowLevelSecuritySampleTests.cs)
- [Operations Track](../../../../docs/operations/README.md)

## Assets

- `rls-brief.md` frames the shared-database tenant-isolation pressure.
- `starter-policy.sql` shows one concrete SQL Server policy shape using `SESSION_CONTEXT`, a predicate function, and an explicit bypass role.
- `tenant-context-surfaces.md` summarizes the current schema and runtime facts that make or break an honest RLS rollout.
- `policy-failure-modes.md` summarizes the filter, block, and operator risks that still matter.
- `investigation-template.md` is the learner-editable workbook.
- `expected-outcomes.md` defines what a strong row-level-security answer should conclude.
- `hints.md` gives progressively more direct guidance.
- `optional-solution.md` shows one defensible adoption decision.

## Baseline Run

1. Run `dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~TenantOrdersEndpointTests"`.
2. Run `dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~RowLevelSecuritySampleTests"`.
3. Read [TenantSessionContextMiddleware](../../../apps/SqlAcademy.Api/Infrastructure/TenantSessionContextMiddleware.cs), [SqlSessionContextApplier](../../../libs/SqlAcademy.Persistence/MultiTenancy/SqlSessionContextApplier.cs), and [starter-policy.sql](starter-policy.sql) before changing anything.
4. Optionally call the sample requests in [SqlAcademy.Api.http](../../../apps/SqlAcademy.Api/SqlAcademy.Api.http) with and without `X-Tenant-Id` while the API is running locally.

## Tasks

1. Trace the tenant identity path from `X-Tenant-Id` through `TenantSessionContextMiddleware`, `SqlSessionContextApplier`, and `sys.sp_set_session_context`.
2. Explain why `academy.TenantOrders` is a credible sample table for RLS while the rest of the academy schema still is not a shared-tenant model by default.
3. Use the focused tests or HTTP requests to prove three cases: tenant `101` sees only its rows, tenant `202` sees only its rows, and no tenant header sees none.
4. Read `starter-policy.sql` and explain what the filter predicate, block predicates, and explicit bypass key are protecting against.
5. Finish `investigation-template.md` with one concrete proof of isolation and one condition that would make you postpone applying the same pattern to a non-sample table.

## Validation

- the learner can point to the exact middleware, connection, and policy surfaces that make the sample work
- the focused endpoint and performance tests both pass while proving tenant-allowed and tenant-denied behavior
- the final write-up distinguishes the dedicated sample from a blanket claim that the whole application is already multitenant

## Focused Companion Check

When you want narrow executable checks for the lab, run:

`dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~TenantOrdersEndpointTests"`

`dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~RowLevelSecuritySampleTests|FullyQualifiedName~RowLevelSecurityTenantIsolationDrillAssetTests"`

Those checks verify the runtime sample behavior and keep the lesson, policy sketch, workbook prompts, and optional solution aligned.