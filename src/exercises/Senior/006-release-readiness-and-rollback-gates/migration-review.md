# Migration Review

## Planned Sequence

1. Confirm `NullExternalReferenceCount = 0` and `DuplicateExternalReferenceCount = 0` before enforcement.
2. Confirm the compatible API and worker build is already deployed and healthy before the strict schema step begins.
3. Apply the enforcement step for `academy.Orders.ExternalReference`.
4. Run one small order-read and order-write smoke surface immediately after the migration.
5. Hold the observation window open until readiness, latency, and error rate stay stable.

## Preflight Facts

- `NullExternalReferenceCount = 0`
- `DuplicateExternalReferenceCount = 0`
- `BackfillBatchFailures = 0`
- readiness endpoints are already returning healthy values before the window starts
- Prometheus and OpenTelemetry pipelines are reachable before the schema step begins

## Gate Questions

- what exact result would block the enforcement step?
- which runtime signal would force an immediate pause even if the migration itself succeeds?
- when is roll-forward safer than trying to reverse the schema step immediately?