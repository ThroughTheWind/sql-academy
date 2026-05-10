# Lesson And Exercise Quality Backlog

This is the maintainer execution backlog for auditing whether each learner-facing lesson and each exercise pack is accurate, deep enough for the academy's stated audience, and linked to the right supporting resources.

## Target

Keep the academy honest at the individual-surface level: every lesson should be repository-grounded and exhaustive enough for its stated scope, and every exercise should be executable, relevant, and clearly routed from the corresponding lesson or supporting reference.

## Scope Boundary

- in scope: every file under `docs/learning`, every exercise pack under `src/exercises`, and any routing consistency change needed to keep those surfaces aligned
- out of scope: broad curriculum expansion before the audit proves a gap, turning support guides into separate full courses, or inventing theory that is not grounded in repository assets

## Status Legend

- `not-started`: planned but not yet underway
- `in-progress`: active implementation slice
- `completed`: merged into the repository
- `blocked`: waiting on a prerequisite decision or missing surface

## Review Status Legend

- `not-reviewed`: the surface has not been audited yet
- `reviewing`: the surface is in the current audit slice
- `pass`: the surface is accurate and sufficient for its declared scope with no follow-up change recorded yet
- `findings-recorded`: the surface has one or more actionable follow-up items or corrective edits
- `blocked`: the surface cannot be reviewed honestly until a prerequisite or evidence gap is resolved

## Lesson And Instructional-Doc Rubric

Review each learner-facing doc against these checks:

1. Topic depth: the content is exhaustive enough for the audience and the promises made in the doc itself.
2. Factual correctness: substantive claims match the schema, seed data, code, tests, phase docs, or runbooks that actually exist.
3. Repository grounding: examples and advice stay tied to real repository anchors rather than generic SQL or EF Core slogans.
4. Prerequisite clarity: the doc names the earlier lessons, references, or setup steps a learner actually needs.
5. Practice routing: the doc links to the corresponding exercise, review pack, or supporting reference when the learner should apply the topic.
6. Self-check quality: review questions, sample outputs, challenge prompts, or exit criteria make completion state clear without dumping a full solution.
7. Scope honesty: support docs are judged as support docs, not as standalone mastery guides, and optional material is not presented as core-route required work.

## Exercise-Pack Rubric

Review each exercise pack against these checks:

1. Enough information to start: the README names prerequisites, scenario, assets, and the expected working loop.
2. Asset completeness: the pack ships the files its declared mode implies, or explicitly explains why a different completion contract is used.
3. Topic relevance: the task teaches the lesson or route concept it is mapped to instead of drifting into a different skill.
4. Real learner value: the failure mode or scenario resembles a real engineering mistake, tradeoff, or investigation path.
5. Lesson and resource links: the README points back to the owning lesson, phase, or reference surfaces a learner would reasonably need.
6. Validation clarity: the harness, completion checklist, workbook contract, or focused companion check is explicit and honest about what it does and does not prove.
7. Mode correctness: validation pack, guided lab, and investigation pack labels match the actual assets and expected learner output.

## Evidence Anchors

Use the narrowest real repository evidence that can falsify a claim:

- schema and seed truth: [LearningDb schema bootstrap](../db/schemas/001_create_learning_db.sql), [reference seed data](../db/seed/001_seed_reference_data.sql), [social and order seed data](../db/seed/002_seed_social_and_orders.sql), and [migration strategy](../db/migrations/README.md)
- application and persistence truth: [SqlAcademy.Api startup](../src/apps/SqlAcademy.Api/Program.cs), [application seed initializer](../src/libs/SqlAcademy.Persistence/Initialization/LearningDbSeed.cs), and the query, mapping, controller, or middleware anchor named by the lesson or pack
- validation truth: [Validation Matrix](validation-matrix.md), the named focused companion checks inside pack READMEs, and the exercise harness for validation packs
- routing truth: [Learning Docs](../docs/learning/README.md), [Curriculum Map](../docs/learning/curriculum-map.md), [Exercise System](../docs/exercises/README.md), [Exercise Index](../src/exercises/README.md), [Documentation Hub](../docs/README.md), and the root [README](../README.md)

