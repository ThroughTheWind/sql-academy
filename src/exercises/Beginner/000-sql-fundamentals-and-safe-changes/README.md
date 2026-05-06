# Beginner 000: SQL Fundamentals And Safe Changes

## Objective

Build confidence with projection, filtering, deterministic ordering, and constraint-aware reasoning before later lessons introduce joins, windows, or performance tuning.

## Scenario

You are reviewing the seeded academy data and a small candidate-user import before anything is allowed to touch production tables.

## Assets

- `starter.sql` sets up the candidate-user staging fixture.
- `answer.sql` is the learner-editable solution template.
- `validation.sql` verifies the expected result tables in one SQL session.
- `broken.sql` shows the kinds of mistakes this exercise is designed to catch.
- `expected-outcomes.md` describes the required end state.
- `hints.md` offers progressively more direct guidance.
- `optional-solution.sql` shows one valid answer.

## Tasks

1. Create `#lesson01_user_directory` with `DisplayOrder`, `UserId`, `UserName`, `Email`, and `CreatedUtc` for every user ordered by `CreatedUtc`, `Id`.
2. Create `#lesson01_recent_actionable_orders` with `ReviewRank`, `OrderNumber`, `Status`, `TotalAmount`, and `UpdatedUtc` for the two most recently updated orders that are not already filled.
3. Create `#lesson01_candidate_user_decisions` with `CandidateRowId` and `DecisionCode` so each staged row is marked as `ReadyToInsert`, `MissingUserName`, `MissingEmail`, `DuplicateUserName`, or `DuplicateEmail`.

## Validation

- the user directory uses explicit projection and deterministic ordering
- the order review slice filters before taking the top two rows
- staged user decisions respect missing values and existing uniqueness rules