# Incident Brief

## Timeline

- `09:28 UTC`: a release adds support for more `sortBy` options on the posts endpoint.
- `09:36 UTC`: request-latency alerts fire for `GET /api/v1/posts`.
- `09:40 UTC`: support reports that searches in the posts UI feel slow but still return `200` responses.
- `09:45 UTC`: readiness remains green, but API CPU and database work both climb.

## Known Facts

- the main affected route is [PostsController](../../../apps/SqlAcademy.Api/Controllers/V1/PostsController.cs)
- the main query path is [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs)
- requests that sort by `createdUtc` remain acceptable
- requests that sort by `commentCount` and include search text are the main regressors
- `/health/live` and `/health/ready` stay healthy, so this is a slow-path incident rather than a startup failure

## Constraints

- do not assume the whole service should be rolled back immediately
- do not change the schema blindly during the incident window
- prefer one containment move that reduces risk quickly and one follow-up change that needs proper validation

## Working Hypothesis Prompt

Use the repo evidence to answer three questions in order:

1. which boundary is slow?
2. what query shape likely changed or became more expensive?
3. what is the safest first mitigation while evidence is still incomplete?