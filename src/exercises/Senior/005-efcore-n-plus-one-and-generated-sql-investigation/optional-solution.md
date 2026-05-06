# Optional Solution

One defensible investigation path looks like this:

1. Start from [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs) and restate the current query in SQL terms: base rowset from posts, optional filters, stable sorting, and direct projection into `PostListItem`.
2. Capture generated SQL for an equivalent diagnostic query and confirm that the current path stays one-query and projection-first instead of issuing per-row lookups.
3. Build a disposable experiment that materializes posts before computing related values in a loop. The important proof is not the exact broken code shape; it is the evidence of repeated work per row.
4. Restore the safe shape and keep one concrete regression guard, such as a focused diagnostic recipe for generated SQL or a test that makes the intended contract easier to defend in review.

The core conclusion should be practical: readable LINQ is not enough. You still need generated-SQL evidence and a habit for spotting per-row access patterns before they ship.