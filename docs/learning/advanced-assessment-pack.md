# Advanced Assessment Pack

Use this pack after Lessons 05 through 07. It checks whether you can connect concurrency, evidence-based tuning, and engine internals into one debugging model.

## Covers

- [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md)
- [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](06-indexing-execution-plans-and-parameter-sensitivity.md)
- [Lesson 07: SQL Server Internals](07-sql-server-internals.md)
- [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
- [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md)

## How To Use This Pack

1. Answer the short questions with evidence-oriented language, not only definitions.
2. For scenarios, separate the likely cause from the evidence you would gather.
3. Do not treat retry, indexing, or internals vocabulary as answers on their own.

## Short Questions

1. What is the difference between blocking and deadlocking?
2. Why does transaction duration matter even when each statement is logically correct?
3. Why does consistent access order reduce deadlock probability?
4. What problem does `rowversion` solve in an optimistic concurrency flow?
5. Why should index design begin with filter, join, and sort shape?
6. What makes a query parameter-sensitive?
7. Why can plan reuse be helpful and risky at the same time?
8. What does a memory grant try to provide for a query?
9. Why are waits useful clues but incomplete explanations?

## Scenarios

1. A request sometimes blocks for several seconds and sometimes fails as a deadlock victim. What two different classes of behavior must you separate before fixing it?
2. A query is fast for rare parameter values and slow for broad parameter values. What is the likely issue, and what evidence should you compare first?
3. A plan shows a large sort, high memory use, and occasional spills. What kinds of root causes would you investigate first?
4. A team proposes a very wide covering index for one slow query. What tradeoff review should happen before approval?

## Answer Key

1. Blocking is waiting behind another session, while deadlocking is a cycle that forces SQL Server to abort one participant.
2. Longer transactions hold locks longer, increasing contention and deadlock probability.
3. It reduces circular waits by making code paths acquire shared resources in the same sequence.
4. It detects that a row changed after read and before write so the application can avoid silent overwrite.
5. Because indexes should serve actual workload access patterns rather than generic rules.
6. Different parameter values make different plans optimal, so one cached plan fits some inputs badly.
7. Reuse saves compilation work, but reused plans can be poorly suited to different parameter patterns.
8. It reserves workspace memory for operators such as sorts and hash joins.
9. They show what the engine was waiting on, but not the full root cause without plan and workload context.
10. Separate ordinary blocking behavior from deadlock behavior before choosing fixes.
11. The likely issue is parameter sensitivity; compare plan shape, estimates, and reads for selective versus broad values.
12. Investigate row estimates, sort necessity, available indexes, and whether the sort can be reduced or avoided.
13. Review read benefit against write cost, storage, maintenance burden, and overlap with existing indexes.

## Ready To Advance When

- you can distinguish retry-worthy failures from design defects
- you can explain a tuning change with plan and read evidence instead of folklore
- you can use internals vocabulary to sharpen a hypothesis rather than to impress a reviewer
- you can connect transaction design, index shape, and execution behavior into one debugging story