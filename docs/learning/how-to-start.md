# How To Start

This guide is for a learner who wants a clear first path instead of browsing the repository tree.

## Step 1: Get The Platform Running

1. Restore tools with `dotnet tool restore`.
2. Start the local platform with `docker compose up --build`.
3. Confirm the API, Prometheus, and Grafana endpoints are reachable.
4. Connect to `LearningDb` with a SQL client and inspect the `academy` schema.

Do not start with EF Core code or performance tuning before the local stack feels predictable.

## Step 2: Learn The Data Model First

Read these before solving exercises:

1. [Phase 0: Local Setup](../phases/phase-0-local-setup.md)
2. [Phase 1: SQL Fundamentals](../phases/phase-1-sql-fundamentals.md)
3. [Lesson 01: SQL Fundamentals](01-sql-fundamentals.md)

Your first goal is to understand the six seeded tables and the relationships between them.

## Step 3: Use The Study Loop

For every lesson or exercise, follow the same loop:

1. Read the lesson doc and write down the query or system behaviors you expect.
2. Run the starter SQL without modifying it.
3. Run the broken SQL and explain exactly why it fails or misbehaves.
4. Attempt a fix without opening the optional solution.
5. Verify the result against the expected outcomes.
6. Compare your answer with the optional solution only after you have a clear opinion.
7. Record one thing you learned and one thing you still find uncertain.

## Step 4: Choose Your Initial Path

| If You Want To Learn | Start With | Then Move To |
| --- | --- | --- |
| SQL basics | [Lesson 01](01-sql-fundamentals.md) | [Lesson 02](02-joins-and-aggregations.md) |
| Analytical querying | [Lesson 02](02-joins-and-aggregations.md) | [Lesson 03](03-window-functions-and-intermediate-querying.md) |
| Concurrency and real production failures | [Lesson 05](05-transactions-blocking-and-deadlocks.md) | [Lesson 06](06-indexing-execution-plans-and-parameter-sensitivity.md) |
| Application integration | [Lesson 09](09-ef-core-dapper-and-query-shape.md) | [Lesson 10](10-observability-testing-and-performance-engineering.md) |

## Step 5: Use Explicit Exit Criteria

Do not say you "covered" a lesson unless you can do all of the following:

- explain the main tradeoff in your own words
- complete the linked exercise without relying on the solution file immediately
- describe one failure mode or debugging scenario related to the topic
- point to the relevant code, SQL, or infrastructure asset in the repository

After every few lessons, use [Cumulative Review](cumulative-review.md) to check whether the topics still connect when the questions are mixed together instead of grouped by lesson.

## First Week Plan

1. Day 1: complete local setup and inspect seed data
2. Day 2: finish Lesson 01 and Beginner 001
3. Day 3: finish Lesson 02 and review your joins carefully
4. Day 4: finish Lesson 03 and practice stable pagination
5. Day 5: read Lesson 04 and review migration safety patterns
6. Day 6: read Lesson 05 and rehearse blocking versus deadlocking
7. Day 7: summarize what you still cannot explain clearly and choose your next lesson based on that gap

## Step 6: Add Harder Practice Deliberately

Each lesson now ends with:

- review questions and suggested answers for fast self-checking
- challenge questions for harder solo practice
- interview-style prompts for spoken tradeoff defense

Use the challenge questions when the review questions feel easy. Use the interview prompts when you want to test whether you can explain the topic clearly without hiding behind code.