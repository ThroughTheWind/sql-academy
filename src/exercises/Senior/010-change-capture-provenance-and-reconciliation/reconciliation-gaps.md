# Reconciliation Gaps

## What Is Still Missing

- no explicit reconciliation proof currently ties one imported trade batch to one downstream consumed or reviewed outcome
- `Source` and `CorrelationId` improve provenance, but they do not by themselves prove no rows were skipped, duplicated, replayed twice, or challenged after publication
- the current outbox-style consistency contract is demonstrated for orders, not automatically for every trade-import downstream workflow
- no operator-facing checklist currently states when batch history is good enough and when a stronger replay or reconciliation path is required
