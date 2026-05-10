# Field Type Selection Guide

Use this guide when the schema question is not only what columns do I need, but also which SQL Server field type each column should use, and why.

This is the learner-facing owner surface for practical SQL Server field selection in SqlAcademy. It starts with the repo defaults, then widens out to common SQL Server types that are not currently used in the core schema so you can choose them intentionally instead of by habit.

Keep [Schema Quick Reference](schema-quick-reference.md) nearby when you need the seeded table map, and keep [Schema bootstrap SQL](../../db/schemas/001_create_learning_db.sql) nearby when you want to inspect the actual definitions the repository uses.

## Repo Defaults At A Glance

| Need | Default In This Repo | Typical Use | Repo Example | Avoid First |
| --- | --- | --- | --- | --- |
| stable internal row identity | `INT IDENTITY(1,1)` | primary keys for application tables | `academy.Users.Id`, `academy.Orders.Id` | using mutable business text as the primary key |
| relationship key | `INT` | foreign keys that point to the same integer primary key family | `academy.Posts.UserId`, `academy.Trades.InstrumentId` | mismatching parent and child key types |
| user-facing or business text | `NVARCHAR(n)` | names, emails, titles, bodies, symbols, statuses | `UserName NVARCHAR(64)`, `Title NVARCHAR(256)` | defaulting to `VARCHAR` or `NVARCHAR(MAX)` without a reason |
| exact totals | `DECIMAL(18,2)` | currency-like totals and amounts that must stay exact | `academy.Orders.TotalAmount` | `FLOAT`, `REAL`, or `MONEY` for business totals |
| exact quantities or prices with finer scale | `DECIMAL(18,4)` | prices, quantities, tick sizes, lot sizes | `academy.Trades.Price`, `academy.Instruments.TickSize` | approximate numeric types for exact business math |
| event timestamp in UTC | `DATETIME2(3)` | created, updated, traded, and other event times | `CreatedUtc DATETIME2(3)`, `TradedUtc DATETIME2(3)` | legacy `DATETIME` as the default choice |
| optimistic concurrency token | `ROWVERSION` | write-conflict detection, not business time | `academy.Orders.RowVersion` | using it as a clock, sort key, or business version number |

## Wider SQL Server Type Families At A Glance

| Need | Usually Start With | Consider Instead | Use It When | Common Wrong First Answer |
| --- | --- | --- | --- | --- |
| small bounded whole number | `TINYINT` or `SMALLINT` | `INT` | the range is truly small and stable | using `INT` for everything without checking scale |
| general whole number | `INT` | `BIGINT` | counts, identities, and foreign keys fit normal application ranges | jumping to `BIGINT` with no volume reason |
| very large whole number | `BIGINT` | none smaller | the row volume or counter range can exceed `INT` safely | waiting until overflow risk is already near |
| externally generated identifier | `UNIQUEIDENTIFIER` | integer key plus separate external ID | the ID must exist before SQL Server sees the row | using GUIDs by reflex for every primary key |
| normal bounded text | `NVARCHAR(n)` | `VARCHAR(n)` | the data is user-facing or business-facing text | assuming ASCII-only input forever |
| fixed-width protocol text | `CHAR(n)` or `NCHAR(n)` | bounded variable-width text | the value is truly fixed-length | using fixed-width text for ordinary names or codes |
| very large text or documents | `NVARCHAR(MAX)` | bounded `NVARCHAR(n)` | the size is genuinely unbounded or document-like | choosing MAX because the real limit is unknown |
| exact business numeric | `DECIMAL` or `NUMERIC` | integer family for whole counts | amounts, prices, quantities, and rates must stay exact | using `FLOAT` or `REAL` for business math |
| approximate scientific numeric | `FLOAT` or `REAL` | `DECIMAL` | approximate measurement is acceptable | using approximate numeric types for finance or contract values |
| date only | `DATE` | `DATETIME2` | the calendar day matters but the instant does not | storing midnight timestamps just to fake a date |
| time only | `TIME` | `DATETIME2` | time-of-day matters without date context | storing partial facts in a full timestamp by habit |
| event timestamp | `DATETIME2` | `DATETIMEOFFSET` | you need one concrete instant, usually in UTC | defaulting to legacy `DATETIME` |
| offset-preserving timestamp | `DATETIMEOFFSET` | UTC `DATETIME2` | the stored offset itself matters to the contract | using it when UTC storage would be simpler |
| binary payload | `VARBINARY(n)` or `VARBINARY(MAX)` | fixed-width `BINARY(n)` | hashes, tokens, encrypted blobs, or files must be stored | forcing binary data into text columns |
| XML document | `XML` | `NVARCHAR(MAX)` | SQL Server XML querying or validation is actually useful | using XML just because the payload looks nested |
| JSON document | `NVARCHAR(MAX)` | normalized tables or XML | the payload is semi-structured and SQL Server JSON functions are enough | pretending SQL Server has a separate JSON storage type |
| true or false flag | `BIT` | status text or lookup key | the field is genuinely two-state | collapsing a multi-state workflow into a boolean |
| optimistic concurrency token | `ROWVERSION` | explicit version number only when the domain needs it | the application must detect stale writes | treating `ROWVERSION` like business time |

