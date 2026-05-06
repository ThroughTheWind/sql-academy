# Hints

1. Start by measuring what the query actually filters and sorts on.
2. A covering index is useful only when the extra write cost is justified.
3. Safe migrations often require nullable introduction, backfill, validation, then enforcement.