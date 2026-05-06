# Phase 2: Intermediate Querying

## Objectives

- solve joins, aggregations, common table expressions, and ranking scenarios
- learn how query shape changes result correctness and readability

## Prerequisites

- strong Phase 1 query fluency

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