# Lesson 10: Observability, Testing, And Performance Engineering

## Focus

Learn how to validate behavior, compare implementations, and observe the running system instead of relying on intuition alone. This lesson turns “I think it works” into a disciplined feedback loop.

## Before You Start

- be comfortable with the earlier lessons on query shape, measurement, release observation, and application data-access tradeoffs
- keep [Learning Glossary](glossary.md) open if terms such as trace, benchmark, integration test, or telemetry are still slow
- expect to map each engineering claim to the narrowest proof surface instead of reaching for every testing layer at once

## Suggested Time Budget

- 75 to 120 minutes to read the lesson, inspect the existing tests and telemetry wiring, and map one claim to one validation surface
- longer if observability and test taxonomy are newer than query-writing itself

## If You Get Stuck Early

- write down the claim first, then choose the narrowest surface that could prove or disprove it
- do not compare performance until you are sure both paths do equivalent work
- use the existing unit, integration, performance, and benchmark surfaces as examples before you invent a new validation layer

## Why This Lesson Matters

Engineering maturity is not only about writing code or queries. It is also about proving claims such as:

- the pagination logic is correct
- the endpoint still returns the expected contract
- the SQL path did not regress under a workload change
- the running service is healthy after deployment

Observability and testing are how you defend those claims with evidence.

## Repository Anchors

- [Incident Triage Runbook](../operations/incident-triage-runbook.md)
- [Unit tests](../../tests/SqlAcademy.UnitTests/Paging/PagedRequestTests.cs)
- [Integration tests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs)
- [Performance tests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs)
- [Benchmarks](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs)

## The Core Feedback Loop

Use this loop repeatedly:

1. state the behavior or performance claim
2. choose the narrowest validation surface that can prove or disprove it
3. collect evidence
4. change the system
5. collect the same evidence again

That loop is the bridge between development, tuning, and operations.

## Logs, Metrics, And Traces Serve Different Questions

### Logs

Logs are best for discrete events and detailed context.

Use them when you need to know:

- what happened
- with which parameters or identifiers
- in what sequence
- with what error details

### Metrics

Metrics are best for trend detection and alerting.

Use them when you need to know:

- is latency climbing?
- is error rate rising?
- is throughput dropping?
- is resource pressure increasing?

### Traces

Traces are best for following a request or job across components and timings.

Use them when you need to know:

- where did the time go in this request?
- which downstream dependency was slow?
- how does database work relate to application work?

These tools are complementary. Treating one as a substitute for all three leads to blind spots.

## What The Repository Already Instruments

In [Observability setup](../../src/libs/SqlAcademy.Observability/OpenTelemetryServiceCollectionExtensions.cs), the repository wires in:

- ASP.NET Core instrumentation
- HTTP client instrumentation
- runtime metrics
- optional OTLP export

In [API startup and metrics endpoints](../../src/apps/SqlAcademy.Api/Program.cs), the app exposes:

- request logging
- Prometheus metrics via `/metrics`
- liveness and readiness endpoints

That means the lesson is not theoretical. The repo already gives you a real telemetry surface to inspect.

## Unit Tests: Prove Small Contracts Fast

The [Unit tests](../../tests/SqlAcademy.UnitTests/Paging/PagedRequestTests.cs) show a good unit-test posture:

- narrow scope
- no external infrastructure requirement
- explicit contract checks

Those tests prove pagination normalization behavior such as page clamping and skip calculation.

What a unit test should answer:

- does this small piece of logic behave correctly for representative inputs?

What it should not pretend to answer:

- is the API endpoint wired correctly?
- is the database query returning the right rows under real SQL Server behavior?

## Integration Tests: Prove Collaborating Components Work Together

The [Integration tests](../../tests/SqlAcademy.IntegrationTests/Api/PostsEndpointTests.cs) exercise the API endpoint against a real test setup and validate contract-level behavior.

Integration tests are appropriate for questions like:

- does the endpoint return the expected payload shape?
- do routing, serialization, persistence, and the database agree?
- does seeded data produce the expected high-level behavior?

They are slower and broader than unit tests, so use them for boundary confidence, not for every small branch.

## Performance Tests: Compare Behaviors Under A Defined Scenario

The [Performance tests](../../tests/SqlAcademy.PerformanceTests/QueryPerformanceComparisonTests.cs) compare EF Core and Dapper for the same trade-page scenario.

Important lesson:

Performance tests should compare equivalent work. If the two paths do different work, the comparison becomes noise.

Performance tests are useful for:

- comparing alternative query shapes
- checking that two implementations produce the same business result
- catching obvious regressions in relative cost

They are less useful when:

- the environment is highly unstable
- inputs are not controlled
- the test is interpreted as a universal law instead of one measured scenario

## Benchmarks: Isolate A Narrow Performance Question

