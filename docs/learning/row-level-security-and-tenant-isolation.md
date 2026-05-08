# Row-Level Security And Tenant Isolation

## Focus

This lesson explains where SQL Server row-level security fits after schema design and least-privilege work are already strong enough to support it, and it now points to one real sample instead of only a hypothetical policy sketch.

The goal is not to treat row-level security as a magic switch. The goal is to decide when a shared-database tenant model is mature enough that a security policy helps more than it hides.

## Why This Lesson Matters

Application-side filters are easy to write and easy to forget.

When tenant data lives in one database, the harder questions are:

- what column actually defines the tenant boundary?
- who sets the tenant context for every connection?
- how do background jobs, migrations, and operators bypass the policy deliberately instead of accidentally?
- how do you prove the policy isolates reads and writes instead of only hoping it does?

If you cannot answer those questions, turning on row-level security is usually theater.

## Repository Anchors

- [DBA And DBRE Extension Track](dba-dbre-extension-track.md)
- [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md)
- [Senior 011: Least Privilege And Operational Security Boundaries](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md)
- [Senior 012: Row-Level Security And Tenant Isolation](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md)
- [TenantSessionContextMiddleware](../../src/apps/SqlAcademy.Api/Infrastructure/TenantSessionContextMiddleware.cs)
- [TenantOrdersController](../../src/apps/SqlAcademy.Api/Controllers/V1/TenantOrdersController.cs)
- [TenantOrderWriteService](../../src/libs/SqlAcademy.Persistence/Commands/TenantOrders/TenantOrderWriteService.cs)
- [SqlSessionContextApplier](../../src/libs/SqlAcademy.Persistence/MultiTenancy/SqlSessionContextApplier.cs)
- [Schema bootstrap SQL](../../db/schemas/001_create_learning_db.sql)
- [TenantOrder EF mapping](../../src/libs/SqlAcademy.Persistence/Database/Configurations/TenantOrderConfiguration.cs)
- [SqlAcademy.Api startup](../../src/apps/SqlAcademy.Api/Program.cs)
- [Senior 012 starter policy](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/starter-policy.sql)

## RLS Is Not Your First Security Boundary

Row-level security belongs after two more fundamental decisions are already in place:

1. the schema has a durable tenant key on every protected table
2. runtime access is already constrained enough that the database policy means something

If the application still connects as `sa`, or the table still has no trustworthy `TenantId`, row-level security cannot rescue the design.

That is why this topic fits after [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md) and after the broader least-privilege review in [Senior 011](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md).

## When Row-Level Security Actually Fits

RLS is most useful when all of the following are true:

- multiple tenants share one database or one schema
- the protected tables all carry a stable tenant discriminator
- the application can set a per-connection tenant context consistently
- reporting, support, migration, and maintenance paths have explicit ownership
- the team wants a database-enforced backstop in addition to application authorization

RLS is a poor fit when the tenant boundary is still ambiguous, when background workers cannot carry the same context safely, or when the team plans to keep using broadly privileged runtime credentials.

## The SQL Server Shape

In SQL Server, the common building blocks are:

- `sys.sp_set_session_context` to stamp the current connection with tenant identity
- an inline table-valued predicate function that compares the tenant column with the current session context
- a `SECURITY POLICY` that attaches filter and optionally block predicates to protected tables

Example shape:

```sql
EXEC sys.sp_set_session_context @key = N'TenantId', @value = 101;
EXEC sys.sp_set_session_context @key = N'RlsBypass', @value = 0;

CREATE FUNCTION security.fn_tenant_order_access(@TenantId INT)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
    SELECT 1 AS fn_tenant_access_result
    WHERE TRY_CONVERT(BIT, SESSION_CONTEXT(N'RlsBypass')) = 1
       OR IS_MEMBER(N'rls_policy_admin') = 1
       OR @TenantId = TRY_CONVERT(INT, SESSION_CONTEXT(N'TenantId'));
GO

CREATE SECURITY POLICY security.TenantOrderIsolationPolicy
ADD FILTER PREDICATE security.fn_tenant_order_access(TenantId) ON academy.TenantOrders,
ADD BLOCK PREDICATE security.fn_tenant_order_access(TenantId) ON academy.TenantOrders AFTER INSERT,
ADD BLOCK PREDICATE security.fn_tenant_order_access(TenantId) ON academy.TenantOrders AFTER UPDATE
WITH (STATE = ON);
```

The point of that example is now visible in the repo: `academy.TenantOrders`, the tenant header middleware, the EF write service, and the focused RLS tests show the contract working on one dedicated sample surface for both reads and writes.

## Filter Predicates Versus Block Predicates

Filter predicates silently hide rows that do not match the current tenant context.

Block predicates stop writes that would cross the boundary.

That distinction matters:

- filter predicates can hide data and make troubleshooting confusing if the operator does not know the connection context
- block predicates can break inserts, updates, or maintenance tasks immediately if the tenant contract is incomplete

Good teams choose both behavior and operator visibility deliberately.

## What The Repo Now Proves And What It Still Does Not

The current academy repository now includes one honest multi-tenant sample, but it is still not a blanket claim that every core table is ready for tenant isolation.

The current sample surfaces are concrete:

- `academy.TenantOrders` carries a durable `TenantId` and composite uniqueness contract
- the API now stamps `SESSION_CONTEXT` through [TenantSessionContextMiddleware](../../src/apps/SqlAcademy.Api/Infrastructure/TenantSessionContextMiddleware.cs) and [SqlSessionContextApplier](../../src/libs/SqlAcademy.Persistence/MultiTenancy/SqlSessionContextApplier.cs)
- the repo now has focused proof harnesses under [TenantOrdersEndpointTests](../../tests/SqlAcademy.IntegrationTests/Api/TenantOrdersEndpointTests.cs) and [RowLevelSecuritySampleTests](../../tests/SqlAcademy.PerformanceTests/RowLevelSecuritySampleTests.cs), including a blocked cross-tenant EF write path

