# Lesson 11: Capstones And Interview Readiness

## Focus

This lesson is about synthesis. You should now be able to combine modeling, querying, tuning, application behavior, observability, and operational tradeoffs into one coherent answer.

## Why This Lesson Matters

Capstones and interviews reward a different level of fluency than isolated exercises. It is no longer enough to know one concept at a time. You need to connect them.

Strong senior-level answers usually do three things well:

- clarify the problem before solving it
- defend tradeoffs instead of reciting best practices
- describe validation and failure handling, not only implementation

That is what this final lesson is training.

## Repository Anchors

- [Phase 9: Capstone Projects](../phases/phase-9-capstone-projects.md)
- [Senior 001: Concurrency, Blocking, And Deadlocks](../../src/exercises/Senior/001-concurrency-blocking-and-deadlocks/README.md)
- [Senior 002: Optimistic Concurrency And Staged Trade Ingestion](../../src/exercises/Senior/002-efcore-concurrency-and-bulk-ingestion/README.md)
- [Senior 004: Posts API Latency And Observability Triage](../../src/exercises/Senior/004-posts-api-latency-and-observability-triage/README.md)
- [Review-Style Practice](review-style-practice.md)
- [Repository improvement suggestions](repository-improvement-suggestions.md)
- [Follow-Up Exercises By Level](follow-up-exercises-by-level.md)

## What A Capstone Should Prove

A good capstone does not only prove that you can code. It should prove that you can:

- define requirements precisely
- choose a schema and query strategy intentionally
- reason about concurrency and performance
- connect SQL behavior to application behavior
- ship the change safely
- explain how you would validate and observe it in production

If your capstone only shows code output and not design reasoning, it is incomplete.

## A Strong Capstone Workflow

### 1. Clarify The Problem

Write down:

- functional requirements
- performance targets
- consistency expectations
- operational constraints
- rollback expectations

Example:

- return a paged report of recent trades
- page 1 should complete within an acceptable latency budget
- results must be deterministic across repeated calls
- release must not block the existing OLTP workload dangerously

### 2. Model The Data And Contracts

Ask:

- which tables and relationships are already sufficient?
- what new schema, if any, is required?
- what invariants belong in the database?
- what response contract does the API need?

### 3. Choose Query And Access Strategy

Ask:

- is EF Core expressive enough here?
- is Dapper or explicit SQL clearer for this read path?
- what indexes or sort guarantees are required?
- how will pagination stay stable?

### 4. Plan Operational Delivery

Ask:

- is the schema change additive?
- what must be deployed first?
- what smoke tests prove success?
- what would I monitor after release?

### 5. Validate And Document

Capture:

- correctness validation
- performance evidence
- observability plan
- rejected alternatives and why they lost

That last step matters. Senior work is often about explaining why not just as much as explaining why.

## Example Capstone Themes In This Repository

### Reporting Slice With Performance Constraints

Build or redesign a query endpoint that joins multiple tables, paginates deterministically, and meets a clear latency target.

What it tests:

- joins and aggregation
- window functions or pagination
- indexing and plan analysis
- API contract design

### Safe Schema Evolution Scenario

Design and ship an additive schema change with backfill and compatibility requirements.

What it tests:

- schema design
- migration safety
- rollout and rollback planning
- observability during release

### Incident Debugging Scenario

Analyze a staged problem that combines contention, query shape, and API behavior.

What it tests:

- concurrency reasoning
- performance hypothesis formation
- telemetry usage
- prioritization under pressure

## How To Structure Interview Answers

When asked a design or debugging question, avoid jumping to a tool or a one-line “best practice.” Use a structured answer.

### Clarify

State what you need to know first:

- expected workload
- consistency requirements
- read versus write ratio
- latency target
- rollback or uptime constraints

### Propose

Describe the core design:

- schema or query strategy
- data-access approach
- indexing or concurrency posture
- API or service shape

### Defend

Explain the tradeoffs and rejected alternatives.

### Validate

Explain how you would test, measure, and observe the solution.

### Risk Review

Name the most likely failure modes and how you would detect or mitigate them.

This structure makes answers feel senior because it shows judgment, not memorization.

## What Interviewers Usually Notice

They often notice these weaknesses quickly:

- treating every problem as a query-writing exercise only
- ignoring operational rollout and monitoring
- offering indexes without workload reasoning
- proposing retries without understanding deadlocks or idempotency
- speaking confidently about performance without evidence plans or measurements

They also notice these strengths quickly:

- asking the right clarifying questions
- naming tradeoffs explicitly
- distinguishing correctness from performance from operability
- explaining how the solution would be validated

## Self-Review Rubric For Final Work

Before you call a capstone complete, review it against these questions:

1. Did I state the problem and assumptions clearly?
2. Can I explain the schema and query choices in plain language?
3. Did I account for concurrency or data integrity risks?
4. Did I define how the change would be tested?
5. Did I define what I would monitor after release?
6. Did I describe at least one credible failure mode?
7. Did I record rejected alternatives and why they were rejected?

If several answers are no, the solution is probably still too shallow.

## Capstone Scoring Rubric

Use this grid when you want a faster and more standardized review than a freeform “this feels senior enough” judgment.

