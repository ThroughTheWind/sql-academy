# Optional Solution

## Recovery Decision

Block the release until the recovery posture is refreshed. The newest recorded log backup is already 75 minutes old before the window starts, which misses the stated 15-minute RPO, and the current backup location has not been validated by a recent full restore drill.

## Required Restore Drill

1. Restore the latest full backup to an isolated SQL Server instance.
2. Apply the differential backup.
3. Apply each required log backup in order until the intended recovery point is reached.
4. Run `DBCC CHECKDB` on the restored copy.
5. Run `/health/ready`, one order read, and one order write against the restored environment before calling the fallback credible.

## Why This Blocks The Release

`RESTORE VERIFYONLY` says the backup file can be read. It does not prove that the current restore chain reaches the needed recovery point, that integrity checks pass, or that the restored application can do useful work inside the expected recovery time.

## What To Record Next Time

Record the exact chain used, the restore duration, the integrity result, and the post-restore smoke checks so the next release window can defend recovery posture from evidence instead of habit.
