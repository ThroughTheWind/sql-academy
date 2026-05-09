# Expected Outcomes

- you can explain [PostReadService](../../../libs/SqlAcademy.Persistence/Queries/Posts/PostReadService.cs) as base rowset, optional filters, deterministic sorting, server-side paging, and direct projection
- you can explain [TradeReadService](../../../libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs) as explicit select text, whitelisted sort behavior, deterministic tie-breaker, and paired count query
- the focused posts or trades tests still pass after one deliberate contract change
- you can defend why the posts read path is a strong EF Core candidate and why the trades read path is a strong Dapper candidate