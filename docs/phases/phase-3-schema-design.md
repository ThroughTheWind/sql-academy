# Phase 3: Schema Design

## Objectives

- design tables, keys, constraints, and naming conventions for maintainability
- evaluate schema changes for correctness and migration safety

## Prerequisites

- Phases 1 and 2 complete

## Concepts

- surrogate keys versus natural keys
- unique constraints, check constraints, and defaults
- normalization and denormalization tradeoffs
- forward-only, reversible migration design

## Exercises

- design a new table that extends the trading domain
- review a deliberately unsafe migration and rewrite it safely
- compare one wide-table design against a normalized alternative

## Expected Outcomes

- the learner can justify schema decisions with workload and operational reasoning
- the learner can identify dangerous migration patterns before execution

## Validation Checklist

- proposed schemas preserve integrity rules
- migrations avoid long blocking changes where possible
- naming and indexing decisions are documented alongside the change