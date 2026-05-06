# Hints

1. Ask whether you want to filter joined rows or preserve unmatched parent rows.
2. Compare `COUNT(*)`, `COUNT(child.Id)`, and `COUNT(DISTINCT parent.Id)`.
3. If a grouped result surprises you, inspect the pre-aggregate rowset first.