## How To Choose A Field Type

Before you pick a type, answer these questions:

1. Is this value an internal identifier, a business identifier, user text, an exact number, a timestamp, a true or false flag, or a concurrency token?
2. Does the value need exact arithmetic or only rough approximation?
3. Will humans type or read the value, and can it contain Unicode characters?
4. Is this a single instant in time, a calendar date, or a concurrency version?
5. Is the field truly two-state, or is it really a multi-state workflow that only looks boolean at first glance?

If you cannot answer those questions, the type choice is probably still too loose.

## Keys And Identifiers

### `TINYINT`, `SMALLINT`, `INT`, And `BIGINT` For Whole Numbers

These types are mainly about range, not meaning.

Use them when:

- the value is a whole number rather than text or decimal data
- the domain has a clear upper and lower bound
- you want the column type to reflect that range honestly

Practical rule:

- use `TINYINT` for very small positive ranges
- use `SMALLINT` for modest ranges that still do not need `INT`
- use `INT` as the normal application default
- use `BIGINT` only when the volume or counter range can outgrow `INT`

This repository uses `INT` for the core schema because that matches its current scale, not because the other integer types are wrong.

### `INT IDENTITY(1,1)` For Surrogate Primary Keys

This is the default key strategy across the core schema.

Use it when:

- the database needs a stable internal identifier
- business fields can change or should stay separate from row identity
- the application is centered on one SQL Server database rather than disconnected global ID generation

Repo examples:

- `academy.Users.Id`
- `academy.Instruments.Id`
- `academy.Orders.Id`

Common failure mode:

- using a business field such as email, symbol, or order number as the primary key instead of protecting it with a separate uniqueness rule

### `INT` For Foreign Keys

Foreign keys should stay in the same type family as the parent key they reference.

Use it when:

- the parent table already uses `INT IDENTITY`
- you want joins, indexes, and mappings to stay simple and aligned

Repo examples:

- `academy.Posts.UserId`
- `academy.Trades.InstrumentId`

Common failure mode:

- choosing a parent key type and child key type that do not match exactly, which complicates joins, mappings, or later migrations

### `UNIQUEIDENTIFIER` Only When The ID Must Exist Outside The Database First

This repository does not use `UNIQUEIDENTIFIER` as the default key shape.

Consider it when:

- identifiers must be generated safely before SQL Server sees the row
- multiple disconnected writers must produce IDs without central coordination

Do not choose it first when:

- a simple database-generated integer key already satisfies the identity problem

The main lesson here is not that `UNIQUEIDENTIFIER` is wrong. It is that the right identifier type depends on where the ID must be created, how it will be joined, and whether a simpler integer key already solves the problem.

## Text And Unicode

### `NVARCHAR(n)` Is The Default For Business And User Text

Use `NVARCHAR(n)` when the value can contain human language, user-entered text, or domain text that should not assume ASCII-only input.

Use it when:

- the data is user-facing or business-facing text
- you want a bounded maximum length instead of unbounded text by default
- the field may reasonably contain Unicode characters

Repo examples:

- `UserName NVARCHAR(64)`
- `Email NVARCHAR(256)`
- `Title NVARCHAR(256)`
- `Body NVARCHAR(4000)`

Common failure modes:

- using `VARCHAR` for application text and silently narrowing what input is safe
- choosing `NVARCHAR(MAX)` too early when the domain clearly has a bounded size
- choosing an arbitrary small max length without tying it to a real business constraint

### Bounded `NVARCHAR` Also Works For Status And Code-Like Text

Not every bounded string is prose. Some are codes, short states, or controlled business identifiers.

Use it when:

- the value is short but still belongs to the string domain
- the set of values is controlled by the application or business contract
- you want a practical maximum length instead of an unbounded string

Repo examples:

