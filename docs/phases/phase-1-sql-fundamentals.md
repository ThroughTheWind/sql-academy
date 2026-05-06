# Phase 1: SQL Fundamentals

## Objectives

- build confidence with projection, filtering, sorting, and inserts
- reinforce set-based thinking over row-by-row habits
- understand keys and relationships in the `academy` schema

## Prerequisites

- Phase 0 complete

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