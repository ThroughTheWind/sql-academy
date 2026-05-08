# Job Catalog

## Candidate Workloads

### DatabaseTelemetrySnapshot

- current behavior: `DatabaseTelemetryWorker` polls `LearningDb` and logs post, trade, and order counts on a configured interval
- proposed schedule: every 15 seconds
- main question: does moving this from the application worker into SQL Server Agent improve safety, or does it remove useful app-level observability and configuration control?

### OutboxConsistencySweep

- current concern: periodically detect ready-to-ship orders that are missing a pending outbox message
- proposed schedule: every 5 minutes
- main question: is this a safe database-local check for SQL Server Agent, or does it risk mutating delivery state without the same application-level controls used in [Senior 003](../003-transactional-outbox-and-delivery-consistency/README.md)?

### SchemaPreflightGateCheck

- current concern: confirm the same null and duplicate preflight facts used in [Senior 006](../006-release-readiness-and-rollback-gates/README.md) before an enforcement step begins
- proposed schedule: every 10 minutes
- main question: should a release gate become a blind recurring job, or stay an explicit change-window control owned by the operator?

## Decision Pressure

- overlapping runs must not duplicate work or hide failures
- the operator must still know which path to validate after the job runs
- the placement choice should preserve the clearest observability boundary rather than only moving code closer to the database
