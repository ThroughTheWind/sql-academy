# Metrics Snapshot

| Signal | Before Release | During Incident | Interpretation |
| --- | --- | --- | --- |
| `GET /api/v1/posts` p95 latency | 90 ms | 2.8 s | one endpoint regressed sharply |
| `GET /health/ready` p95 latency | 4 ms | 5 ms | the service is alive and ready |
| active requests on posts route | 2 | 19 | requests are stacking behind a slow path |
| API CPU | 22% | 71% | application work increased, but not enough to explain everything alone |
| database CPU | 18% | 78% | the hot path likely spends most time in query execution or sorting |

## What The Metrics Suggest

- this is a hot-path latency incident, not a full platform outage
- both the API and the database are busy, but the database-facing span should be the next place to look
- health signals staying green means you should avoid treating this as a binary up-or-down incident