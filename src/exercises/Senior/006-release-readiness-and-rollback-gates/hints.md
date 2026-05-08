# Hints

1. Start with the migration gate that could make the enforcement step unsafe even before deployment begins.
2. A green readiness endpoint is necessary, but it is not sufficient if latency or order-write failures regress immediately after release.
3. If the schema step already committed, ask whether a controlled roll-forward preserves compatibility more safely than scrambling to reverse the database change.
4. Your smoke test should prove useful work on the order path, not only that the process is alive.