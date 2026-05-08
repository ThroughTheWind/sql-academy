# Row-Level Security And Tenant Isolation Runbook

Use this as a printable checklist when a shared-database tenant boundary needs a quick reality check: is the tenant context being stamped, is the security policy attached, and is the bypass path explicit instead of accidental?

This runbook is a companion to [Senior 012: Row-Level Security And Tenant Isolation](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md), [Row-Level Security And Tenant Isolation](../learning/row-level-security-and-tenant-isolation.md), and [Operations Track](README.md).

It is not a replacement for the guided lab. It is the compressed checklist for verifying the dedicated `academy.TenantOrders` sample and for deciding whether the same pattern is credible anywhere else.

## Tenant Context Gate

- confirm which request or worker boundary sets `X-Tenant-Id` or the equivalent tenant value first
- verify that [TenantSessionContextMiddleware](../../src/apps/SqlAcademy.Api/Infrastructure/TenantSessionContextMiddleware.cs) or an equivalent runtime edge owns tenant parsing before the query path opens a SQL connection
- reject the sample as incomplete if tenant identity is still implicit in controller filters or query parameters instead of `SESSION_CONTEXT`

## Policy Proof

- check that `security.TenantOrderIsolationPolicy` is attached to `academy.TenantOrders`
- prove tenant `101` and tenant `202` see different rowsets through the same read surface
- verify that a request without tenant context returns no rows instead of silently falling back to broad access
- keep [TenantOrdersEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/TenantOrdersEndpointTests.cs) and [RowLevelSecuritySampleTests](../../tests/SqlAcademy.PerformanceTests/RowLevelSecuritySampleTests.cs) as the first executable proof before trusting screenshots or ad hoc query output

## Controlled Bypass Review

- name the explicit bypass path: `RlsBypass`, `rls_policy_admin`, or a narrower equivalent
- require migrations, seeding, and maintenance workflows to opt into bypass on purpose instead of inheriting broad runtime privilege
- stop the rollout if the bypass path is still “run as admin and hope” rather than a named reviewed mechanism

## Disable And Recovery

- document how to disable `security.TenantOrderIsolationPolicy` quickly if the policy blocks useful work
- record which seed or maintenance workflows require bypass before an incident happens
- if the first failure mode is confusing missing rows, check session context first before assuming data loss
- keep one explicit rollback note for the policy and one explicit ownership note for who can bypass it

## Drill Links

- [Senior 012: Row-Level Security And Tenant Isolation](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md)
- [Row-Level Security And Tenant Isolation](../learning/row-level-security-and-tenant-isolation.md)
- [Operations Track](README.md)