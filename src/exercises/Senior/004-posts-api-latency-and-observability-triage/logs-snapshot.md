# Log Snapshot

```text
2026-05-06T09:36:12.144Z INF HTTP GET /api/v1/posts responded 200 in 2714.3 ms { pageNumber: 1, pageSize: 50, sortBy: commentCount, sortDirection: Desc, search: market }
2026-05-06T09:36:12.151Z INF Activity ef.posts.list completed in 2679.1 ms { posts.page_size: 50, posts.sort_by: commentCount }
2026-05-06T09:36:12.158Z WRN Request latency budget exceeded for GET /api/v1/posts { route: /api/v1/posts, durationMs: 2714.3 }
2026-05-06T09:36:15.006Z INF HTTP GET /health/ready responded 200 in 4.1 ms
2026-05-06T09:36:18.442Z INF HTTP GET /api/v1/posts responded 200 in 2491.8 ms { pageNumber: 1, pageSize: 50, sortBy: commentCount, sortDirection: Desc, search: sql }
2026-05-06T09:36:21.900Z INF HTTP GET /api/v1/posts responded 200 in 81.7 ms { pageNumber: 1, pageSize: 10, sortBy: createdUtc, sortDirection: Desc, search: null }
```

## What The Logs Suggest

- the route stays successful, so the main symptom is latency rather than outright failure
- the slow time is concentrated inside `ef.posts.list`
- page size and sort choice appear in telemetry, but the presence of search text does not