The remaining gaps are also concrete:

- most academy tables still model users and application ownership, not a durable shared-database `TenantId` boundary
- the serving runtime still uses local-learning privilege defaults that should be narrowed before broader RLS rollout means much
- background jobs and maintenance workflows still need explicit review before you generalize the pattern beyond the sample

Because of that, RLS still belongs in the optional DBA or DBRE security module as a follow-on lesson and guided lab, not in the core numbered route.

## Design Questions To Answer Before You Enable RLS

Ask these questions explicitly:

1. Which tables are actually tenant-scoped?
2. Is `TenantId` immutable, indexed, and part of the uniqueness rules that matter?
3. Which layer sets `SESSION_CONTEXT(N'TenantId')`, and how is that fact tested?
4. Which service account, migration path, support tool, or reporting workflow needs an explicit bypass?
5. How will you prove both isolation and safe rollback if the policy causes damage?

If one of those answers is vague, the rollout is not ready.

## Operational And Performance Tradeoffs

RLS changes more than access control.

It changes:

- how developers reason about missing rows during debugging
- how support or analytics sessions are granted broader access safely
- how background processing and batch maintenance work are authenticated
- how index strategy and predicate selectivity affect performance

The predicate function should stay simple, deterministic, and close to the access path you actually indexed. Complex predicate logic turns a security boundary into a performance trap.

## Good Practice Sequence

1. Add and backfill a durable tenant key before you attempt policy enforcement.
2. Remove broadly privileged runtime access and make connection ownership explicit.
3. Stamp tenant session context at the connection boundary and prove it in tests.
4. Introduce a narrow predicate function and a security policy on one table first.
5. Validate tenant-allowed, tenant-denied, operator-bypass, and background-job behavior.
6. Document how to disable or roll back the policy safely if production behavior is wrong.

## Practice Route In This Repo

Use this sequence when you want to study the topic honestly here:

1. Revisit [Lesson 04: Schema Design And Migration Safety](04-schema-design-and-migration-safety.md) and ask what schema changes a real `TenantId` rollout would require.
2. Use [Senior 011: Least Privilege And Operational Security Boundaries](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md) to separate runtime privilege, migration authority, and operator access first.
3. Work through [Senior 012: Row-Level Security And Tenant Isolation](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md) to review policy shape, tenant context ownership, and proof obligations.

## Exit Criteria

You are ready to use the Senior 012 pack when you can do all of the following:

- explain why RLS is a follow-on to schema design and least privilege instead of a replacement for them
- name the schema and runtime prerequisites needed before `CREATE SECURITY POLICY` is credible
- distinguish filter predicates from block predicates and describe one operational risk of each
- explain how tenant session context, explicit bypass rules, and proof checks fit into one rollout plan

## Review Questions

1. Why does row-level security belong after schema-design and least-privilege work instead of before it?
2. What is the practical role of `SESSION_CONTEXT` in a SQL Server RLS design?
3. Why is a `TenantId` column not enough by itself to make an RLS rollout safe?
4. What operational difference exists between a filter predicate and a block predicate?
5. Why should an operator or maintenance bypass path be explicit inside the policy design?

## Suggested Answers

1. RLS depends on a trustworthy tenant key, controlled runtime identity, and explicit operator boundaries, so it only helps after those more basic contracts already exist.
2. `SESSION_CONTEXT` carries the per-connection tenant identity that the predicate function compares against each protected row.
3. The column does not prove who sets tenant context, how writes are blocked, how privileged access is limited, or how the policy will be tested and rolled back.
4. A filter predicate hides disallowed rows from reads, while a block predicate rejects writes that would violate the policy.
5. Support, reporting, maintenance, or emergency workflows still need broader access sometimes, and implicit bypass paths are harder to audit and easier to abuse.

## Challenge Questions

1. Describe a production-safe rollout that adds `TenantId` to a hot table before enabling RLS.
2. Defend a decision to reject RLS for a service that still connects as `sa` and does not stamp session context.
3. Design one proof plan that demonstrates both allowed access and denied cross-tenant access without relying on screenshots alone.

## Interview-Style Prompts

1. Explain to an interviewer why row-level security is not a substitute for application authorization.
2. Defend an explicit operator-bypass role to a reviewer who wants the policy to apply blindly to every workflow.

## Model Answer Rubrics

### Challenge Questions

A strong set of challenge answers should include:

- a staged schema rollout before policy attachment
- a clear rejection of over-privileged runtime defaults as an RLS foundation
- concrete proof steps for tenant-allowed, tenant-denied, and maintenance or bypass behavior

### Interview-Style Prompts

A strong spoken answer should include:

- the distinction between app authorization and database enforcement
- the role of tenant context, predicate functions, and explicit bypass ownership
- operational reasoning about debugging, rollback, and performance rather than only security slogans

## Lesson Checkpoint

- name one schema precondition and one runtime precondition for RLS
- explain when a filter predicate is useful and when it can become confusing
- state one reason the core academy tables are not yet ready for the same RLS treatment as the dedicated tenant sample

## Next Step

Move to [Senior 012](../../src/exercises/Senior/012-row-level-security-and-tenant-isolation/README.md) when you can defend when row-level security is a real boundary and when it is only an illusion.