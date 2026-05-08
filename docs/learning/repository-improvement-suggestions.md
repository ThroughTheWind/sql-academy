# Repository Improvement Suggestions

These are the most useful next improvements after the current learner-path update.

This file is the learner-facing summary, not the execution tracker.

Use [Curriculum Map](curriculum-map.md) for the default course route, use this file for the public summary of what should improve next, and use [../../.ai/sql-efcore-mastery-backlog.md](../../.ai/sql-efcore-mastery-backlog.md) for the tracked maintainer backlog.

Recent learner-path updates already added a SQL-first day-one guide, a compact schema quick reference, sample result-shape sections in the first lessons, and clearer pacing or blocked-path guidance across the early and middle route.

## Near-Term Improvements

- Add copy-paste connection examples for Azure Data Studio, SSMS, and `sqlcmd` so the day-one docs cover the most common client workflows directly.
- Add a dedicated troubleshooting guide for Docker startup, port conflicts, readiness failures, and SQL client login issues.
- Add printable one-page lesson checklists for Lessons 01 through 03 plus the beginner packs.
- Extend sample output sections into the intermediate validation packs so the support pattern stays consistent after the beginner route.
- Add one page that summarizes estimated effort across all lessons and packs instead of only inside individual docs.
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