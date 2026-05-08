# Beginner 001: Joins And Aggregations

## Objective

Use `academy.Users`, `academy.Posts`, and `academy.Comments` to produce correct relational and aggregate answers without accidental row multiplication.

## Before You Start

- complete [Phase 1: SQL Fundamentals](../../../../docs/phases/phase-1-sql-fundamentals.md)
- read [Lesson 02: Joins And Aggregations](../../../../docs/learning/02-joins-and-aggregations.md)
- keep [Learning Glossary](../../../../docs/learning/glossary.md) nearby if terms such as join multiplicity, cardinality, or left join are still slow

## Suggested Workflow

1. Run `starter.sql` unchanged and inspect the pre-aggregate rowset before you edit `answer.sql`.
2. Identify where parent rows should survive even when comments are missing.
3. Solve one output at a time instead of rewriting every join and aggregate in one pass.
4. Open `hints.md` only after you can explain why the current counts are wrong.

## If You Get Stuck

- go back to the raw join examples in [Lesson 02](../../../../docs/learning/02-joins-and-aggregations.md) and count rows before you aggregate
- use `broken.sql` to name whether the failure is row multiplication, lost parent rows, or a counting mistake
- do not open `optional-solution.sql` until you already have a concrete opinion about the repair

## Scenario

You have inherited a reporting query that mixes joins and aggregations but produces incorrect counts once comments are introduced.

## Assets

- `starter.sql` contains the baseline query shapes.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the expected result tables in one SQL session.
- `broken.sql` contains the incorrect implementation.
- `expected-outcomes.md` lists the checks the result must satisfy.
- `hints.md` gives progressive guidance.
- `optional-solution.sql` shows one clean solution.

## Tasks

1. Fix the join path so posts without comments are still represented correctly.
2. Produce per-user post counts without inflating results because of comment joins.
3. Return comment counts per post with a stable sort order.

## Validation

- every seeded post appears exactly once in the detail output
- post counts match the seed data
- the result explains why `COUNT(*)` and `COUNT(CommentId)` differ after a left join