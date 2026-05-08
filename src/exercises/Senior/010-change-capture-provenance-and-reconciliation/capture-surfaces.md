# Capture Surfaces

## Current Provenance And Movement Evidence

| Surface | Current Fact | Why It Matters |
| --- | --- | --- |
| import API | `POST /api/v1/trades/import` accepts `Source` and `CorrelationId` | the ingest path can already stamp provenance at write time |
| batch history | `GET /api/v1/trades/import-batches` and related batch detail endpoints expose processed import history | operators can inspect what batch was processed and when |
| metadata model | `TradeImportBatches` now stores `CorrelationId` and `Source` with supporting indexes | provenance is queryable instead of buried in logs only |
| row history | import batch rows can be queried by batch and row number | replay and dispute handling need row-level evidence, not only batch counts |
| consistency contract | `Senior 003` already proves one idempotent outbox pattern with deterministic batching and a consistency check | capture credibility still depends on reconciliation, not only on metadata columns |
