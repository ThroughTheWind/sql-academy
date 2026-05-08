# Privilege Surfaces

## Current Security-Relevant Facts

| Surface | Current Fact | Why It Matters |
| --- | --- | --- |
| API runtime connection | `SqlAcademy.Api` uses a `LearningDb` connection string that authenticates as `sa` in local configuration | runtime writes currently hold full database authority instead of a narrower application boundary |
| worker runtime connection | `SqlAcademy.Worker` also connects as `sa` | background processing shares the same broad privilege boundary as the API |
| startup migration authority | the API calls `InitializeLearningDatabaseAsync` during startup | runtime startup can perform privileged schema or seed work automatically |
| design-time fallback | `DesignTimeLearningDbContextFactory` falls back to a privileged local `sa` connection string | migration creation and review are still coupled to an admin-style convenience path |
| operator admin surface | `docker-compose.yml` passes Grafana admin credentials through environment variables | operator-facing admin surfaces need explicit ownership, not only convenience defaults |
