# Release Brief

## Change Window

- `13:45 UTC`: preflight review begins
- `14:00 UTC`: release window opens
- `14:05 UTC`: migration enforcement step is eligible to run if all gates pass
- `14:10 UTC`: compatible API and worker build should already be deployed and waiting for enforcement

## Planned Change

- enforce `academy.Orders.ExternalReference` as a required contract after the staged backfill work from [Advanced 002](../../Advanced/002-staged-backfill-and-contract-enforcement/README.md)
- keep API readiness, request latency, and error rate under the same watch window as the migration step
- use roll-forward first if the schema step commits but the runtime shows a narrow application regression

## Known Facts

- staging backfill runs completed without null or duplicate `ExternalReference` values
- the local platform already exposes `/health/live`, `/health/ready`, and `/metrics`
- the API and worker builds in the window were written to tolerate the additive column before enforcement
- direct rollback after a committed `NOT NULL` enforcement would require a follow-up migration or manual compatibility step, not a single instant undo button

## Risks To Decide Explicitly

- preflight queries may still reveal unexpected null or duplicate `ExternalReference` values in production
- readiness could stay green while request latency or order-write failures regress after the application build uses the stricter contract
- a partial rollback may be riskier than a controlled roll-forward if the schema step already committed and traffic is live