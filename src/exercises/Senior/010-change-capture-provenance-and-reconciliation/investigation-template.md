# Investigation Template

## Provenance Decision

- is the current data-movement posture already credible for downstream replay or audit?
- which one platform or business fact makes that answer non-negotiable?

## Capture Surface Review

- which provenance surface matters most: source, correlation, batch history, or row history?
- which consistency or replay fact is still missing from the current platform story?
- what would an operator still have to guess today?

## Reconciliation Requirement

- what minimum reconciliation checklist would you require before calling the movement contract safe?
- where would you prove no skipped, duplicated, or ambiguous work remains?
- when is metadata alone insufficient?

## Post-Movement Proof

- which endpoint, query, or consistency check would you use first after a disputed batch?
- what evidence should be recorded so the next operator can replay or challenge the batch without guesswork?
- what result would force you to stop claiming the path is audit-ready?
