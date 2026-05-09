# Early-Route Printable Checklists

Use these compact checklists when you want a one-page completion sheet for the early route instead of rereading the full lesson or phase narrative.

These checklists are companion surfaces, not a second curriculum map. Keep [Curriculum Map](curriculum-map.md) as the ordered route and use these pages only to confirm that the current step is actually complete.

## Phase 0 Local Setup Checklist

Use this after [Phase 0: Local Setup](../phases/phase-0-local-setup.md).

### Must Prove

- [ ] `docker compose config` succeeds before full startup.
- [ ] a SQL client can connect to `LearningDb`.
- [ ] `SELECT COUNT(*) FROM academy.Users;` returns seeded data.
- [ ] `http://localhost:8080/health/ready` succeeds when the full stack is running.

### Can Explain

- [ ] where the schema and seed scripts live.
- [ ] the difference between local startup, readiness, and optional observability checks.
- [ ] which docs to use next if the failure is Docker, SQL connection, or early route confusion.

### Ready To Continue When

- [ ] the first successful query feels repeatable instead of lucky.
- [ ] you can point to the next owner surface without browsing the repo tree blindly.

## Lesson 01 SQL Fundamentals Checklist

Use this after [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md).

### Must Prove

- [ ] you can explain why deterministic ordering needs a tiebreaker.
- [ ] you can point to one uniqueness rule and one foreign-key rule in the seeded model.
- [ ] you can write a preview `SELECT` before any `UPDATE` or `DELETE`.

### Can Explain

- [ ] why `SELECT *` is a weak learning default.
- [ ] how a logically correct table can still be queried unsafely.
- [ ] what kind of failure to expect from a broken required value, uniqueness rule, or foreign key.

### Ready To Continue When

- [ ] single-table reasoning feels routine.
- [ ] you can predict row shape before running the query.

## Lesson 02 Joins And Aggregations Checklist

Use this after [Lesson 02: Joins And Aggregations](02-joins-and-aggregations.md).

### Must Prove

- [ ] you can choose the correct join type for one report and explain what row shape it guarantees.
- [ ] you can show one aggregate that stays correct when comments are joined to posts.
- [ ] you can inspect the pre-aggregate rowset before rewriting a broken grouped query.

### Can Explain

- [ ] when a `LEFT JOIN` stops behaving like a true outer join.
- [ ] when to use `COUNT(*)`, `COUNT(column)`, or `COUNT(DISTINCT ...)`.
- [ ] why a correct join path does not guarantee a correct aggregate.

### Ready To Continue When

- [ ] join correctness is no longer guesswork.
- [ ] you can predict row multiplication before adding `GROUP BY`.

## Lesson 03 Window Functions And Intermediate Querying Checklist

Use this after [Lesson 03: Window Functions And Intermediate Querying](03-window-functions-and-intermediate-querying.md).

### Must Prove

- [ ] you can choose between `ROW_NUMBER()`, `RANK()`, and `DENSE_RANK()` for one concrete output.
- [ ] you can explain why running totals need both a partition and a deterministic order.
- [ ] you can defend one pagination `ORDER BY` that will stay stable across repeated calls.

### Can Explain

- [ ] when a window function is a better fit than a grouped aggregate.
- [ ] why unstable ordering is a correctness bug, not only a UX issue.
- [ ] how a CTE can improve reasoning even when it does not change the physical plan.

### Ready To Continue When

- [ ] analytical querying feels dependable.
- [ ] you can preserve row detail without losing correctness.

## Beginner Checkpoint Checklist

Use this after [Beginner Assessment Pack](beginner-assessment-pack.md) and the linked beginner validation packs.

### Must Prove

- [ ] you can explain why deterministic ordering needs a stable tiebreaker.
- [ ] you can predict how a join will change row counts before adding `GROUP BY`.
- [ ] you can distinguish safe counting patterns on left joins without guessing.

### Can Explain

- [ ] one uniqueness rule and one foreign-key rule in the schema and how each would fail.
- [ ] why previewing rows before a write is structural safety, not just caution.
- [ ] why a grouped result can still be wrong even when the relationship path is valid.

### Ready To Continue When

- [ ] you can answer mixed Lesson 01 and Lesson 02 questions without reopening the lesson docs immediately.
- [ ] Beginner 000 and Beginner 001 feel like validation, not first exposure.

## Intermediate Checkpoint Checklist

Use this after [Intermediate Assessment Pack](intermediate-assessment-pack.md) and the linked intermediate route packs.

### Must Prove

- [ ] you can explain when a window function is the right shape instead of a grouped aggregate.
- [ ] you can defend a pagination order as deterministic instead of merely convenient.
- [ ] you can review a migration as a release sequence rather than only a DDL statement.

### Can Explain

- [ ] why correct SQL can still be operationally unsafe under live traffic.
- [ ] why compatibility windows matter during schema evolution.
- [ ] how partitioning, ranking choice, and stable ordering affect API-facing query contracts.

### Ready To Continue When

- [ ] you can answer Lesson 03 and Lesson 04 review questions from memory.
- [ ] Intermediate 001, Intermediate 002, and Advanced 002 feel connected instead of like unrelated exercises.

## Best Companions

1. [Curriculum Map](curriculum-map.md)
2. [How To Start](how-to-start.md)
3. [Command Cheat Sheet](command-cheat-sheet.md)