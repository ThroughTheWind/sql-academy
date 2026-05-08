# Optional Solution

## Placement Decision

- keep `DatabaseTelemetrySnapshot` in `SqlAcademy.Worker` because it already depends on application hosting, dependency injection, and shared observability, and moving it to SQL Server Agent would weaken the current runtime boundary without improving the actual polling logic
- allow `OutboxConsistencySweep` to be a SQL Server Agent candidate only if it stays alert-oriented or read-only at first and proves missing-message drift without mutating delivery state blindly
- keep `SchemaPreflightGateCheck` as an explicit release-step owned by the operator, because a release gate should not become a blind recurring job that quietly turns red or green outside the change window

## Job-Safety Rules

- require one running instance at a time so overlap does not duplicate work or hide a stuck execution
- require a clear failure signal that pages or blocks follow-up action instead of silently retrying forever
- require one post-run validation query or smoke path for the chosen schedule before calling the job safe

## Post-Run Validation

- for the worker-owned telemetry loop, confirm the expected `LearningDb snapshot` log line and the configured interval remain visible
- for the SQL Server Agent candidate, verify the consistency sweep produces one explicit drift report and does not create duplicate outbox rows as a side effect
