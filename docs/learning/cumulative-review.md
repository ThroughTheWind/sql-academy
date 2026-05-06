# Cumulative Review

This guide is a mixed assessment across the full learning path. Use it after you have completed several lessons, or use it at the end of the lesson sequence to find which topics still feel weak.

If you want narrower checkpoints first, use the level-scoped packs before coming back here:

- [Beginner Assessment Pack](beginner-assessment-pack.md)
- [Intermediate Assessment Pack](intermediate-assessment-pack.md)
- [Advanced Assessment Pack](advanced-assessment-pack.md)
- [Senior Assessment Pack](senior-assessment-pack.md)

## How To Use This Review

1. Answer the questions without looking at the lesson pages first.
2. Write short explanations, not only keywords.
3. For scenario questions, state both the likely issue and the evidence you would seek.
4. Use the answer key only after you have committed to your own explanation.

## Section 1: Fundamentals And Query Shape

1. Why is `ORDER BY` required if you want a query to mean “latest rows”?
2. What is the difference between a primary key and a unique business column?
3. Why is previewing an `UPDATE` target set with a `SELECT` a good default habit?
4. What is the row-shape difference between an inner join and a left join?
5. Why can a logically correct join still lead to an incorrect aggregate?
6. In a left-joined query, why can `COUNT(*)` and `COUNT(child.Id)` produce different answers?
7. When is a grouped aggregate the wrong tool because you still need row-level detail?

## Section 2: Analytical Querying And Pagination

8. What does `PARTITION BY` control in a window function?
9. Why is a stable tiebreaker necessary for pagination?
10. When would you choose `ROW_NUMBER()` over `RANK()`?
11. Why can a CTE improve correctness reasoning even if it does not improve runtime?
12. What goes wrong when a running-total query forgets to partition by the logical owner of the rows?

## Section 3: Schema Design And Change Safety

13. Why is a surrogate key not enough on its own if the business also requires uniqueness for fields like email or order number?
14. Why should nullability be treated as a business rule instead of as a deployment shortcut?
15. Why is an additive migration with backfill usually safer than a direct destructive change?
16. What makes a large backfill operationally risky under live traffic?
17. What is a compatibility window, and why does it matter during schema evolution?

## Section 4: Concurrency And Performance

18. What is the difference between blocking and deadlocking?
19. Why does transaction duration influence system behavior even when the SQL statements are individually correct?
20. Why does consistent lock ordering reduce deadlock probability?
21. What problem does `rowversion` solve in an optimistic concurrency flow?
22. Why should index design start with the query’s filter, join, and sort shape?
23. Why is a very wide covering index often a poor first answer?
24. What makes a query parameter-sensitive?
25. Why are execution plans and logical reads more reliable than runtime alone when evaluating a tuning change?

## Section 5: Internals, Operations, And Application Integration

26. Why can plan reuse be both beneficial and risky?
27. What does a memory grant try to provide for a query execution?
28. Why are waits useful clues but incomplete explanations?
29. Why must release validation include health, metrics, and smoke checks instead of only “deployment succeeded”?
30. Why is a running container not the same as a ready service?
31. Why is EF Core versus Dapper usually the wrong first framing for a data-access decision?
32. What is the practical shape of an N+1 problem in an application?
33. Why should telemetry design happen before the production incident rather than during it?

## Section 6: Synthesis And Senior-Level Reasoning

34. Why do strong capstone answers start with clarification rather than implementation?
35. What makes a tradeoff discussion stronger than a list of best practices?
36. Why should validation and observability be part of the proposed solution instead of a final afterthought?
37. What does a credible failure-mode discussion demonstrate about an engineer?
38. Why is the release story part of the design, not a separate operational footnote?

## Section 7: Applied Scenarios

39. A report query returns the right rows but the counts are too high after comments are joined in. What is the most likely class of mistake, and what rowset would you inspect first?
40. An API page sorted by `CreatedUtc` shows duplicated or missing rows between repeated requests. What is the likely root cause, and what is the cheapest safe repair?
41. A migration proposal adds a required column to a populated table in one statement. Why is that risky, and what safer rollout shape would you propose?
42. Two sessions update the same pair of rows in opposite orders and occasionally one request is aborted. What is happening, and what fix belongs in design versus retry logic?
43. A query is fast for a selective parameter value and slow for a broad parameter value. What is the likely issue, and what evidence would you collect first?
44. A release technically succeeds, but request latency spikes and readiness becomes unstable in the first five minutes. What signals should you inspect immediately?
45. A team argues that switching from EF Core to Dapper will automatically solve a slow endpoint. What higher-quality question should you ask before changing libraries?

