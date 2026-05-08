# Phase 2: Intermediate Querying

## Why This Phase Starts Here

This phase is called intermediate because it expands beyond single-table queries into row multiplication, grouping, and analytical SQL.

It still starts with beginner-safe join and aggregation work in [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md) before it moves into window functions and pagination.

## Suggested Time Budget

- 90 to 150 minutes for Lesson 02, Beginner 001, and the first ranking or pagination examples after joins feel predictable
- longer if joined row counts and grouped totals still feel harder to explain than single-table queries

## Objectives

- solve joins, aggregations, common table expressions, and ranking scenarios
- learn how query shape changes result correctness and readability

## Prerequisites

- strong Phase 1 query fluency

## First Success Markers

- you can explain why one post appears multiple times after joining comments before you add `GROUP BY`
- you can inspect a pre-aggregate rowset and predict which parent rows survive a `LEFT JOIN`
- you can finish [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md) before you push deeper into ranking or pagination

## If You Get Stuck

- return to [Lesson 02: Joins And Aggregations](../learning/02-joins-and-aggregations.md) and inspect the raw joined rowset before changing the aggregate
- do not jump to `ROW_NUMBER()`, CTEs, or pagination until join cardinality already makes sense
- keep [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md) as the default first pack for this phase before opening [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)

## Concepts

- inner and outer joins
- grouping and `HAVING`
- CTEs and derived tables
- window functions for ranking, paging, and running totals

## Exercises

- join users, posts, and comments into reporting views
- compute post and trade summaries by user
- rank instruments by trade activity with window functions

## Expected Outcomes

- the learner can move between row-level and aggregate views confidently
- the learner can recognize when a window function beats a self-join or nested aggregate

## Validation Checklist

- join results preserve intended cardinality
- aggregate totals match seeded facts
- window-function queries produce correct ranking and pagination results