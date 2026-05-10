# Relational Databases And Course Fit

Use this page when you are still deciding whether a relational database is the right tool for your problem and whether this academy is the right route for you.

This page explains what a relational database is, what problem it solves, when to use one, when not to start with one, how SQL Server compares with PostgreSQL and MySQL, and what value the academy is designed to deliver.

The default academy route is built for working backend and application engineers who can already navigate a local development environment. It is not a zero-assumption introduction to programming, terminals, Docker, or first-ever SQL syntax.

## Why Databases Exist

Applications outgrow files and in-memory objects when:

- more than one user or process needs the same data
- the data must survive restarts, deployments, and crashes
- multiple writes can happen at the same time
- the same facts need to be queried in more than one way
- correctness matters more than convenience

A database is the system that stores those facts, coordinates concurrent access, and gives you a reliable way to read, write, and recover them.

## What Makes A Database Relational

A relational database stores data as related sets of rows. In practice that usually means:

- `tables`, `rows`, and `columns` define the shape of the data
- `primary keys` identify one row uniquely
- `foreign keys` connect one table to another
- `constraints` reject impossible or invalid states
- `joins` answer questions that span multiple tables
- `transactions` let several changes succeed or fail as one unit
- `indexes` make common access patterns fast enough to be useful
- `schema` gives the data model an explicit contract

The relational part is not just that the data lives in tables. It is that the relationships between facts are modeled explicitly and can be queried consistently.

## The Problem Relational Databases Solve

Relational databases are the default answer when you need one operational source of truth for business facts that relate to each other.

They are strong when you need:

- a user, order, post, trade, or invoice to mean the same thing across multiple workflows
- rules such as uniqueness, required references, and valid states enforced close to the data
- many sessions to read and write safely at the same time
- predictable queries over related entities instead of whole-document retrieval only
- controlled schema evolution as the application changes

This is why relational databases stay central in `OLTP` systems. `OLTP` means online transaction processing: the day-to-day operational read and write path for application data.

## Core Concepts Worth Recognizing Early

| Concept | Why It Matters |
| --- | --- |
| `schema` | the contract for tables, columns, keys, and rules |
| `normalization` | separates facts into related tables so updates stay consistent and duplication stays controlled |
| `denormalization` | duplicates or precomputes data intentionally when read simplicity or performance matters more than strict normalization |
| `constraint` | stops invalid states from being stored quietly |
| `transaction` | groups several changes so they succeed or fail together |
| `isolation` | defines what concurrent sessions can see while changes are still in progress |
| `index` | adds an access path that can make reads much faster at the cost of storage and write work |
| `execution plan` | shows how the database actually chooses to answer a query |
| `migration` | applies a schema change in a controlled way over time |

These ideas show up throughout the academy because they are where application code, data modeling, and production behavior meet.

## Use A Relational Database First When

- the data has clear entities and relationships
- correctness, consistency, and concurrent writes matter
- the application needs joins, aggregations, filtering, and ad hoc inspection
- schema changes should be explicit, reviewable, and reversible where practical
- audit, reconciliation, or support work needs stable keys and queryable records
- the workload is operational application data rather than mostly cache traffic or warehouse analytics

## Do Not Start With A Relational Database When

- the only need is a cache or session store and the data can be rebuilt cheaply
- the dominant access pattern is direct key-to-value lookup with almost no relational querying
- the primary unit is a large flexible document and cross-document relationships are weak
- the main problem is many-hop graph traversal or path analysis
- the main workload is warehouse-style scanning and aggregation over large historical slices rather than hot transactional writes

Even in those cases, many real systems still keep a relational database for the core operational truth and add another store beside it for the specific workload a relational engine does not fit well.

## Relational Versus Other Database Families

| Family | Strongest When | Starts Hurting When | Typical Role |
| --- | --- | --- | --- |
| relational | you need transactions, constraints, joins, and one operational source of truth | schema flexibility matters more than data integrity, or the workload is graph-native or warehouse-heavy | core application data and operational reporting |
| document | each record is naturally a self-contained document and the shape changes often | integrity depends on many cross-document relationships or join-heavy reporting | semi-structured application data |
| key-value | reads and writes are mostly exact-key lookups with simple value retrieval | you need rich filtering, constraints, joins, or multi-entity reasoning | caching, sessions, ephemeral state, hot lookup paths |
| graph | the hardest questions are path traversal, network structure, and many-hop relationships | most queries are ordinary CRUD, filtering, and aggregation over business entities | identity graphs, recommendation graphs, connected-network analysis |
| analytical or warehouse | the workload is dominated by large scans, historical aggregates, and BI-style reporting | the same system must also handle strict transactional integrity for hot application writes | analytics, dashboards, historical trend analysis |

