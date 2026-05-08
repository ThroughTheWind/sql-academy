# Least Privilege And Security Boundary Runbook

Use this as a printable checklist when a runtime, migration, or operator workflow feels too privileged and you need to name the exact boundary that must change before the environment is defensible.

This runbook is a companion to [Senior 011: Least Privilege And Operational Security Boundaries](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md), [Lesson 04: Schema Design And Migration Safety](../learning/04-schema-design-and-migration-safety.md), and [Operations Track](README.md).

It is not a replacement for the investigation pack. It is the compressed checklist for separating local-learning convenience from the minimum least-privilege and separation-of-duties rules needed in a shared environment.

## First Boundary Check

- name the one privileged path you are reviewing first: runtime access, migration authority, or operator-admin access
- decide whether the current behavior is acceptable only for local learning or already unsafe for shared use
- keep one concrete privilege boundary in scope before proposing broad security rewrites

## Runtime Versus Migration Authority

- confirm whether runtime processes still connect as `sa` instead of a narrower application identity
- check whether `InitializeLearningDatabaseAsync` still allows the serving runtime to apply privileged schema or seed work
- treat runtime access and migration authority as separate boundaries even when they currently share the same convenience credential

## Admin Surface Review

- identify which operator-admin surface still depends on convenience defaults, such as Grafana admin credentials
- require explicit ownership for credential storage, rotation, and use instead of assuming environment variables are enough by themselves
- do not call the boundary safe if the next operator still cannot tell who owns privileged access

## Promotion Gate

- stop promoting the same stack shape into shared use while runtime, migration, and admin privileges remain collapsed together
- require one explicit reviewed step for schema application instead of silent privileged startup behavior
- require one concrete proof that the runtime no longer relies on an admin-level default before calling the environment defensible

## Follow-Up Validation

- record which privileged default was removed, what replaced it, and who now owns the remaining admin path
- rerun the same boundary review after the change instead of assuming the privilege split happened because the configuration file changed
- keep [Senior 011: Least Privilege And Operational Security Boundaries](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md) nearby when the question is about least privilege rather than only deployment convenience

## Drill Links

- [Senior 011: Least Privilege And Operational Security Boundaries](../../src/exercises/Senior/011-least-privilege-and-operational-security-boundaries/README.md)
- [Lesson 04: Schema Design And Migration Safety](../learning/04-schema-design-and-migration-safety.md)
- [Operations Track](README.md)
