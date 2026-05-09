# Repository Improvement Suggestions

These are the most useful next improvements after the current learner-path update.

This file is the learner-facing summary, not the execution tracker.

Use [Curriculum Map](curriculum-map.md) for the default course route, use this file for the public summary of what should improve next, use [../../.ai/academy-enhancements-backlog.md](../../.ai/academy-enhancements-backlog.md) for the active maintainer execution tracker, and use [../../.ai/sql-efcore-mastery-backlog.md](../../.ai/sql-efcore-mastery-backlog.md) for the earlier mastery-scope history.

Recent learner-path updates already added a SQL-first day-one guide, a reduced-startup database-first path, a compact schema quick reference, a SQL client connection guide, a local troubleshooting guide, an effort summary page, sample result-shape sections in the early lessons and intermediate validation packs, and clearer pacing or blocked-path guidance across the early and middle route.

## Near-Term Improvements

- Add a dedicated Compose profile or VS Code task for the reduced database-first startup path so learners do not need to remember service names manually.
- Add screenshots or short terminal transcripts to the connection guide and troubleshooting guide so first-run recovery feels less text-heavy.
- Add printable one-page lesson checklists for Lessons 01 through 03 plus the beginner and intermediate packs.
- Extend sample output sections into the deterministic advanced validation packs so the support pattern stays consistent after the intermediate route.
- Add a compact command cheat sheet for `docker compose`, `sqlcmd`, the validation harness, and the most useful focused test commands.
- Add a local-environment reset FAQ that distinguishes safe retry steps from destructive reset steps.
- Add statistics-drift and skew-focused performance lessons that build on the existing Query Store and larger-cardinality workload route.
- Add dashboard JSON exports so learners get immediate observability visuals after startup.
- Add spaced-repetition drills built from the glossary and lesson checkpoints.

## Medium-Term Improvements

- Expand the incident pack catalog with release, deadlock, ingestion, and outbox failures.
- Add guided pull-request style review exercises for migrations, indexes, and schema changes.
- Expand the printable release and incident runbooks with more scenario-specific variants.
- Expand the DBA and DBRE extension track beyond the current initial specialization packs with deeper follow-on drills for replication, application-wired tenant security policy, and advanced recovery or reconciliation cases.

## Longer-Term Improvements

- Add multiple bounded domains so learners can practice cross-service data concerns.
- Add Kubernetes deployment manifests only after the Docker-first learning path is stable.
- Add automated integration and performance test execution in CI when pipeline runtime budget allows.
- Add richer capstone scoring rubrics for interview and senior-level review sessions.