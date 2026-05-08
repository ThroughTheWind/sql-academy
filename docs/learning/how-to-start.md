# How To Start

This guide is for a learner who wants a clear first path instead of browsing the repository tree.

It is written for working backend and application engineers who can already navigate a local development environment and want a more structured SQL Server route through the repository.

It is not yet the right first stop if you are learning your first developer workflow or your first SQL syntax at the same time.

## Before You Commit To This Route

- use this route when you are comfortable running Docker, opening a SQL client, and reading application code or test assets
- slow down in Lesson 01, the [Learning Glossary](glossary.md), and [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md) if SQL syntax still feels unfamiliar
- do not treat the sample week-one pace below as a deadline if the query basics are not yet predictable

## Step 1: Get The Platform Running

1. Review [../../.env](../../.env) before first startup. Keep the defaults unless a port or password conflicts with your machine, and use [../../.env.example](../../.env.example) as the reference for supported settings.
2. Restore tools with `dotnet tool restore`.
3. Start the local platform with `docker compose up --build`.
4. Wait for the API to report ready at `http://localhost:8080/health/ready`.
5. Connect to `LearningDb` with a SQL client, inspect the `academy` schema, and run `SELECT COUNT(*) FROM academy.Users`.
6. Treat Prometheus and Grafana as optional on day one. Open them after the API and database already feel predictable.

Do not start with EF Core code or performance tuning before the local stack feels predictable.

If the setup itself is new territory, stop after the first successful `SELECT COUNT(*) FROM academy.Users` and make that workflow boring before you add more moving parts.

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

## Step 4: Stay On The Main Route By Default

Use [Curriculum Map](curriculum-map.md) as the main sequence.

Only skip ahead if you already know your gap and you plan to rejoin the main route later.

If you do need a targeted start, use the [Optional On-Ramps](curriculum-map.md#optional-on-ramps) instead of inventing a new order from the repo tree.

## Step 5: Use Explicit Exit Criteria

Do not say you "covered" a lesson unless you can do all of the following:

- explain the main tradeoff in your own words
- complete the linked exercise without relying on the solution file immediately
- describe one failure mode or debugging scenario related to the topic
- point to the relevant code, SQL, or infrastructure asset in the repository

After every few lessons, use [Cumulative Review](cumulative-review.md) to check whether the topics still connect when the questions are mixed together instead of grouped by lesson.

If you want narrower checkpoints, use the level-scoped packs after each stage:

- [Beginner Assessment Pack](beginner-assessment-pack.md) after Lessons 01 and 02
- [Intermediate Assessment Pack](intermediate-assessment-pack.md) after Lessons 03 and 04
- [Advanced Assessment Pack](advanced-assessment-pack.md) after Lessons 05 through 07
- [Senior Assessment Pack](senior-assessment-pack.md) after Lessons 08 through 11

## Sample First Week Plan

This pacing is realistic for an experienced engineer ramping into the repository. It is not intended as a speed target for someone still learning the basics.

1. Day 1: complete local setup and inspect seed data
2. Day 2: finish Lesson 01 and [Beginner 000](../../src/exercises/Beginner/000-sql-fundamentals-and-safe-changes/README.md)
3. Day 3: finish Lesson 02 and [Beginner 001](../../src/exercises/Beginner/001-joins-and-aggregations/README.md)
4. Day 4: finish Lesson 03 and practice stable pagination
5. Day 5: read Lesson 04 and review migration safety patterns
6. Day 6: read Lesson 05 and rehearse blocking versus deadlocking
7. Day 7: summarize what you still cannot explain clearly and choose your next lesson based on that gap

## Step 6: Add Harder Practice Deliberately

Each lesson now ends with:

- review questions and suggested answers for fast self-checking
- challenge questions for harder solo practice
- interview-style prompts for spoken tradeoff defense
- model answer rubrics that tell you what a strong response should include without giving you a fully solved answer

Use the challenge questions when the review questions feel easy. Use the interview prompts when you want to test whether you can explain the topic clearly without hiding behind code.