# Expected Outcomes

- the existing external reference stays unchanged
- null contract values are backfilled with deterministic legacy values
- only the backfilled rows appear in the batch plan
- the enforcement check proves the column can safely move to a stricter contract later