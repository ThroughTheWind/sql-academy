# Lesson 04: Schema Design And Migration Safety

## Focus

Schema design is not only about modeling entities correctly. It is also about introducing change safely in a live system. A schema change that is logically correct but operationally unsafe is still a bad change.

## Why This Lesson Matters

Many engineers can design tables that look reasonable on a whiteboard. Fewer can answer:

- what constraints enforce the intended rules?
- how will this design behave under real query patterns?
- how will this schema evolve without blocking production traffic?

This lesson treats data modeling and migration safety as the same discipline.

## Repository Anchors

- [Phase 3: Schema Design](../phases/phase-3-schema-design.md)
- [Schema bootstrap SQL](../../db/schemas/001_create_learning_db.sql)
- [Migration strategy notes](../../db/migrations/README.md)
- [Advanced 001: Indexing, Parameter Sniffing, And Migration Safety](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md)
- [Order EF mapping](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs)

## The Design Questions Every Table Should Answer

Before you create or change a table, answer these questions explicitly:

1. What entity or event does this row represent?
2. What uniquely identifies it to the business?
3. What uniquely identifies it to the database?
4. Which relationships are mandatory and which are optional?
5. Which columns are immutable facts and which are mutable state?
6. Which query patterns will read this data most often?
7. Which operational risks does this change introduce?

If you cannot answer those questions, the schema is not ready.

## Study The Existing Schema As Design Examples

The current schema already shows several intentional choices:

- surrogate integer primary keys across all tables
- business-level uniqueness on `UserName`, `Email`, `Symbol`, and `OrderNumber`
- foreign keys for relationship integrity
- timestamp precision standardized with `DATETIME2(3)`
- `ROWVERSION` on `academy.Orders` to support optimistic concurrency
- indexes shaped around common access paths such as `(UserId, CreatedUtc)`

These are not arbitrary. They encode design decisions about correctness and workload.

## Good Schema Design Principles In This Repo

### Keys Should Support Both Identity And Change

Using a surrogate primary key does not eliminate business uniqueness. It separates internal identity from mutable business fields.

Example:

- `academy.Users.Id` is the stable database key
- `academy.Users.Email` is still protected by a unique index because duplicates would be invalid

### Nullability Is A Business Rule

A nullable column means “absence is valid.” A non-nullable column means “every row must supply this fact.” Do not mark a column nullable just to make the first migration easier.

### Constraints Beat Convention

If an invariant matters, prefer enforcing it in the database:

- use foreign keys for relationship validity
- use unique indexes for duplicate prevention
- use defaults where the database should supply a safe value

### Indexes Are Part Of Design, Not An Afterthought

A table definition is incomplete if you know the dominant read paths but ignore how they will access the data.

## Normalization Versus Denormalization

Normalization reduces duplication and update anomalies. Denormalization can reduce join cost or simplify specific reads. Neither is automatically superior.

Ask these questions:

- will duplicated values drift out of sync?
- is the read pattern frequent enough to justify stored redundancy?
- can the same benefit be achieved with a better query or index instead?

In a learning repository like this one, the normalized model is valuable because it teaches relationship reasoning directly.

## Migration Safety Starts With Change Shape

The safest production migrations are usually additive and staged.

Dangerous patterns often include:

- adding a non-null column to a large populated table without a rollout plan
- rewriting or backfilling the entire table in one long transaction
- dropping or renaming columns that old application versions still expect
- creating wide indexes without understanding build cost and write amplification

## The Safer Pattern: Add, Backfill, Enforce, Cut Over

Suppose you want to add a required `ExternalReference` to `academy.Orders`.

The risky version is:

```sql
ALTER TABLE academy.Orders
ADD ExternalReference NVARCHAR(64) NOT NULL;
```

That can fail immediately because existing rows have no value.

The safer rollout is usually:

1. add the column as nullable
2. deploy application code that can write both old and new contract safely
3. backfill existing rows in controlled batches
4. validate that no nulls remain
5. enforce `NOT NULL`
6. remove any temporary compatibility logic later

Example shape:

```sql
ALTER TABLE academy.Orders
ADD ExternalReference NVARCHAR(64) NULL;

UPDATE academy.Orders
SET ExternalReference = OrderNumber
WHERE ExternalReference IS NULL;

ALTER TABLE academy.Orders
ALTER COLUMN ExternalReference NVARCHAR(64) NOT NULL;
```

In production you would usually batch the backfill instead of doing it in one all-at-once statement.

## Backfills Need Operational Thinking

A backfill is not “just an update.” It can:

- hold locks for too long
- generate log growth
- contend with OLTP traffic
- create deadlocks if it touches hot rows in poor order

That means you should think about:

- batch size
- ordering
- retry strategy
- release window
- monitoring during execution

## Contract Changes Need Compatibility Windows

