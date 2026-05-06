# Exercise System

Exercises are organized by difficulty under `src/exercises`.

If you are following the curriculum in order, start with [Learning Docs](../learning/README.md) and return here when you need the exercise map or validation workflow.

Each exercise folder includes:
- a narrative README
- starter SQL
- intentionally broken queries or change scripts
- expected outcomes
- hints
- an optional solution file

Some exercise packs also include:
- `answer.sql` as a learner-editable answer template
- `validation.sql` as a sqlcmd-compatible verification harness

Design rules:
- exercises should validate a specific learning objective
- broken states should resemble real engineering mistakes
- learners should be able to verify results locally against `LearningDb`
- hints should help without fully solving the exercise too early

## Exercise Ladder

1. [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)
2. [Beginner 002](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md)
3. [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md)
4. [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md)
5. [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
6. [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md)
7. [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
8. [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
9. [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
10. [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)

## Validation Workflow

1. Edit the exercise `answer.sql` file if the pack includes one.
2. Start the Docker stack so SQL Server is available.
3. Run `infra/scripts/run-exercise-validation.ps1 -Exercise <relative-exercise-path>`.
4. Fix the answer until the validation harness returns success.

Operational investigation packs such as [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) use Markdown workbooks and evidence snapshots instead of `answer.sql` plus `validation.sql`.