# Academy Roadmap

| Phase | Theme | Main Outcome |
| --- | --- | --- |
| 0 | Local setup and tooling | A learner can run the full platform locally and inspect the seeded data. |
| 1 | SQL fundamentals | A learner can write correct CRUD and filtering queries against the baseline schema. |
| 2 | Intermediate querying | A learner can solve joins, aggregations, CTEs, and window-function exercises. |
| 3 | Schema design | A learner can model entities, constraints, keys, and safe relational changes. |
| 4 | Transactions and concurrency | A learner can explain blocking, isolation, optimistic concurrency, and deadlocks. |
| 5 | Indexing and performance | A learner can read execution plans and make index decisions with evidence. |
| 6 | SQL Server internals | A learner can reason about storage, plan cache, memory, and wait behavior. |
| 7 | Operational engineering | A learner can run migrations safely, observe systems, and debug production-like incidents. |
| 8 | .NET and EF Core integration | A learner can build hybrid EF + Dapper services against the same SQL Server model. |
| 9 | Capstones | A learner can combine data modeling, performance, and operations into end-to-end solutions. |

## Scope Target

The current academy route targets working backend and application engineers who use SQL Server together with EF Core and Dapper in real application and production work.

It starts at SQL fundamentals, but it is not yet intended as a first-ever programming or database course.

That means the core route is expected to cover query shape, schema design, concurrency, tuning, internals, observability, release safety, and application-data-access tradeoffs to a strong professional level.

It does not yet claim full DBA or DBRE mastery. Backup and restore, SQL Server Agent, HA/DR, replication, security administration, and similar specialties belong in an optional extension track after the core route is stronger.

The implementation backlog for that scope expansion lives in [.ai/sql-efcore-mastery-backlog.md](.ai/sql-efcore-mastery-backlog.md).

## Phase Gates

- Phase 0 exit: the stack is running and the learner can query `academy.Users`.
- Phase 2 exit: the learner can solve ranking, pagination, and aggregate exercises without hints.
- Phase 4 exit: the learner can reproduce blocking and reason about transaction scope decisions.
- Phase 5 exit: the learner can justify indexes with execution-plan evidence rather than folklore.
- Phase 8 exit: the learner can explain when to use EF Core, Dapper, or both.
- Phase 9 exit: the learner can defend a design under operational and interview-style pressure.

## Delivery Principles

- Every phase must be runnable locally.
- Every phase must contain docs, exercises, and explicit validation goals.
- New material should extend the platform instead of replacing previous phases.
- Exercises should trend from narrow query correctness to system behavior and tradeoff analysis.