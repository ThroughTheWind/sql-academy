# Telemetry Snapshot

## Baseline Signals Before The Window

- `/health/live`: healthy
- `/health/ready`: healthy
- request error rate: flat at background level
- request latency for order and post reads: within normal pre-release range
- OpenTelemetry traces and Prometheus metrics: available and updating

## Signals To Watch During The Window

- readiness changes during or after the enforcement step
- request latency spike on order reads or writes
- order-write failures that suggest the stricter contract is surfacing compatibility bugs
- restart loops or dependency connection churn in the API or worker

## Questions This Snapshot Should Answer

- do you have enough signal coverage to tell a safe release from a superficially healthy one?
- which signal would make you stop the rollout before users reported the problem?
- which signal would justify a controlled roll-forward instead of an immediate rollback attempt?