## Review Order

1. Route-entry and early-core docs: `README.md`, `curriculum-map.md`, `how-to-start.md`, `sql-first-day-one.md`, `sql-to-dotnet-data-access-path.md`, and Lessons 01 through 04.
2. Early and intermediate packs: Beginner 000 through 002, Intermediate 001 through 002, and Advanced 002.
3. Advanced-core and operations docs: Lessons 05 through 08 and their immediate review or assessment support surfaces.
4. Advanced-core and operations packs: Senior 001, Advanced 001, Advanced 003, Advanced 004, Senior 004, and Senior 006.
5. Integration, capstone, and extension docs: Lessons 09 through 11, bridge or extension docs, assessment packs, cumulative review, and review-style practice.
6. Integration, capstone, and extension packs: Advanced 005, Senior 002, Senior 003, Senior 005, and Senior 007 through Senior 012.
7. Cross-surface routing reconciliation after the individual reviews are complete.

## Learner-Doc Inventory

| Surface ID | Category | Surface | Review Status | Notes |
| --- | --- | --- | --- | --- |
| L001 | route hub | [docs/learning/README.md](../docs/learning/README.md) | not-reviewed | learner entry point and inventory owner |
| L002 | route hub | [docs/learning/curriculum-map.md](../docs/learning/curriculum-map.md) | not-reviewed | authoritative lesson order and lesson-to-practice map |
| L003 | route entry | [docs/learning/how-to-start.md](../docs/learning/how-to-start.md) | findings-recorded | default guided-route start |
| L004 | route entry | [docs/learning/sql-first-day-one.md](../docs/learning/sql-first-day-one.md) | findings-recorded | reduced-startup first-query route |
| L005 | route entry | [docs/learning/sql-to-dotnet-data-access-path.md](../docs/learning/sql-to-dotnet-data-access-path.md) | not-reviewed | optional on-ramp into phase 8 and Lesson 09 |
| L006 | core lesson | [docs/learning/01-sql-fundamentals.md](../docs/learning/01-sql-fundamentals.md) | findings-recorded | maps to Beginner 000 |
| L007 | core lesson | [docs/learning/02-joins-and-aggregations.md](../docs/learning/02-joins-and-aggregations.md) | pass | maps to Beginner 001 |
| L008 | core lesson | [docs/learning/03-window-functions-and-intermediate-querying.md](../docs/learning/03-window-functions-and-intermediate-querying.md) | not-reviewed | maps to Intermediate 001 and 002 |
| L009 | core lesson | [docs/learning/04-schema-design-and-migration-safety.md](../docs/learning/04-schema-design-and-migration-safety.md) | not-reviewed | maps to Advanced 002 |
| L010 | core lesson | [docs/learning/05-transactions-blocking-and-deadlocks.md](../docs/learning/05-transactions-blocking-and-deadlocks.md) | not-reviewed | maps to Senior 001 |
| L011 | core lesson | [docs/learning/06-indexing-execution-plans-and-parameter-sensitivity.md](../docs/learning/06-indexing-execution-plans-and-parameter-sensitivity.md) | not-reviewed | maps to Advanced 001 and optional Advanced 004 |
| L012 | core lesson | [docs/learning/07-sql-server-internals.md](../docs/learning/07-sql-server-internals.md) | not-reviewed | maps to Advanced 003 |
| L013 | core lesson | [docs/learning/08-operational-engineering-and-release-safety.md](../docs/learning/08-operational-engineering-and-release-safety.md) | not-reviewed | maps to Senior 006 |
| L014 | core lesson | [docs/learning/09-ef-core-dapper-and-query-shape.md](../docs/learning/09-ef-core-dapper-and-query-shape.md) | not-reviewed | maps to Advanced 005, Senior 005, and Senior 002 |
| L015 | core lesson | [docs/learning/10-observability-testing-and-performance-engineering.md](../docs/learning/10-observability-testing-and-performance-engineering.md) | not-reviewed | maps to Senior 004 and runtime validation surfaces |
| L016 | core lesson | [docs/learning/11-capstones-and-interview-readiness.md](../docs/learning/11-capstones-and-interview-readiness.md) | not-reviewed | maps to Senior 003 and capstone routing |
| L017 | bridge guide | [docs/learning/from-sql-to-efcore-and-dapper.md](../docs/learning/from-sql-to-efcore-and-dapper.md) | not-reviewed | handoff from SQL mental model into phase 8 |
| L018 | extension track | [docs/learning/dba-dbre-extension-track.md](../docs/learning/dba-dbre-extension-track.md) | not-reviewed | optional specialization menu |
| L019 | extension lesson | [docs/learning/row-level-security-and-tenant-isolation.md](../docs/learning/row-level-security-and-tenant-isolation.md) | not-reviewed | optional extension-track lesson with concrete sample |
| L020 | review pack | [docs/learning/cumulative-review.md](../docs/learning/cumulative-review.md) | not-reviewed | mixed-topic checkpoint across the route |
| L021 | assessment pack | [docs/learning/beginner-assessment-pack.md](../docs/learning/beginner-assessment-pack.md) | not-reviewed | lessons 01 through 02 checkpoint |
| L022 | assessment pack | [docs/learning/intermediate-assessment-pack.md](../docs/learning/intermediate-assessment-pack.md) | not-reviewed | lessons 03 through 04 checkpoint |
| L023 | assessment pack | [docs/learning/advanced-assessment-pack.md](../docs/learning/advanced-assessment-pack.md) | not-reviewed | lessons 05 through 07 checkpoint |
| L024 | assessment pack | [docs/learning/senior-assessment-pack.md](../docs/learning/senior-assessment-pack.md) | not-reviewed | lessons 08 through 11 checkpoint |
| L025 | review practice | [docs/learning/review-style-practice.md](../docs/learning/review-style-practice.md) | not-reviewed | review-judgment practice using existing pack surfaces |
| L026 | support reference | [docs/learning/glossary.md](../docs/learning/glossary.md) | not-reviewed | terminology support rather than standalone topic mastery |
| L027 | support reference | [docs/learning/schema-quick-reference.md](../docs/learning/schema-quick-reference.md) | not-reviewed | compact schema and relationship map |
| L028 | support reference | [docs/learning/field-type-selection-guide.md](../docs/learning/field-type-selection-guide.md) | not-reviewed | practical SQL Server field-type reference |
| L029 | support reference | [docs/learning/sql-client-connection-guide.md](../docs/learning/sql-client-connection-guide.md) | not-reviewed | client-specific connection help |
| L030 | support reference | [docs/learning/local-setup-troubleshooting.md](../docs/learning/local-setup-troubleshooting.md) | not-reviewed | setup recovery and blocked-path guidance |
| L031 | support reference | [docs/learning/command-cheat-sheet.md](../docs/learning/command-cheat-sheet.md) | not-reviewed | compact startup, validation, and focused-test lookup |
| L032 | support reinforcement | [docs/learning/early-route-printable-checklists.md](../docs/learning/early-route-printable-checklists.md) | not-reviewed | early-route completion companion |
| L033 | support planning | [docs/learning/effort-and-pacing-guide.md](../docs/learning/effort-and-pacing-guide.md) | not-reviewed | time-planning aid |
| L034 | optional follow-on | [docs/learning/follow-up-exercises-by-level.md](../docs/learning/follow-up-exercises-by-level.md) | not-reviewed | optional post-route exercise menu |
| L035 | public summary | [docs/learning/repository-improvement-suggestions.md](../docs/learning/repository-improvement-suggestions.md) | not-reviewed | learner-facing summary, not execution tracker |

