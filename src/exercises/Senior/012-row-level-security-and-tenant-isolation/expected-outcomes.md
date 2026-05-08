# Expected Outcomes

- the learner identifies the exact runtime path that moves `X-Tenant-Id` into `SESSION_CONTEXT`
- the learner explains why `academy.TenantOrders` is a credible RLS sample without claiming the rest of the schema is automatically tenant-ready
- the answer includes one concrete proof that tenant `101` and tenant `202` are isolated plus one case where missing tenant context returns no rows
- the write-up still names one prerequisite that would be required before applying the same pattern to non-sample tables