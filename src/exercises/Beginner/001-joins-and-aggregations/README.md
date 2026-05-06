# Beginner 001: Joins And Aggregations

## Objective

Use `academy.Users`, `academy.Posts`, and `academy.Comments` to produce correct relational and aggregate answers without accidental row multiplication.

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