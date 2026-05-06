# Hints

1. Decide what column or columns make the ordering fully deterministic.
2. If a cumulative calculation spans multiple users, revisit the partition clause.
3. Prefer window functions when you need both detail rows and analytical metadata.