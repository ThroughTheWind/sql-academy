# Expected Outcomes

- ready-to-ship orders 1, 2, and 4 each have one pending outbox message
- order 4 does not receive a duplicate message because one already exists
- the dispatch batch chooses the earliest two pending messages deterministically
- the final consistency check proves no ready order is missing an outbox entry