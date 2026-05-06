# Trace Snapshot

```text
GET /api/v1/posts                                            2714 ms
  ef.posts.list                                              2679 ms
    db.query posts-list-with-comment-count-sort              2651 ms
```

## Observed Tags

- `posts.page_size = 50`
- `posts.sort_by = commentCount`

## Missing Tags That Would Help

- whether search text was present
- whether `AuthorId` was supplied
- a route or query-shape label that distinguishes `createdUtc` sorts from `commentCount` sorts more directly