## Exercise-Pack Inventory

| Surface ID | Mode | Placement | Surface | Review Status | Notes |
| --- | --- | --- | --- | --- | --- |
| X001 | validation pack | core route | [src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md](../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) | pass | Lesson 01 fundamentals pack |
| X002 | validation pack | core route | [src/exercises/Beginner/001-joins-and-aggregations/README.md](../src/exercises/Beginner/001-joins-and-aggregations/README.md) | pass | Lesson 02 joins and aggregates pack |
| X003 | validation pack | optional early follow-on | [src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md](../src/exercises/Beginner/002-filtering-constraints-and-data-quality/README.md) | not-reviewed | early filtering and data-quality follow-up outside the core matrix |
| X004 | validation pack | core route | [src/exercises/Intermediate/001-window-functions-and-pagination/README.md](../src/exercises/Intermediate/001-window-functions-and-pagination/README.md) | not-reviewed | Lesson 03 primary pack |
| X005 | validation pack | core route | [src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md](../src/exercises/Intermediate/002-cohort-analysis-and-pagination-drift/README.md) | not-reviewed | Lesson 03 follow-up pack |
| X006 | validation pack | core route | [src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md](../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) | not-reviewed | Lesson 06 primary pack |
| X007 | validation pack | core route | [src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md](../src/exercises/Advanced/002-staged-backfill-and-contract-enforcement/README.md) | not-reviewed | Lesson 04 primary pack |
| X008 | guided lab | core route | [src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md](../src/exercises/Advanced/003-plan-cache-memory-grants-and-waits/README.md) | not-reviewed | Lesson 07 primary lab |
| X009 | guided lab | optional deepening | [src/exercises/Advanced/004-query-store-and-regression-triage/README.md](../src/exercises/Advanced/004-query-store-and-regression-triage/README.md) | not-reviewed | optional performance deepening after Lesson 06 or 10 |
| X010 | guided lab | core route | [src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md](../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md) | not-reviewed | Lesson 09 read-path entry lab bundle |
| X011 | validation pack | core route | [src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md](../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md) | not-reviewed | Lesson 05 primary pack with deadlock-graph companion |
| X012 | guided lab | core route | [src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md](../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) | not-reviewed | Lesson 09 write-side follow-up with supplemental review pack |
| X013 | validation pack | core route | [src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md](../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md) | not-reviewed | Lesson 11 synthesis pack |
| X014 | investigation pack | core route | [src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md](../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md) | not-reviewed | Lesson 10 primary incident pack |
| X015 | guided lab | core route | [src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md](../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) | not-reviewed | Lesson 09 generated-SQL and N+1 follow-up |
| X016 | investigation pack | core route | [src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md](../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md) | not-reviewed | Lesson 08 primary release-readiness pack |
| X017 | investigation pack | optional extension | [src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md](../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md) | not-reviewed | DBA or DBRE recovery specialization |
| X018 | investigation pack | optional extension | [src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md](../src/exercises/Senior/008-sql-server-agent-job-safety-and-operational-scheduling/README.md) | not-reviewed | DBA or DBRE scheduling specialization |
| X019 | investigation pack | optional extension | [src/exercises/Senior/009-ha-dr-failover-posture-and-verification/README.md](../src/exercises/Senior/009-ha-dr-failover-posture-and-verification/README.md) | not-reviewed | DBA or DBRE HA or DR specialization |
| X020 | investigation pack | optional extension | [src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md](../src/exercises/Senior/010-change-capture-provenance-and-reconciliation/README.md) | not-reviewed | DBA or DBRE change-capture specialization |
| X021 | investigation pack | optional extension | [src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md](../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md) | not-reviewed | DBA or DBRE security specialization |
| X022 | guided lab | optional extension | [src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md](../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md) | not-reviewed | DBA or DBRE row-level-security specialization |

