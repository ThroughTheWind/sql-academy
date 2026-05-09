# Advanced 001: Indexing, Parameter Sniffing, And Migration Safety

## Objective

Tune a read path, recognize a parameter-sensitive query, and rewrite a risky migration into a safer rollout.

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