# Restore Verification

## Last Recorded Restore Drill

- last full restore drill date: 2026-03-14
- restore target: isolated staging SQL Server instance
- evidence recorded: restore command transcript and one schema-open check

## What Is Missing

- no restore drill has been recorded since the backup artifacts moved to the current storage path
- `RESTORE VERIFYONLY` exists for the latest full backup, but no complete full-plus-differential-plus-log replay is recorded for the current chain
- no `DBCC CHECKDB` result is recorded on the restored copy
- no post-restore application smoke path is recorded, such as `/health/ready`, one order read, and one order write

## Interpretation

`RESTORE VERIFYONLY` is useful, but it is not the same as proving the current restore chain, integrity check, and post-restore smoke path all succeed inside the current recovery target.
