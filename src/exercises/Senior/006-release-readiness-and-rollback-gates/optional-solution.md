# Optional Solution

## Go Or No-Go

Proceed only if the preflight checks still show zero null and duplicate `ExternalReference` values, the compatible API and worker build is already healthy, and the telemetry pipeline is known-good before the window starts.

Block the release if any of those facts is unverified, because the strict enforcement step becomes much harder to reverse once it commits under live traffic.

## First Five Minutes Watch Plan

1. Check `/health/ready` first so dependency or startup regressions are visible immediately.
2. Check request error rate and latency for the order path, because a compatibility bug can surface there even when readiness stays green.
3. Run one small order-read and order-write smoke path to prove useful behavior under the new contract.
4. Confirm traces and metrics are still flowing before declaring the window safe.

## Rollback And Roll-Forward Gates

- rollback gate: the migration has not yet committed and the preflight or immediate smoke checks already show unsafe results
- roll-forward gate: the schema step committed, but the regression is narrow enough that a compatible application fix or feature disablement is safer than trying to reverse the database state under traffic

The key idea is that rollback is not automatically safer. Once the stricter contract is live, a controlled roll-forward can preserve more correctness and stability than a rushed schema reversal.

## Post-Release Validation

Verify one order write and one order read under the stricter contract, then compare readiness, request latency, and error rate against the pre-release baseline.