# Hints

1. Check whether a pending message already exists before inserting a new one.
2. Deterministic dispatch order needs more than `TOP (2)`.
3. A useful consistency check compares ready orders with pending messages by order identifier.