# Intermediate Assessment Pack

Use this pack after Lessons 03 and 04. It checks whether you can move between analytical query shape and safe schema change reasoning without treating them as unrelated topics.

## Covers

- [Lesson 03: Window Functions And Intermediate Querying](03-window-functions-and-intermediate-querying.md)
- [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
- [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)
- [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
- [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)

## How To Use This Pack

1. Answer the short questions from memory.
2. For scenarios, describe both the likely issue and the first validating check you would run.
3. Compare your answer with the key only after you have written a position you can defend.

## Short Questions

1. What does `PARTITION BY` control in a window function?
2. Why is a stable tiebreaker required for pagination?
3. When is `ROW_NUMBER()` a better fit than `RANK()`?
4. Why can a CTE improve reasoning even when it does not improve performance?
5. Why is a nullable-add, backfill, enforce sequence usually safer than a one-step required-column change?
6. What makes a compatibility window necessary during schema evolution?
7. Why can a large backfill create production risk even when the SQL is logically correct?

## Scenarios

1. A paged trade API sorted by timestamp shows row drift between repeated calls. What is the likely root cause, and what is the cheapest safe repair?
2. A running-total query suddenly mixes multiple users into one sequence. What likely clause is missing or wrong?
3. A proposed migration adds `NOT NULL` to a new column on a large populated table. What safer rollout would you propose?
4. A team wants to drop the old column in the same release that introduces a new column contract. What deployment risk are they ignoring?

## Answer Key

1. It defines which rows belong to the same analytical group.
2. Tied rows need deterministic secondary ordering or they can drift between pages.
3. Use `ROW_NUMBER()` when every row needs a unique sequence position, especially for pagination or latest-row-per-group logic.
4. A CTE separates reasoning stages, which makes verification easier before optimization.
5. It breaks the change into safer phases that reduce blocking, compatibility, and rollback risk.
6. Old and new code paths may both need to operate safely during rollout, so the contracts must coexist temporarily.
7. It can hold locks too long, grow the log, and contend with live traffic.
8. The likely issue is unstable ordering on tied values; add a deterministic tiebreaker such as `Id`.
9. The query is likely missing the correct `PARTITION BY` for the logical owner of the running total.
10. Add the column as nullable, deploy compatible writes, backfill in controlled batches, validate, then enforce `NOT NULL`.
11. They are ignoring the compatibility window between old and new contracts.

## Ready To Advance When

- you can explain when a window function is the right shape instead of a grouped aggregate
- you can defend a pagination order as deterministic instead of merely convenient
- you can review a migration as a release sequence rather than as only a DDL statement
- you can explain why correct SQL can still be operationally unsafe under live traffic