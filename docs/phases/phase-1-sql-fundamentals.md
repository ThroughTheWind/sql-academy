# Phase 1: SQL Fundamentals

## Suggested Time Budget

- 60 to 120 minutes if Phase 0 already feels predictable
- longer if `SELECT`, `WHERE`, `ORDER BY`, and constraint errors are still unfamiliar

## Objectives

- build confidence with projection, filtering, sorting, and inserts
- reinforce set-based thinking over row-by-row habits
- understand keys and relationships in the `academy` schema

## Prerequisites

- Phase 0 complete

## First Success Markers

- you can connect to `LearningDb` without re-solving setup problems every few minutes
- you can run a read-only query against `academy.Users`, `academy.Orders`, and `academy.Posts`
- you can explain why a query needs explicit ordering before you start the exercise pack

## If You Get Stuck

- return to [How To Start](../learning/how-to-start.md) and [Lesson 01: SQL Fundamentals](../learning/01-sql-fundamentals.md) before widening scope
- use [Learning Glossary](../learning/glossary.md) when terms such as predicate, projection, or deterministic order block progress
- keep your work read-only until you can predict the result set before running the query

## Concepts

- `SELECT`, `WHERE`, `ORDER BY`, `TOP`, and predicates
- primary keys, foreign keys, and referential integrity
- insert, update, and delete safety basics

## Exercises

- retrieve users, posts, orders, and trades with simple predicates
- insert a new user and verify uniqueness constraints
- update an order status and inspect the resulting row state
- complete [Beginner 000: SQL Fundamentals And Safe Changes](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)

## Expected Outcomes

- the learner can write correct single-table queries without guesswork
- the learner can explain why constraints fail when invalid data is attempted

## Validation Checklist

- all starter queries return the expected number of rows
- uniqueness and FK violations are recognized and explained
- learners can reason about why a query shape is correct before running it
- [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) validates successfully when Docker is available