## Supporting Alignment Surfaces

These are evidence or routing surfaces, not primary lesson or pack score owners:

- [README.md](../README.md)
- [docs/README.md](../docs/README.md)
- [docs/exercises/README.md](../docs/exercises/README.md)
- [src/exercises/README.md](../src/exercises/README.md)
- [docs/sql/README.md](../docs/sql/README.md)
- [docs/efcore/README.md](../docs/efcore/README.md)
- [docs/operations/README.md](../docs/operations/README.md)
- [docs/performance/README.md](../docs/performance/README.md)
- [docs/phases/README.md](../docs/phases/README.md)
- [Repo Map](repo-map.md)
- [Validation Matrix](validation-matrix.md)

## Findings Backlog

Record corrective work here after an individual review produces action. Reuse the `L###` or `X###` surface ID when the finding belongs to one learner doc or one exercise pack, and use `R###` for cross-surface routing or consistency fixes.

| ID | Priority | Status | Surface | Finding | Evidence | Expected Outcome |
| --- | --- | --- | --- | --- | --- | --- |
| L003 | P1 | not-started | [docs/learning/how-to-start.md](../docs/learning/how-to-start.md) | clarify that the early full-stack path still centers the six core route tables, because API startup now also seeds the optional `academy.TenantOrders` sample and makes the current “six seeded tables” wording too absolute | [docs/learning/how-to-start.md](../docs/learning/how-to-start.md), [SqlAcademy.Api startup](../src/apps/SqlAcademy.Api/Program.cs), [application seed initializer](../src/libs/SqlAcademy.Persistence/Initialization/LearningDbSeed.cs) | setup guidance stays factually correct without front-loading the extension-track sample as a required early-route concept |
| L004 | P2 | not-started | [docs/learning/sql-first-day-one.md](../docs/learning/sql-first-day-one.md) | clarify that the database-first path shows six core seeded tables, while the optional full-stack path can also surface the app-seeded tenant-order sample after API startup | [docs/learning/sql-first-day-one.md](../docs/learning/sql-first-day-one.md), [database-first startup script](../infra/scripts/start-database-first.ps1), [application seed initializer](../src/libs/SqlAcademy.Persistence/Initialization/LearningDbSeed.cs) | the smallest-startup guide stays accurate for both startup options without confusing day-one learners about extension-track data |
| L006 | P1 | not-started | [docs/learning/01-sql-fundamentals.md](../docs/learning/01-sql-fundamentals.md) | revise the lesson framing so it explicitly teaches the six core route tables first rather than implying they are the only seeded tables present after normal full-stack startup | [docs/learning/01-sql-fundamentals.md](../docs/learning/01-sql-fundamentals.md), [LearningDb schema bootstrap](../db/schemas/001_create_learning_db.sql), [application seed initializer](../src/libs/SqlAcademy.Persistence/Initialization/LearningDbSeed.cs) | Lesson 01 remains factually correct while preserving its intended early-route focus on the main academy domain |

