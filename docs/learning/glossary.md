# Learning Glossary

Use this glossary when a lesson or exercise assumes vocabulary faster than you can comfortably recall it. The goal is speed of recognition, not encyclopedic detail.

## Query And Relational Terms

- `Cardinality`: how many rows SQL Server expects or actually sees at each step of a query.
- `Deterministic order`: a sort order that stays stable because ties are broken explicitly, usually with a key such as `Id`.
- `Join multiplicity`: the way one-to-many joins can multiply rows from the parent side.
- `Predicate`: the condition that filters rows, usually in `WHERE` or `ON`.
- `Projection`: the exact columns or shaped result returned by a query.
- `Sargable predicate`: a predicate the optimizer can match efficiently against indexed values.
- `Selectivity`: how narrowly a predicate reduces the candidate rows.
- `Set-based`: solving the problem as an operation over rows as a set instead of row-by-row loops.

## Indexing And Plan Terms

- `Covering index`: an index that has enough key and included columns to satisfy a query without extra lookups.
- `Execution plan`: SQL Server's chosen strategy for reading, joining, sorting, and aggregating data.
- `Key lookup`: a trip from a nonclustered index back to the base row because the index does not contain all needed columns.
- `Logical reads`: pages read from the buffer pool, often used as a practical cost signal while tuning.
- `Parameter sensitivity`: one cached plan performs well for one parameter shape and poorly for another.
- `Statistics`: summarized distribution information SQL Server uses to estimate row counts.
- `Tempdb spill`: an operator ran out of granted memory and wrote intermediate work to tempdb.

## Concurrency And Integrity Terms

- `Blocking`: one session waits because another session holds a conflicting lock.
- `Deadlock`: two or more sessions form a cycle of waits and SQL Server must choose a victim.
- `Idempotent`: safe to run more than once without producing duplicate side effects.
- `Isolation level`: the rules that control what concurrent data changes a transaction may observe.
- `Optimistic concurrency`: the application checks whether data changed before committing, often with `rowversion`.
- `Retry policy`: application behavior that retries transient failures such as deadlock victims, but not bad transaction design.

## Schema And Release Terms

- `Additive change`: a schema or API change that keeps older consumers working while the new shape rolls out.
- `Backfill`: populate a new column or structure for existing rows before enforcing a stricter contract.
- `Contract enforcement`: the step where the schema starts requiring the new invariant, such as `NOT NULL` or uniqueness.
- `Roll-forward`: fix a bad release by moving to a safer forward state instead of reverting every change.
- `Smoke test`: a small proof that the critical path still works right after deployment.

## EF Core And Application Terms

- `AsNoTracking`: EF Core mode for read-only queries that avoids change-tracking overhead.
- `N+1 query`: one query loads a list and then extra queries are triggered per item for related data.
- `Projection-first read`: shape only the fields you need instead of materializing full entities first.
- `Query shape`: the combined filter, join, sort, pagination, and projection behavior of a read path.
- `Tracking`: EF Core keeps entity state so later changes can be detected and saved.

## Observability Terms

- `Health endpoint`: a runtime endpoint such as `/health/live` or `/health/ready` used to prove the service can do useful work.
- `Log`: discrete event record with detailed context.
- `Metric`: numeric signal aggregated over time for rates, latency, throughput, or pressure.
- `Span`: one timed unit of work inside a trace.
- `Trace`: the ordered path of spans that shows where time went across a request or job.
- `Telemetry`: the combined logs, metrics, traces, and health signals used to observe the system.

## How To Use It

1. Stop when a term is slowing you down.
2. Read only the matching section instead of the full glossary.
3. Return to the lesson or exercise immediately and restate the term in your own words.
4. Use the lesson checkpoint before moving on so the term becomes active vocabulary instead of passive recognition.