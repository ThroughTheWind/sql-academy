# Investigation Template

## Recovery Target Review

- is the planned maintenance window actually compatible with the stated RPO and RTO targets?
- which one business or traffic fact makes those targets non-negotiable?

## Replica And Lag Review

- which replica or failover fact matters most to the go or no-go decision?
- what does the current lag or rehearsal evidence say about likely data loss or recovery delay?
- what missing fact would still have to be proven before calling the failover path credible?

## Failover Decision

- is the current HA or DR posture good enough for the window, or should the plan be blocked?
- which one failover-readiness gap drove that decision?
- what exact pre-failover checklist would you require before proceeding?

## Post-Failover Validation

- which one smoke path proves the application can still do useful work after the role change?
- which one recovery-time or role-stability check proves the failover stayed inside objective?
- what evidence should be recorded so the next operator does not have to guess again?