Do not read this as "pick one database family forever." Many production systems are polyglot. The important question is which store owns the primary truth for the workload you are building.

## SQL Server Versus PostgreSQL And MySQL

SQL Server, PostgreSQL, and MySQL are all serious relational options. The correct choice usually depends on your team, hosting model, operational habits, licensing posture, and workload details, not on slogans.

| Engine | Often A Good Fit When | Watch For | What Transfers From This Academy |
| --- | --- | --- | --- |
| SQL Server | you want the exact platform used by this repository, including T-SQL, Query Store, SQL Server-specific plan and DMV workflows, and the optional SQL Server Agent extension track | some syntax, tooling, and operational details are vendor-specific | all of it |
| PostgreSQL | you want a widely used open-source relational engine with strong SQL support and a large extension ecosystem | dialect, operational tooling, and engine-specific features differ from the SQL Server route taught here | most relational modeling, query shape, transaction, indexing, and troubleshooting habits |
| MySQL | you need a common application-oriented relational engine with broad hosting support and a straightforward CRUD path for many workloads | version and distribution differences matter, and the optimizer, locking details, and tooling are not the same as SQL Server | schema reasoning, joins, aggregates, indexing basics, and transaction fundamentals |

This academy teaches SQL Server specifically because the repository uses SQL Server-specific features and operational surfaces such as T-SQL, Query Store, `rowversion`, SQL Server plan analysis, and the optional SQL Server Agent, HA or DR, security, and change-capture extension work.

If you use PostgreSQL or MySQL in your day job, much of the academy still transfers:

- relational modeling, keys, joins, and normalization logic
- transaction thinking, blocking awareness, and concurrency tradeoffs
- index design habits and query-shape reasoning
- the discipline of safe schema change and production-minded validation

What does not transfer one-for-one is vendor-specific syntax, tooling, execution-plan UX, data types, and some operational behavior.

## Who This Course Is For

- working backend and application engineers
- .NET engineers who need stronger SQL Server, EF Core, or Dapper depth
- engineers who can already use a terminal, source control, and a local development stack
- people who want performance, concurrency, and operational context instead of isolated query puzzles

## Who Should Not Start Here

- someone learning programming, terminals, Docker, and SQL all at the same time
- someone who only needs a vendor-neutral database survey with no repository or code context
- someone whose main goal is full-time DBA certification or deep platform administration from day one
- someone whose work is primarily warehouse BI, data science notebooks, or graph analytics rather than application data systems

## The Value This Academy Brings

- it ties schema design, query writing, transactions, indexing, plans, EF Core, Dapper, observability, and release safety together in one repo
- it shows how database decisions appear in application code, tests, incidents, and operational runbooks
- it gives you a guided path from first query to production-style reasoning instead of disconnected SQL snippets
- it uses a SQL Server-specific training ground while still teaching relational habits that transfer to other engines

## What Success Looks Like After The Core Route

By the end of the core route, you should be able to:

- explain the core tables and relationships without guessing
- write and debug common read and write queries against operational data
- reason about keys, constraints, normalization, and migration safety
- recognize blocking, deadlocks, plan regressions, and parameter sensitivity as system behaviors instead of mystery failures
- connect SQL behavior back to EF Core, Dapper, tests, telemetry, and runtime symptoms

## Choose Your Next Step

| If You Want | Start Here | Then Use |
| --- | --- | --- |
| a smaller first success before the full route | [SQL-First Day One](sql-first-day-one.md) | [How To Start](how-to-start.md), [Phase 0](../phases/phase-0-local-setup.md), then [Curriculum Map](curriculum-map.md) |
| the default guided route | [How To Start](how-to-start.md) | [Curriculum Map](curriculum-map.md), the current lesson, then the matching exercise pack |
| the shorter application-data route | [SQL To .NET Data Access Path](sql-to-dotnet-data-access-path.md) | rejoin the main route at [Lesson 10](10-observability-testing-and-performance-engineering.md) and [Lesson 11](11-capstones-and-interview-readiness.md) |
| optional DBA or DBRE specialization after the core route | [DBA And DBRE Extension Track](dba-dbre-extension-track.md) | keep the core route as the baseline, then use the specialization menu deliberately |

Use [Learning Glossary](glossary.md) when terms slow you down. Use [SQL Syntax And Query Patterns](../sql/sql-syntax-and-query-patterns.md) when the main blocker is query syntax rather than database choice.