The [Benchmarks](../../tests/SqlAcademy.Benchmarks/TrackingModeBenchmarks.cs) isolate a specific concern: tracking versus no tracking.

That is good benchmark design because it asks one narrow question. Benchmarks are strongest when they:

- isolate one variable
- keep workload shape stable
- run enough iterations to reduce noise
- avoid hidden environmental differences as much as possible

What benchmarks do not do well:

- simulate full production traffic
- prove end-to-end user experience
- replace integration or performance tests

## Performance Engineering Is More Than “Make It Faster”

Real performance engineering includes:

- defining what good enough means
- measuring current behavior
- identifying the dominant cost driver
- choosing the cheapest change that addresses that driver
- validating that you did not break correctness or operability

That is why testing and observability belong in the same lesson.

## Choosing The Right Validation Surface

Use this rough guide:

- unit test for deterministic in-process logic
- integration test for multi-component correctness
- performance test for comparative scenario behavior
- benchmark for isolated micro-performance questions
- telemetry for live-system observation and release validation

The error many teams make is using only one of these for everything.

## What Good Regression Thinking Looks Like

When an endpoint regresses, a strong engineer asks:

1. did the business result change?
2. did latency, error rate, or reads change?
3. did query shape change?
4. did observability become less informative?
5. which validation layer should have caught this earlier?

That is performance engineering with accountability, not just tuning.

## Common Failure Modes

### Writing Tests That Assert Very Little

A test that only checks “not null” for a complex contract may not prove anything meaningful.

### Comparing Performance Without Equivalent Work

That creates misleading narratives about tools rather than evidence about implementations.

### Using Benchmarks To Replace Observability

Microbenchmarks do not tell you what a real deployed service is experiencing.

### Shipping Without Telemetry You Intend To Rely On

If you expect to debug production with metrics and traces, they must exist before the incident.

## A Good Practice Sequence

1. Read the unit tests and say exactly what they prove.
2. Read the integration test and identify the system boundary it exercises.
3. Read the performance test and explain why the compared outputs are equivalent.
4. Read the benchmark and identify the single variable it isolates.
5. Open the observability wiring and name the first signal you would inspect after a regression.

## Exit Criteria

You are ready for Lesson 11 when you can do all of the following:

- choose the right test or telemetry surface for a given claim
- explain what the repository’s existing tests do and do not prove
- compare two implementations with a fair performance question
- identify a useful metric or trace for production regression detection
- connect observability and testing into one evidence-driven engineering loop

## Review Questions

1. Why do logs, metrics, and traces answer different kinds of questions?
2. What kind of claim should a unit test prove compared with an integration test?
3. Why must a performance comparison test ensure both implementations do equivalent work?
4. What makes a benchmark useful rather than misleading?
5. Why should telemetry design happen before an incident rather than during one?

## Suggested Answers

1. Logs capture detailed events, metrics show trends and rates, and traces show the timing path of requests across components, so each surface answers a different debugging need.
2. A unit test should prove a narrow in-process contract, while an integration test should prove that multiple real components work together across a boundary.
3. If the compared implementations do different work, the timing difference does not tell you anything trustworthy about the tools or the query shapes.
4. A useful benchmark isolates one variable, keeps the workload stable, and answers a narrow question instead of pretending to model the full production system.
5. Because telemetry only helps if it already exists when the regression happens; designing it during the incident means the system is already under-observed.

## Challenge Questions

1. Choose one claim about this system and explain which validation layer should prove it first: unit test, integration test, performance test, benchmark, or telemetry.
2. Design a regression investigation path for a posts endpoint that is still correct but slower than last week.
3. Explain why a benchmark that looks precise can still be misleading for engineering decisions.

## Interview-Style Prompts

1. Explain to an interviewer how you separate correctness testing from performance validation and live-system observability.
2. Defend the statement “if you cannot say what evidence would prove the regression, you are not ready to fix it yet.”

## Model Answer Rubrics

### Challenge Questions

A strong set of challenge answers should include:

- a deliberate choice of validation surface matched to the claim being tested
- one investigation path that separates correctness, performance, and runtime observation instead of collapsing them together
- skepticism toward benchmark precision when workload realism or equivalence is weak

### Interview-Style Prompts

A strong spoken answer should include:

- a clear boundary between unit, integration, performance, benchmark, and telemetry evidence
- the idea that fixes should be driven by evidence plans rather than gut feel
- one explanation of how a regression can be real even when a narrow test still passes

## Lesson Checkpoint

- pick the right validation surface for one concrete claim in this repository
- name the first log, metric, and trace you would inspect for a posts endpoint regression
- explain why a benchmark result cannot replace live telemetry

## Next Lesson

Move to [Lesson 11](11-capstones-and-interview-readiness.md) once you can validate and observe the system instead of only implementing it.