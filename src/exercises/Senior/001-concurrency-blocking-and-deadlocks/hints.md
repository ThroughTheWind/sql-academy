# Hints

1. Use two query windows and keep one transaction open on purpose.
2. Draw the lock order on paper before proposing a fix.
3. In the graph, start with `victim-list`, then use each process `inputbuf` and resource owner or waiter edge to map the nodes back to Session A and Session B.
4. `logused` is one clue for why SQL Server might prefer one victim over another when priority is the same.
5. Retrying a deadlock is useful, but it does not excuse a poor transaction design.