# Tenant Context Surfaces

- the repo now defines a dedicated `academy.TenantOrders` sample with a durable `TenantId`, composite uniqueness, and a matching `security.TenantOrderIsolationPolicy`
- `TenantSessionContextMiddleware` turns `X-Tenant-Id` into scoped runtime state before the read service opens a SQL connection
- `SqlSessionContextApplier` stamps both `TenantId` and `RlsBypass` into `SESSION_CONTEXT`, so pooled connections do not keep stale tenant identity accidentally
- the sample proves shared-database isolation for one table, but the rest of the academy schema still uses application and user boundaries rather than a global tenant model
- startup migrations and seeding still require an explicit bypass path, which is why the policy sample keeps bypass state named and reviewable instead of hidden
- any credible rollout must define who can bypass the policy for support, reporting, or maintenance before the first `CREATE SECURITY POLICY` statement is enabled