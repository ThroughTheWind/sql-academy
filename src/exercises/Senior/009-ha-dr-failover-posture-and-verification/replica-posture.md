# Replica Posture

## Expected Recovery Targets

- target RPO = 5 minutes
- target RTO = 10 minutes

## Current Failover Evidence

| Surface | Current Fact | Why It Matters |
| --- | --- | --- |
| primary role | `sql-primary` currently owns the write path | the maintenance window requires a role change if failover becomes real |
| failover target | `sql-secondary` is documented as the warm standby | the target must absorb real order traffic, not only appear reachable |
| data movement lag | last observed replica lag is 18 minutes | this already misses the stated 5-minute RPO |
| rehearsal freshness | no successful planned failover drill is recorded after 2026-02-11 | stale drill evidence weakens confidence before the maintenance window |
| application proof | no recorded `/health/ready`, order read, or order write check exists against the failover target | failover is not credible until useful work is proven after the role change |
