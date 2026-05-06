# Hints

1. Search for loops that trigger additional database calls per row.
2. In optimistic concurrency, the point is to detect contention, not to hide it.
3. Bulk ingestion usually wants a staging boundary before touching the serving tables.