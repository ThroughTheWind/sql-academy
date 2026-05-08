# Backup Inventory

## Expected Recovery Targets

- target RPO = 15 minutes
- target RTO = 30 minutes

## Current Backup Evidence

| Backup Type | Timestamp | Artifact | Verification Note |
| --- | --- | --- | --- |
| full | 2026-05-08 01:00 UTC | `LearningDb_full_20260508_0100.bak` | `RESTORE VERIFYONLY` passed |
| differential | 2026-05-08 12:00 UTC | `LearningDb_diff_20260508_1200.bak` | no full restore drill recorded against the current storage path |
| log | 2026-05-08 12:15 UTC | `LearningDb_log_20260508_1215.trn` | chain present |
| log | 2026-05-08 12:30 UTC | `LearningDb_log_20260508_1230.trn` | chain present |
| log | 2026-05-08 12:45 UTC | `LearningDb_log_20260508_1245.trn` | chain present |
| log | 2026-05-08 13:00 UTC | `LearningDb_log_20260508_1300.trn` | chain present |

## Observed Risk

- the planned release window starts at 14:15 UTC, so the newest recorded log backup is already 75 minutes old
- that log-backup age is outside the stated 15-minute RPO before the change even begins