A schema rarely changes in isolation. The application, jobs, dashboards, and scripts may all depend on the current contract.

Safer release posture usually means:

- introduce the new schema in a backward-compatible way
- deploy application code that can tolerate both old and new shape
- cut reads or writes over deliberately
- remove old contract only after all consumers are updated

This matters especially when multiple services or long-lived workers are involved.

## Read The EF Core Mapping As Schema Documentation

In [Order EF mapping](../../src/libs/SqlAcademy.Persistence/Database/Configurations/OrderConfiguration.cs), the mapping reinforces database intent:

- `OrderNumber` is required and unique
- `Status` is converted to a string with a fixed max length
- `TotalAmount` has explicit precision
- `RowVersion` is marked as a row version for optimistic concurrency
- the `(UserId, CreatedUtc)` index matches a likely read pattern

That alignment is what you want: schema, ORM mapping, and query shape all telling the same story.

## A Practical Migration Review Checklist

Before approving a migration, ask:

- is the change additive or destructive?
- what happens to existing rows?
- what locks could this operation take, and for how long?
- is there a compatibility window for old and new application versions?
- how will you validate success?
- how will you recover if the release stalls halfway?

If the answer to recovery is “restore backup and hope,” the change is not mature enough.

## Common Failure Modes

### Designing Only For The Current Query

Schema lasts longer than a single report. Avoid overfitting the model to one immediate endpoint.

### Pushing Integrity Into Application Code Only

If correctness matters, database constraints should help enforce it.

### Treating Migrations As Development-Time Mechanics Only

A migration is production behavior. It must be reviewed like code that runs under load.

### Choosing Unsafe Speed Over Safe Sequence

A one-step destructive change may feel simpler, but a staged rollout is often the only production-safe answer.

## A Good Practice Sequence

1. Read the schema bootstrap file table by table.
2. Identify where uniqueness, foreign keys, and defaults are enforced.
3. Pick one existing table and explain its likely read patterns.
4. Design one new column addition and write both the unsafe and the safer rollout versions.
5. Review [Advanced 001](../../src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety/README.md) with a production mindset, not only a correctness mindset.

## Exit Criteria

You are ready for Lesson 05 when you can do all of the following:

- justify a table design in terms of integrity, workload, and change safety
- explain why a nullable-add, backfill, enforce sequence is safer than a direct required-column change
- identify which migration steps are likely to block or contend under traffic
- reason about compatibility windows between schema and application code
- review a migration as an operational event instead of a file-generation artifact

## Review Questions

1. Why is a surrogate primary key not a replacement for business-level uniqueness?
2. Why should nullability be treated as a business rule instead of a migration convenience?
3. What makes an additive staged migration safer than an all-at-once destructive change?
4. Why can a backfill be operationally risky even when the update logic is correct?
5. What is a compatibility window, and why does it matter during schema evolution?

## Suggested Answers

1. The surrogate key gives the row stable database identity, but business columns still need their own uniqueness rules if duplicates would be invalid to the domain.
2. Allowing `NULL` means the absence of a value is valid for the business, so using nullability only to make deployment easier can weaken the model incorrectly.
3. A staged additive migration reduces blocking and rollback risk by separating schema introduction, data movement, enforcement, and cleanup into safer phases.
4. Backfills can hold locks, grow the log, contend with live traffic, and create deadlocks or latency spikes if they are done in large uncontrolled batches.
5. A compatibility window is the period where old and new application/schema contracts must both work, which prevents deployment order from breaking running systems.

## Challenge Questions

1. Propose a new required column for `academy.Orders` and describe a production-safe rollout that avoids a one-step breaking change.
2. Explain how you would review an index request that helps one report but makes write-heavy tables more expensive.
3. Compare a normalized design and a denormalized design for one new reporting requirement and defend the tradeoff you would choose here.

## Interview-Style Prompts

1. Explain to an interviewer why “the migration works locally” is not enough evidence that it is safe.
2. Defend a staged migration strategy to a reviewer who wants a faster one-step schema rewrite.

## Model Answer Rubrics

### Challenge Questions

A strong set of challenge answers should include:

- a staged rollout shape that separates schema addition, data movement, validation, and enforcement
- explicit consideration of write cost, integrity rules, and workload impact rather than only schema aesthetics
- a tradeoff discussion between normalized and denormalized designs tied to a real access pattern

### Interview-Style Prompts

A strong spoken answer should include:

- a distinction between logical correctness and production safety
- concrete reasons a one-step rewrite can be dangerous under traffic
- a defense of compatibility windows, rollback posture, and observable release checkpoints

## Lesson Checkpoint

- classify one change as additive and one as breaking
- outline a nullable, backfill, validate, enforce rollout in the correct order
- name one migration you would stop and redesign before release

## Next Lesson

Move to [Lesson 05](05-transactions-blocking-and-deadlocks.md) when you can reason about safe change, not just correct change.