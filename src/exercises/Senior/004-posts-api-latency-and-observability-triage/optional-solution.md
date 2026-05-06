# Optional Solution

## Likely Boundary

The strongest initial boundary is the posts read path: [PostsController](../../../apps/SqlAcademy.Api/Controllers/V1/PostsController.cs) delegates directly to [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs), and the trace snapshot concentrates almost all time in `ef.posts.list`.

## Likely Root-Cause Hypothesis

- requests sorted by `createdUtc` stay fast, which argues against a general API or serializer problem
- the slow requests combine broad text search, larger page size, and `commentCount` sorting
- that shape pushes the database toward more grouping and sorting work than the default recent-posts path
- the current telemetry tags page size and sort field, but not whether search text was present, so the incident is observable enough to narrow the boundary but not yet ideal for fast segmentation

## Immediate Containment

- cap or temporarily disable `commentCount` sorting for search-heavy requests
- keep the default `createdUtc` path available so the API remains functional
- watch route-specific latency and readiness while the containment rolls out

This is safer than improvising a schema change during the incident window.

## Durable Fix

- inspect the generated SQL for the `commentCount` sort path in [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs)
- compare the grouped sort path with the normal `createdUtc` path using performance tests or an isolated query review
- evaluate whether this route needs a dedicated read model, a persisted comment-count strategy, or a more targeted index review on `Posts` and `Comments`
- add telemetry that records whether search text was present so future traces and metrics segment cleanly by query shape

## Follow-Up Validation

- add an integration test that exercises `GET /api/v1/posts` with `sortBy=commentCount`
- add a performance comparison for the heavy search-plus-sort path
- compare before and after metrics for route latency, active requests, and database-facing span duration