| Dimension | Weak | Acceptable | Strong |
| --- | --- | --- | --- |
| problem framing | jumps to implementation without clarifying workload, constraints, or assumptions | names the main requirements and a few key constraints | frames the problem precisely, exposes hidden constraints, and makes assumptions explicit |
| schema and query design | proposes tables, indexes, or SQL shape without a clear reason | makes defensible schema and query choices tied to the main requirement | ties schema, query shape, pagination, and indexing decisions directly to workload and correctness |
| application and data-access fit | picks EF Core, Dapper, or endpoints mostly by preference | chooses a reasonable application boundary and data-access style | explains why the selected abstraction keeps SQL behavior understandable and maintainable |
| validation and evidence | says the solution would be tested but does not define evidence | names at least one correctness and one runtime validation surface | defines correctness, performance, and observability evidence with the right test or telemetry surface for each claim |
| release and operability | treats deployment as a later concern | names a basic rollout and watch plan | defines compatibility posture, smoke checks, telemetry gates, and rollback or roll-forward triggers clearly |
| tradeoffs and failure modes | describes one best practice as if context does not matter | names at least one downside or likely failure mode | compares alternatives honestly, names credible failure modes, and explains why the chosen downside is acceptable |

If several dimensions stay in the weak column, the capstone is not ready for final defense yet.

## Design-Defense Template

Use this outline when you want a reusable answer shape for capstones, interview prompts, or senior review sessions.

### 1. Clarify

- problem statement:
- workload and latency expectations:
- consistency or correctness constraints:
- rollout or uptime constraints:
- assumptions still needing confirmation:

### 2. Propose

- schema or contract shape:
- query or indexing strategy:
- application boundary and data-access approach:
- why this is the simplest viable design:

### 3. Defend

- main tradeoff accepted:
- strongest rejected alternative:
- why the chosen downside is acceptable here:

### 4. Validate

- correctness checks:
- performance evidence:
- telemetry or observability plan:
- post-release useful-work smoke test:

### 5. Risk Review

- most likely failure mode:
- earliest signal that would expose it:
- containment move if it happens during release:

## Common Failure Modes

### Presenting Implementation Without Decision Logic

Senior-level work requires justification, not just output.

### Ignoring Non-Functional Requirements

A functionally correct system can still fail latency, stability, or operability requirements.

### Acting As If There Is One Universal Best Practice

Tradeoffs are contextual. Strong answers make the context explicit.

### Forgetting The Release Story

If you cannot describe how the change gets shipped safely, the design is incomplete.

## A Good Practice Sequence

1. Pick one capstone scenario from [Phase 9: Capstone Projects](../phases/phase-9-capstone-projects.md).
2. Write assumptions and success criteria before building anything.
3. Solve it with schema, query, application, and operations all in view.
4. Prepare a short design defense as if explaining it in an interview.
5. Review your own answer with the rubric above.

## Exit Criteria

You have completed the core learning path when you can do all of the following:

- defend your chosen schema, query, and rollout under scrutiny
- explain what you would test before release and monitor after release
- name likely failure modes before they happen
- discuss rejected alternatives without hand-waving
- give a structured interview answer that connects SQL, application behavior, and operations

## Review Questions

1. Why do strong capstone or interview answers start with clarification instead of immediate implementation?
2. What makes a tradeoff discussion stronger than a list of best practices?
3. Why should validation and observability be part of the solution, not an afterthought?
4. What does a credible failure-mode discussion show about your engineering judgment?
5. Why is the release story part of the design rather than a separate operational detail?

## Suggested Answers

1. Clarification aligns the solution to real requirements and constraints, which prevents technically polished answers to the wrong problem.
2. A tradeoff discussion shows that you understand context, alternatives, and consequences, rather than reciting rules as if they apply universally.
3. A solution is incomplete if you cannot prove it works correctly, performs acceptably, and can be monitored safely after release.
4. It shows that you can anticipate likely problems before they happen and plan detection and mitigation rather than reacting blindly.
5. Deployment, compatibility, rollback, and monitoring constraints shape what is actually safe to build and ship, so they are part of the design itself.

## Challenge Questions

1. Pick one capstone theme in this repository and write the first five clarifying questions you would ask before proposing a design.
2. Prepare a short defense of a design choice that you know has at least one serious tradeoff, and explain why you would still choose it.
3. Explain how you would turn one of the senior exercises into an interview answer that sounds like engineering judgment instead of memorized theory.

## Interview-Style Prompts

1. Explain to an interviewer how you would structure a senior-level answer to a data-platform design problem from first clarification through release plan.
2. Defend the statement “a design is incomplete until it includes how success, failure, and rollout will be measured.”

## Model Answer Rubrics

### Challenge Questions

A strong set of challenge answers should include:

- clarifying questions that expose workload, consistency, latency, and rollout constraints before design choice
- at least one defended decision with a named downside that is still acceptable in context
- a conversion of exercise work into design reasoning, validation, and risk discussion rather than only implementation detail

### Interview-Style Prompts

A strong spoken answer should include:

- a stable structure such as clarify, propose, defend, validate, and risk review
- the idea that measurement and release planning are part of the design answer itself
- one credible failure mode together with the signal that would detect it

## Lesson Checkpoint

- structure one senior answer as clarify, propose, defend, validate, and risk review
- name one likely failure mode for your capstone and how you would detect it
- defend one rejected alternative without hiding the tradeoff

## Next Step

Use [Review-Style Practice](review-style-practice.md), [Cumulative Review](cumulative-review.md), [Follow-Up Exercises By Level](follow-up-exercises-by-level.md), and [Repository improvement suggestions](repository-improvement-suggestions.md) to extend the path after the scaffolded lesson sequence.