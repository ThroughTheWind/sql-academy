# Learning Docs

This folder is the learner-facing entry point for the academy.

If you want one authoritative journey through the repository, start with [Curriculum Map](curriculum-map.md).

If you are brand new to the repository, read [How To Start](how-to-start.md) first and then return to [Curriculum Map](curriculum-map.md).

Use [Learning Glossary](glossary.md) when a term blocks progress, and clear the checkpoint section at the end of each lesson before moving on.

## Ordered Lessons

1. [How To Start](how-to-start.md)
2. [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md)
3. [Lesson 02: Joins And Aggregations](02-joins-and-aggregations.md)
4. [Lesson 03: Window Functions And Intermediate Querying](03-window-functions-and-intermediate-querying.md)
5. [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
6. [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md)
7. [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](06-indexing-execution-plans-and-parameter-sensitivity.md)
8. [Lesson 07: SQL Server Internals](07-sql-server-internals.md)
9. [Lesson 08: Operational Engineering And Release Safety](08-operational-engineering-and-release-safety.md)
10. [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md)
11. [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md)
12. [Lesson 11: Capstones And Interview Readiness](11-capstones-and-interview-readiness.md)
13. [Cumulative Review](cumulative-review.md)

## How To Use This Folder

- [Curriculum Map](curriculum-map.md) is the source of truth for sequence, companion practice, and validation mode.
- [How To Start](how-to-start.md) is the first-week walkthrough for a clean clone.
- [Cumulative Review](cumulative-review.md) is the mixed-topic checkpoint after several lessons.
- the assessment packs split review by level so you can test one slice at a time: [Beginner](beginner-assessment-pack.md), [Intermediate](intermediate-assessment-pack.md), [Advanced](advanced-assessment-pack.md), and [Senior](senior-assessment-pack.md)
- [Follow-Up Exercises By Level](follow-up-exercises-by-level.md) is for extension ideas after the main route, not the main route itself.
- [DBA And DBRE Extension Track](dba-dbre-extension-track.md) is the optional specialization route after the main academy path feels complete.
- [Row-Level Security And Tenant Isolation](row-level-security-and-tenant-isolation.md) is an optional DBA or DBRE security follow-on after Lesson 04 and Senior 011.
- [Repository Improvement Suggestions](repository-improvement-suggestions.md) is a maintainer backlog, not a learner progression guide.

## Exercise Modes

- `validation pack`: includes `starter.sql`, `answer.sql`, and `validation.sql`.
- `guided lab`: uses lesson walkthroughs, repository anchors, or code changes without a formal answer template.
- `investigation pack`: uses evidence files and written analysis instead of a single SQL answer file.

## Supporting Guides

- [Curriculum Map](curriculum-map.md)
- [How To Start](how-to-start.md)
- [From SQL To EF Core And Dapper](from-sql-to-efcore-and-dapper.md)
- [Learning Glossary](glossary.md)
- [Cumulative Review](cumulative-review.md)
- [Beginner Assessment Pack](beginner-assessment-pack.md)
- [Intermediate Assessment Pack](intermediate-assessment-pack.md)
- [Advanced Assessment Pack](advanced-assessment-pack.md)
- [Senior Assessment Pack](senior-assessment-pack.md)
- [Follow-Up Exercises By Level](follow-up-exercises-by-level.md)
- [DBA And DBRE Extension Track](dba-dbre-extension-track.md)
- [Row-Level Security And Tenant Isolation](row-level-security-and-tenant-isolation.md)
- [Repository Improvement Suggestions](repository-improvement-suggestions.md)

## Validation Workflow

For validation-ready packs, run:

`powershell -ExecutionPolicy Bypass -File ../../infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift`