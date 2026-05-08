# Investigation Template

## Job Placement Decision

- which candidate workload belongs in SQL Server Agent?
- which candidate workload should stay in `SqlAcademy.Worker`?
- which workload should remain an explicit release-step or operator-controlled gate?

## Overlap And Idempotency Risk

- what goes wrong if the scheduled job overlaps with itself?
- what makes the chosen job safe or unsafe to replay?
- what duplicate-work or side-effect risk matters most?

## Failure And Retry Rule

- what failure should page or stop the workflow instead of retrying automatically?
- when would retry be safe, and when would it only hide a deeper design problem?
- what single-instance or lock rule would you require?

## Observability And Post-Run Validation

- which log, metric, or trace signal proves the job actually ran as intended?
- which post-run query, smoke path, or integrity check proves the outcome is useful?
- what evidence should be recorded so the next operator does not have to guess again?
