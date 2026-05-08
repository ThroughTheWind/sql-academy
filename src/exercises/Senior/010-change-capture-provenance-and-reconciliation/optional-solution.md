# Optional Solution

## Decision

Treat the current posture as improved but not yet fully credible for downstream replay. `Source`, `CorrelationId`, batch history, and row history make the import path traceable, but they do not yet prove one imported batch has one reviewable downstream outcome or that disputed movement can be reconciled without extra operator guesswork.

## Minimum Reconciliation Contract

1. Keep stamping `Source` and `CorrelationId` on every import batch so provenance remains queryable.
2. Use the import-batch and row endpoints first to isolate which batch and rows are being challenged.
3. Require one deterministic reconciliation query or downstream consistency check for the specific workflow that consumes imported trades.
4. Treat duplicate replay or unprovable downstream consumption as a stop condition instead of assuming provenance metadata solved the problem.

## Why This Is Not Full CDC Yet

The current platform already has better provenance than a blind import, and Senior 003 proves one strong outbox-style consistency contract. That still does not mean every data-moving workflow is replay-safe or audit-ready. Traceability metadata and deterministic reconciliation need to stay paired, or the operator is still guessing when a batch is disputed.
