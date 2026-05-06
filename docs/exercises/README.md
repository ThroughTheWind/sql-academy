# Exercise System

Exercises are organized by difficulty under `src/exercises`.

If you are following the curriculum in order, start with [Learning Docs](../learning/README.md) and return here when you need the exercise map or validation workflow.

Every exercise folder includes:
- a narrative README
- starter material
- an intentionally broken query, script, or investigation surface
- expected outcomes
- hints
- an optional solution file

Exercises currently ship in three supported modes:

- `validation pack`: includes `starter.sql`, `answer.sql`, and `validation.sql` so the learner can self-check with the harness.
- `guided lab`: includes starter assets and explicit completion criteria, but the learner validates the work manually against the README and expected outcomes.
- `investigation pack`: includes evidence snapshots or workbooks and expects a written analysis instead of a single SQL answer file.

Design rules:
- exercises should validate a specific learning objective
- broken states should resemble real engineering mistakes
- learners should be able to verify results locally against `LearningDb`
- hints should help without fully solving the exercise too early

## Exercise Ladder

| Pack | Mode | Primary Focus |
| --- | --- | --- |
| [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) | validation pack | SQL fundamentals and safe changes |
| [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md) | validation pack | joins and aggregations |
| [Beginner 002](../../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md) | validation pack | filtering, constraints, and data quality |
| [Intermediate 001](../../src/exercises/Intermediate/001-window-functions-and-pagination/README.md) | validation pack | window functions and stable pagination |
| [Intermediate 002](../../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md) | validation pack | cohort analysis and pagination drift |
| [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) | validation pack | indexing, parameter sensitivity, and rollout safety |
| [Advanced 002](../../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md) | validation pack | staged backfill and contract enforcement |
| [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) | guided lab | plan cache, memory grants, and wait signals |
| [Senior 001](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) | validation pack | concurrency, blocking, and deadlocks |
| [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) | validation pack | EF Core N+1, optimistic concurrency, and ingestion design |
| [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) | validation pack | transactional outbox and delivery consistency |
| [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | investigation pack | posts API latency and observability triage |

## Validation Workflow For Validation Packs

1. Edit the exercise `answer.sql` file if the pack includes one.
2. Start the Docker stack so SQL Server is available.
3. Run `infra/scripts/run-exercise-validation.ps1 -Exercise <relative-exercise-path>`.
4. Fix the answer until the validation harness returns success.

## Guided Lab Workflow

1. Read the exercise README and identify the repository anchors you need to inspect.
2. Work through the starter material and broken example before looking at the optional solution.
3. Use `expected-outcomes.md` and the README validation criteria as the completion contract.

Guided labs such as [Advanced 003](../../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) do not ship with `answer.sql` plus `validation.sql`.

## Investigation Pack Workflow

1. Read the incident or scenario brief.
2. Fill in the workbook or written response template.
3. Compare the write-up with `expected-outcomes.md` and the optional solution.

Investigation packs such as [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) use Markdown evidence snapshots and narrative output instead of a single SQL answer file.