- `Symbol NVARCHAR(24)`
- `AssetClass NVARCHAR(32)`
- `Status NVARCHAR(32)`
- `Side NVARCHAR(16)`

Common failure mode:

- treating every status-like value as a boolean when the workflow is really multi-state

### `VARCHAR`, `CHAR`, And `NCHAR` Need A More Specific Reason

This repository does not use these as its default application-data choice.

Consider them only when:

- the data contract is intentionally non-Unicode and tightly controlled
- the value is truly fixed-width and protocol-like rather than normal business text

Do not choose them first when:

- `NVARCHAR(n)` already expresses the domain safely and clearly

### `NVARCHAR(MAX)` Only When The Content Is Truly Unbounded

Use `NVARCHAR(MAX)` when:

- the value is document-like or genuinely long-form
- a realistic bounded maximum is not part of the contract

Do not choose it first when:

- the business domain already suggests a practical upper bound
- the column is likely to participate in hot query shapes where oversized text storage becomes noise or cost

The current repo mostly prefers bounded `NVARCHAR(n)` because the core tables model operational application data rather than free-form document storage.

## Numeric Values

### `DECIMAL(p,s)` For Exact Business Math

Use `DECIMAL` when the number must be exact, especially for totals, prices, quantities, or sizes that participate in business logic.

Use it when:

- rounding rules matter
- equality and comparisons must behave predictably
- the number represents quantity, money-like totals, or price-like values

Repo examples:

- `TotalAmount DECIMAL(18,2)`
- `Quantity DECIMAL(18,4)`
- `Price DECIMAL(18,4)`
- `TickSize DECIMAL(18,4)`

Common failure mode:

- using approximate numeric types and then acting surprised when arithmetic and comparison behavior drift from business expectations

### Choosing Scale In This Repo

The repository uses two common defaults:

- `DECIMAL(18,2)` for totals that behave like currency or order amount summaries
- `DECIMAL(18,4)` for prices, quantities, lot sizes, and tick sizes where finer scale matters

That does not make those precisions universal laws. It means they match the current domain examples here.

### `NUMERIC(p,s)` Solves The Same Practical Problem

In SQL Server, `NUMERIC` and `DECIMAL` solve the same exact-arithmetic problem.

Use either when:

- the main requirement is exactness plus explicit precision and scale

The academy uses `DECIMAL` in the schema examples because that is the convention already present in the repo.

### `MONEY` And `SMALLMONEY` Need Extra Caution

Consider them only when:

- you have a strong compatibility reason or a legacy contract that already depends on them

Do not choose them first when:

- `DECIMAL(p,s)` would express the scale and intent more explicitly

### Avoid `FLOAT`, `REAL`, And Usually `MONEY` For Business Facts

Do not choose `FLOAT` or `REAL` first when:

- the value must stay exact across inserts, updates, comparisons, and reporting

Be cautious with `MONEY` too. `DECIMAL(p,s)` is usually clearer and more explicit for application-facing financial data.

### `FLOAT` And `REAL` Are For Approximate Measurements, Not Contracts

Consider them when:

- approximation is acceptable
- the value behaves more like measurement or scientific data than a contractual business fact

Do not choose them when:

- the value participates in exact comparison, billing, financial totals, or externally visible contract behavior

## Time And Date Types

### `DATETIME2(3)` Is The Repo Default For UTC Event Times

Use `DATETIME2(3)` for created, updated, and event timestamps when the application stores a concrete instant in time.

Use it when:

- you want a timestamp with modern SQL Server precision behavior
- the data is recorded in UTC
- you need the same type family consistently across schema, queries, and EF mappings

Repo examples:

- `CreatedUtc DATETIME2(3)`
- `UpdatedUtc DATETIME2(3)`
- `TradedUtc DATETIME2(3)`

Common failure modes:

- defaulting to legacy `DATETIME` out of habit
- mixing local time and UTC semantics in the same column family
- using a timestamp column as if it implies natural ordering without an explicit `ORDER BY`

### `DATE` Or `TIME` Only When You Truly Need Part Of The Instant

This repository mostly stores full UTC instants, so `DATETIME2(3)` is the norm.

Consider `DATE` when:

- only the calendar day matters

Consider `TIME` when:

- only a time-of-day fact matters and no date context belongs in the same field

Do not choose them when:

- the application really needs the full event instant later for ordering, debugging, or release analysis

### `DATETIMEOFFSET` Only When Offset Preservation Matters

This repo does not use `DATETIMEOFFSET` by default.

