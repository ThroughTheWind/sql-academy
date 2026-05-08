# Optional Solution

## Failover Decision

Block the maintenance window until the HA or DR posture is refreshed. The failover target is already 18 minutes behind the primary, which misses the stated 5-minute RPO, and no recent drill proves the application can recover within the 10-minute RTO after the role change.

## Required Verification Before The Window

1. Confirm the failover target is inside the 5-minute RPO before maintenance begins, or explicitly reschedule the window.
2. Run a planned failover rehearsal against the current target and measure total recovery time against the 10-minute RTO.
3. Verify `/health/ready`, one order read, and one order write after the role change before calling the path credible.
4. Record one explicit stop condition for staying on the primary when lag or recovery time is already outside objective.

## Why This Blocks The Window

HA or DR posture is credible only when recovery targets, role movement, and useful work are proven together. A documented standby target and a green replica-state screenshot do not prove the system can absorb the write path safely inside the stated business objectives.
