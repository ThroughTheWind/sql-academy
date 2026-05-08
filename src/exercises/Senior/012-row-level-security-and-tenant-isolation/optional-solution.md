# Optional Solution

The repo is now ready for a real row-level-security sample, but only for the dedicated `academy.TenantOrders` lab surface. That is the important distinction.

The sample works because three concrete pieces now line up:

## 1. The Sample Table Has A Real Tenant Contract

`academy.TenantOrders` now carries a durable `TenantId`, a composite unique index on `(TenantId, OrderNumber)`, and a dedicated access path index. That gives the policy a real boundary to evaluate.

## 2. The Runtime Now Owns Session Context For The Sample Path

`TenantSessionContextMiddleware` reads `X-Tenant-Id`, and `SqlSessionContextApplier` stamps the resulting `TenantId` into `SESSION_CONTEXT` whenever EF Core or Dapper opens a SQL connection. That makes the tenant boundary explicit instead of relying on application query filters alone.

## 3. The Policy Still Keeps Bypass Explicit

The sample uses `RlsBypass` and an optional `rls_policy_admin` role as named bypass surfaces instead of pretending broad runtime privilege is acceptable. The initializer and test fixtures must still opt into bypass on purpose.

What this sample does not prove is that the rest of the academy schema is ready for the same treatment. Core tables like `academy.Orders` still do not carry a tenant boundary, and the serving runtime still uses local-learning defaults that would need deeper privilege cleanup before a broader rollout was credible.

If the team later wants to extend RLS beyond this sample, the next shape would still be:


- narrow runtime credentials before policy rollout on core tables
- add and backfill durable tenant keys where the business model truly needs shared-database isolation
- prove tenant-allowed and tenant-denied behavior through focused tests like the sample already does
- document how to disable the policy or switch it off during emergency rollback if a first rollout behaves incorrectly

RLS still supplements application authorization instead of replacing it. The difference is that the repo now has one honest place where learners can watch that claim become real.