# Advanced 001: Indexing, Parameter Sniffing, And Migration Safety

## Objective

Tune a read path, recognize a parameter-sensitive query, and rewrite a risky migration into a safer rollout.

## Before You Start

- read [Lesson 06: Indexing, Execution Plans, And Parameter Sensitivity](../../../../docs/learning/06-indexing-execution-plans-and-parameter-sensitivity.md)
- keep [Trade Dapper read path](../../../../src/libs/SqlAcademy.Persistence/Queries/Trades/TradeReadService.cs) nearby if the filter, sort, and paging shape still feels fuzzier than the index discussion itself
- keep [Query performance comparison tests](../../../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) nearby if you want one concrete reminder that query shape and result-contract comparison come before tool folklore

## Suggested Workflow

1. Run `starter.sql` unchanged and restate the filter, sort, and projection shape before you edit `answer.sql`.
2. Read `expected-outcomes.md` so you know the exact index and rollout contract the validation harness will verify.
3. Propose the narrow trade index first and defend its read or write tradeoff before you touch the migration steps.
4. Write the parameter-sensitivity summary only after you can explain why one cached plan can misfit another parameter pattern.
5. Rewrite the migration into staged forward-only steps last so the rollout story stays separate from the indexing story.

## If You Get Stuck

- go back to the query-shape checklist in [Lesson 06](../../../../docs/learning/06-indexing-execution-plans-and-parameter-sensitivity.md) before you add another column to the index
- separate the workload-support question from the plan-stability question and solve them one at a time
- use `broken.sql` to name whether the current mistake is an over-wide index, a vague parameter-sensitivity claim, or an unsafe migration rewrite before you open `optional-solution.sql`

## Scenario

An API endpoint is slow under some parameter values, and the proposed fix includes both an index change and a schema migration that may block production traffic.

## Assets

- `starter.sql` creates the baseline procedure used in the review.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the index shape and structured rollout answers.
- `broken.sql` contains the unsafe implementation.
- `expected-outcomes.md` lists the checks the result must satisfy.
- `hints.md` gives progressive guidance.
- `optional-solution.sql` shows one valid implementation.

## Tasks

1. review the query and decide which index, if any, is justified
2. identify why one parameter value compiles a plan that harms another
3. rewrite the migration into safer forward-only steps

## Validation

- index changes are justified with read/write tradeoffs
- parameter sensitivity is demonstrated, not assumed
- the migration no longer requires a dangerous all-at-once table rewrite

## Sample Output Cues

You do not need byte-for-byte formatting, but your answer should leave the validation surfaces showing these shapes:

### `#parameter_sensitivity_summary`

```text
ParameterPattern             RiskSummary                  MitigationCode
---------------------------  ---------------------------  ------------------------
DifferentUserIdSelectivity  OneCachedPlanCanMisfitAnother ReviewRepresentativePlans
```

### `#safe_post_summary_rollout`

```text
StepNumber  StepCode             StepCategory
----------  -------------------  -----------
1           AddSummaryNullable   Additive
2           BackfillExistingRows Backfill
3           EnforceNotNull       Enforcement
```

Your created index should also validate as a narrow workload-shaped index on `academy.Trades` keyed by `UserId` then `TradedUtc DESC` with only the expected covering columns.