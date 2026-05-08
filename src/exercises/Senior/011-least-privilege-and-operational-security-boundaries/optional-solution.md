# Optional Solution

## Decision

Treat the current posture as local-learning only. Reusing `sa` for the API, worker, and migration fallback, while also allowing the API to initialize the database on startup, is too permissive for any shared environment even before you reach full production.

## Minimum Separation-Of-Duties Contract

1. Stop treating the runtime API and worker as database administrators; give runtime access only the permissions needed for normal application work.
2. Move schema-application authority into an explicit deployment or operator-owned step instead of silent startup execution.
3. Require migration creation and execution to use explicit credentials or an explicit operator workflow rather than a privileged fallback default.
4. Treat admin surfaces such as Grafana credentials as owned operator boundaries with reviewable rotation and storage expectations, not as permanent convenience defaults.

## Operational Proof

The environment becomes more credible only when the runtime no longer relies on `sa`, startup does not silently apply privileged changes, and the operator can point to one explicit reviewed step that owns migration authority instead of hiding it inside process startup.
