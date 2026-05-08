# Investigation Template

## Adoption Decision

- should the repo adopt row-level security now, only after prerequisites, or not at all for the current stack shape?
- which one missing fact makes that answer non-negotiable?

## Schema And Session Preconditions

- what tenant-key contract is missing or must be enforced first?
- who sets `SESSION_CONTEXT(N'TenantId')`, and how would you prove it happens on every relevant connection?
- which background workload, maintenance task, or reporting path is hardest to make safe?

## Policy Review

- which line in `starter-policy.sql` matters most to your decision?
- where could filter or block predicate behavior surprise developers or operators?
- which bypass path must be explicit instead of accidental?

## Proof And Rollback

- what one test or evidence set would prove tenant isolation is working?
- how would you disable or roll back a broken policy safely?
- what failure would force you to reject RLS until the platform changes?