# Failover Readiness

## Last Recorded Failover Rehearsal

- last planned failover drill date: 2026-02-11
- evidence recorded: role-change transcript and one replica-state screenshot
- measured business validation: none recorded

## What Is Missing

- no recent drill measures whether the role change and application recovery stay inside the 10-minute RTO
- no post-failover `/health/ready`, order read, or order write result is recorded for the current application build
- no explicit stop condition is documented for starting the window when replica lag already exceeds the 5-minute RPO
- no operator note explains when the team should stay on the primary and reschedule maintenance instead of treating failover as mandatory
