# Senior Assessment Pack

Use this pack after Lessons 08 through 11. It checks whether you can connect release safety, application integration, observability, and senior-level design defense.

For the full Lesson 09 practice ladder, use [Advanced 005](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md) as the read-path entry point, [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md) as the generated-SQL and N+1 proof surface, and [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md) as the later write-side follow-up.

## Covers

- [Lesson 08: Operational Engineering And Release Safety](08-operational-engineering-and-release-safety.md)
- [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md)
- [Lesson 11: Capstones And Interview Readiness](11-capstones-and-interview-readiness.md)
- [Advanced 005](../../src/exercises/Advanced/005-efcore-dapper-read-paths-and-query-contracts/README.md)
- [Senior 002: Optimistic Concurrency And Staged Trade Ingestion](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
- [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)
- [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md)
- [Senior 006](../../src/exercises/Senior/006-release-readiness-and-rollback-gates/README.md)

## How To Use This Pack

1. Answer as if a reviewer will ask you to justify the tradeoffs, not just the final decision.
2. For scenarios, describe what you would measure, not only what you would build.
3. Use this pack as written practice for interview-style explanations or release reviews.

## Scoring Guide

Use this guide when you want to score a written answer or spoken defense more consistently.

| Dimension | Needs Work | Solid | Strong |
| --- | --- | --- | --- |
| clarification | jumps into tools or fixes immediately | names the main requirement and a few important constraints | exposes workload, consistency, rollout, and observability constraints before proposing a design |
| proposal quality | gives a vague or tool-first answer | proposes a coherent design or debugging plan | proposes a design that stays tied to SQL shape, application behavior, and release reality |
| tradeoff defense | argues from slogans or defaults | names at least one real tradeoff | compares alternatives and defends why the chosen downside is acceptable |
| evidence plan | says “test it” or “monitor it” vaguely | names at least one relevant test or telemetry surface | chooses the right correctness, performance, and observability evidence for the exact claim |
| risk and release posture | ignores failure modes or rollout order | names a plausible risk and a basic rollout plan | names concrete stop conditions, failure signals, and rollback or roll-forward gates |

## Senior Design-Defense Template

Use this when answering the scenario prompts or rehearsing a spoken capstone defense.

1. Clarify: what workload, correctness, latency, and rollout constraints matter most?
2. Propose: what schema, query, API, or operational shape best fits those constraints?
3. Defend: what alternative did you reject, and why is your chosen downside acceptable?
4. Validate: what test, benchmark, trace, metric, or smoke check would prove the claim?
5. Risk Review: what fails first, how would you detect it, and what would you do next?

## Short Questions

1. Why is deployment safety part of engineering design rather than a separate operations detail?
2. Why is a running service not automatically a ready service?
3. Why is EF Core versus Dapper usually the wrong first framing for a data-access decision?
4. What is the practical shape of an N+1 problem?
5. Why do logs, metrics, and traces answer different questions?
6. Why should telemetry design happen before the incident?
7. Why do strong capstone answers start with clarification rather than implementation?
8. Why is a tradeoff discussion stronger than a list of best practices?

## Scenarios

1. A release technically succeeds, but readiness flaps and latency spikes in the first few minutes. What signals should you inspect immediately?
2. A team wants to replace EF Core with Dapper because one endpoint is slow. What higher-quality question should you ask first?
3. A benchmark shows one implementation is faster, but production traces still show poor request latency. What mistake might the team be making in its reasoning?
4. You are asked to defend a capstone design under scrutiny. What structure would you use to keep the answer senior-level instead of improvised?

## Deeper Follow-On Scenarios

Use these after the base scenarios if you want a second round that mixes more than one discipline.

5. A release keeps `/health/ready` green, but outbox backlog and downstream delivery lag begin growing after the enforcing migration step. What boundary do you inspect first, and what concrete gate would make you stop the window?
6. A post-deploy incident shows one endpoint with higher latency and rising lock waits. How would you separate query-shape regression from concurrency amplification before choosing containment?

## Answer Key

1. Rollout, compatibility, monitoring, and rollback constraints influence what is actually safe to build and ship.
2. Liveness does not prove dependency readiness, initialization success, or useful service behavior.
3. The better first question is about query and update shape, because either tool can be a good or poor fit depending on the workload.
4. One query loads a list and then extra queries fire per item for related data, multiplying round trips.
5. Logs capture detailed events, metrics show trends, and traces show request timing across components.
6. Missing telemetry during the incident means the system is already under-observed when evidence is needed most.
7. Clarification aligns the design to real constraints and avoids solving the wrong problem well.
8. Tradeoff discussion shows context, alternatives, and consequences rather than memorized slogans.
9. Inspect readiness endpoints, latency, error rate, restart behavior, and traces or metrics pointing to dependency or startup contention.
10. Ask what SQL shape, access pattern, or plan behavior is slow now before changing libraries.
11. They may be treating an isolated benchmark result as a substitute for end-to-end evidence from the real running system.
12. Use a structure such as clarify, propose, defend, validate, and risk review.
13. Check whether useful work is still completing, inspect the outbox or delivery boundary directly, and stop the window when the release is green only at the container level but no longer safe at the business-work level.
14. Keep one healthy control path nearby, inspect lock or wait evidence together with the slow query boundary, and choose the smallest safe containment that narrows the problem before schema or library changes.

## Ready To Finish The Core Path When

- you can connect SQL, application behavior, observability, and rollout safety in one explanation
- you can defend a design choice and still name its serious tradeoffs
- you can choose the right validation surface for a claim before making the claim loudly
- you can describe both how a change is built and how it is safely released and observed

## Final Defense Checklist

- [ ] the answer starts with clarification instead of a tool choice
- [ ] the design is defended with tradeoffs rather than slogans
- [ ] the evidence plan includes at least one correctness check and one runtime or release check
- [ ] the response names one credible failure mode and one early signal that would expose it
- [ ] the rollout story is concrete enough that another engineer could challenge it usefully