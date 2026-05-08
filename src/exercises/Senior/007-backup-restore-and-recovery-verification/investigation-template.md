# Investigation Template

## Recovery Objective

- is the current release actually compatible with the stated RPO and RTO targets?
- which one business or change-window fact makes those targets non-negotiable?

## Backup Chain Review

- which full, differential, and log backups make up the intended restore chain?
- what evidence says the chain is intact or already unsafe?
- what fact about backup age matters most to the go or no-go decision?

## Restore Drill Decision

- is the current recovery posture good enough for the release, or should the window be blocked?
- which one restore-verification gap or stale drill fact drove that decision?
- what exact restore sequence would you require before trusting restore as a fallback?

## Post-Restore Validation

- which smoke path proves useful application work after the restore?
- which integrity or correctness check proves the restored database is trustworthy?
- what evidence should be recorded so the next release does not have to guess again?
