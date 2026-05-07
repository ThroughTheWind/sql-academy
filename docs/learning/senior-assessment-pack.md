# Senior Assessment Pack

Use this pack after Lessons 08 through 11. It checks whether you can connect release safety, application integration, observability, and senior-level design defense.

## Covers

- [Lesson 08: Operational Engineering And Release Safety](08-operational-engineering-and-release-safety.md)
- [Lesson 09: EF Core, Dapper, And Query Shape](09-ef-core-dapper-and-query-shape.md)
- [Lesson 10: Observability, Testing, And Performance Engineering](10-observability-testing-and-performance-engineering.md)
- [Lesson 11: Capstones And Interview Readiness](11-capstones-and-interview-readiness.md)
- [Senior 002](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
- [Senior 003](../../src/exercises/Senior/003-transactional-outbox-and-delivery-consistency/README.md)
- [Senior 004](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)
- [Senior 005](../../src/exercises/Senior/005-efcore-n-plus-one-and-generated-sql-investigation/README.md)

## How To Use This Pack

1. Answer as if a reviewer will ask you to justify the tradeoffs, not just the final decision.
2. For scenarios, describe what you would measure, not only what you would build.
3. Use this pack as written practice for interview-style explanations or release reviews.

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

## Ready To Finish The Core Path When

- you can connect SQL, application behavior, observability, and rollout safety in one explanation
- you can defend a design choice and still name its serious tradeoffs
- you can choose the right validation surface for a claim before making the claim loudly
- you can describe both how a change is built and how it is safely released and observed