## Current Slice

### Completed

- created the dedicated lesson-and-exercise audit owner surface under `.ai`
- seeded the review rubrics so depth, correctness, routing, and validation are judged consistently
- listed every file under `docs/learning` and every pack under `src/exercises` exactly once as the initial audit inventory
- completed the first concrete early-route audit pass: `X001` currently passes, while `L003`, `L004`, and `L006` need wording fixes around the full-stack seeded-table story
- confirmed `L007` and `X002` as clean passes: the joins lesson is repository-grounded, and the first join pack has the expected assets, prerequisites, and validation contract

### Next Recommended Slice

- start with `L001` through `L009` and `X001` through `X007` so the first findings cover route entry, early lessons, and the first wave of validation packs before later-route abstractions add noise

## Acceptance Checks

1. Every file under `docs/learning` appears exactly once in the learner-doc inventory.
2. Every exercise pack under `src/exercises` appears exactly once in the exercise-pack inventory.
3. Each reviewed surface ends in one of: `pass`, `findings-recorded`, or `blocked`; do not leave completed review work at `reviewing`.
4. Every corrective finding cites at least one concrete repository evidence anchor and classifies the gap as incorrect content, insufficient depth, missing learner guidance, missing link or routing, or intentional out-of-scope material.
5. Route-consistency fixes update the owning lesson or pack first, then the supporting routers only when the owner change requires it.