## Answer Key

1. Without `ORDER BY`, SQL does not guarantee row order, so “latest” is not actually expressed.
2. A primary key gives stable database identity, while a unique business column protects a domain rule such as “no duplicate email addresses.”
3. It lets you confirm exactly which rows would change before you perform a potentially destructive write.
4. An inner join returns only matched rows, while a left join preserves parent rows even when the child side is missing.
5. The join may multiply rows before aggregation, so the aggregate can count the wrong thing even when the join path itself is valid.
6. `COUNT(*)` counts joined rows, while `COUNT(child.Id)` ignores `NULL` child rows preserved by the left join.
7. A grouped aggregate is the wrong tool when you need analytics such as rankings or running totals while still returning one row per detail record.
8. It defines which rows belong to the same analytical group.
9. Tied rows need a deterministic secondary order or they can drift between pages across executions.
10. Use `ROW_NUMBER()` when every row needs a unique sequence position, especially for pagination or latest-row-per-group logic.
11. A CTE separates reasoning stages, which makes it easier to verify the intended logic before worrying about optimization.
12. Rows from different logical owners get mixed into the same running total or sequence.
13. Because stable database identity does not enforce domain rules about duplicate user-facing values.
14. `NULL` means “missing is valid,” so using nullability only to ease rollout can weaken data rules incorrectly.
15. It reduces blocking and rollout risk by separating introduction, data movement, enforcement, and cleanup into safer phases.
16. It can hold locks too long, grow the log, create contention, and degrade foreground traffic.
17. It is the period where old and new contracts must both work so deployment ordering does not break running consumers.
18. Blocking is waiting behind another session; deadlocking is a cycle where sessions wait on each other until the engine aborts one.
19. Longer transactions hold locks longer, increasing contention and deadlock probability.
20. It prevents circular waits by making all code paths acquire the same resources in the same order.
21. It detects that a row changed after it was read so the application can avoid silently overwriting newer data.
22. Because indexes should serve real access patterns, not abstract rules disconnected from workload behavior.
23. It may help one read path while increasing storage, maintenance, and write cost across the system.
24. Different parameter values make different plan shapes optimal, so one cached plan fits some inputs badly.
25. They show how much work the engine actually did and why, while runtime alone can fluctuate because of noise.
26. Reuse saves compilation cost, but a reused plan can be poorly suited to a very different parameter pattern.
27. It reserves workspace memory for operators such as sorts and hash joins.
28. They indicate what the engine was waiting on, but they need context from plans, workload, and row estimates to explain root cause.
29. A deployment can succeed mechanically while the service still fails functionally, performs poorly, or cannot serve real traffic safely.
30. Process liveness does not prove dependency readiness, initialization success, or useful service behavior.
31. The first question should be about query and update shape, because either tool can be a good or bad fit depending on the workload.
32. The app loads a list and then issues extra queries per item for related data, multiplying round trips.
33. If telemetry is missing when the incident starts, the system is already under-observed and harder to debug.
34. Clarification aligns the solution to actual constraints and avoids building the wrong answer well.
35. Tradeoff discussion shows contextual judgment, alternatives, and consequences rather than memorized slogans.
36. A solution is incomplete if you cannot prove it works correctly, performs acceptably, and can be observed after release.
37. It demonstrates anticipation, prioritization, and the ability to plan detection and mitigation before failure happens.
38. Rollout, compatibility, monitoring, and rollback constraints change what is actually safe to build and ship.
39. The likely issue is row multiplication before aggregation; inspect the pre-aggregate joined rowset first.
40. The likely root cause is unstable ordering on tied values; add a deterministic tiebreaker such as `Id` to the sort.
41. It is risky because existing rows do not satisfy the new requirement and the change can block heavily; use add-nullable, backfill, validate, then enforce.
42. This is a deadlock pattern caused by inconsistent resource ordering; redesign should enforce consistent order, while retry logic may still be used for the transient victim outcome.
43. The likely issue is parameter sensitivity; compare plan shape, estimates, and reads for the selective versus broad parameter cases.
44. Inspect readiness endpoints, request error rate, latency, restart behavior, and any metrics or traces showing startup or dependency contention.
45. Ask what SQL shape, query plan, row count, and access pattern are making the endpoint slow now, because library choice alone does not explain the bottleneck.