Consider it when:

- the stored offset itself is part of the business fact you need to preserve

Do not choose it first when:

- UTC storage plus application-side presentation already solves the problem more simply

### `DATETIME` And `SMALLDATETIME` Are Usually Legacy Compatibility Choices

Consider them only when:

- an existing system or legacy contract forces you to keep them

Do not choose them first when:

- `DATETIME2` already solves the problem with clearer modern behavior

## Binary Values And Payloads

### `VARBINARY(n)` And `VARBINARY(MAX)` For Binary Data

Use them when:

- the value is a hash, token, encrypted blob, file payload, or other true binary content
- text encoding would be the wrong abstraction for the stored value

Use `VARBINARY(n)` when the size is bounded and `VARBINARY(MAX)` when the content is genuinely large or document-like.

Do not choose text columns when:

- the value is fundamentally binary rather than human-readable text

### `BINARY(n)` Only For Truly Fixed-Width Binary Values

Consider it when:

- every value is always the same byte length

Do not choose it first when:

- a variable-width binary contract would be simpler and more honest

## Semi-Structured Documents

### `XML` When SQL Server XML Features Matter

Consider it when:

- SQL Server XML querying, indexing, or validation is part of the real workload

Do not choose it first when:

- the content only needs to be stored and occasionally passed through

### JSON In SQL Server Usually Means `NVARCHAR(MAX)` Plus Validation

SQL Server does not have a separate native JSON storage type in the same way some other databases do.

Consider JSON text when:

- the payload is semi-structured
- the schema is intentionally flexible
- SQL Server JSON functions are enough for the read patterns you actually need

Do not choose it first when:

- the fields are relational and deserve normal columns, keys, and indexes

## Booleans, States, And Workflow Columns

### `BIT` For True Or False Only

Use `BIT` when the field is genuinely binary.

Use it when:

- the answer is only yes or no
- adding a third state would mean the domain is actually different

Do not choose it when:

- the process has states such as pending, submitted, filled, cancelled, failed, or retried

In this repository, workflow-heavy columns such as `academy.Orders.Status` are strings because the domain is multi-state, not binary.

If the workflow is larger or more controlled than a short status string should carry, consider a lookup table or strongly defined domain contract instead of a bare boolean or ad hoc free-text status.

## Concurrency And Versioning

### `ROWVERSION` For Optimistic Concurrency Tokens

Use `ROWVERSION` when the application needs to detect whether a row changed between read and write.

Use it when:

- you need optimistic concurrency checks
- the write path should reject stale updates instead of silently overwriting them

Repo example:

- `academy.Orders.RowVersion`

Common failure modes:

- treating `ROWVERSION` like a timestamp
- ordering rows by `ROWVERSION` as if it were business time
- exposing it to users as a meaningful business version rather than a concurrency token

For the application-side follow-up, pair this with [Lesson 05: Transactions, Blocking, And Deadlocks](05-transactions-blocking-and-deadlocks.md) and the later EF write-path material.

## Repo Defaults Worth Memorizing

These are the repo defaults, not universal laws:

- use `INT IDENTITY(1,1)` for surrogate primary keys in the core schema
- use matching `INT` foreign keys for those relationships
- use bounded `NVARCHAR(n)` for business and user text
- use `DECIMAL(18,2)` for totals and `DECIMAL(18,4)` for finer-grained exact numeric values
- use `DATETIME2(3)` for UTC event timestamps
- use `ROWVERSION` for optimistic concurrency tokens, not time
- choose `BIT` only when the domain is truly two-state

## Common "Wrong First Answer" Patterns

- `VARCHAR` just because the text looks English-only today
- `UNIQUEIDENTIFIER` just because distributed IDs sound modern, even when a simpler integer key already solves the problem
- `FLOAT` or `REAL` for prices, quantities, or totals that must stay exact
- `NVARCHAR(MAX)` before checking whether a clear bounded size already exists
- `DATETIMEOFFSET` when the system only needs a UTC instant
- `XML` or JSON storage before checking whether the data should simply be relational
- `BIT` for a workflow that clearly has more than two states
- `DATETIME` because it is familiar, even though `DATETIME2` is the better default here
- `ROWVERSION` because it sounds like a generic version field rather than a concurrency token

## Best Companions

1. [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md)
2. [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
3. [Schema Quick Reference](schema-quick-reference.md)
4. [SQL Track](../sql/README.md)
5. [Schema bootstrap SQL](../../db/schemas/001_create_learning_db.sql)