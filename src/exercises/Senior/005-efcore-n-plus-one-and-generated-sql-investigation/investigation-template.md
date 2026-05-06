# Investigation Template

## Current Safe Shape

- what is the base rowset?
- where are filters applied?
- where does projection happen?
- where does read-only behavior stay explicit?

## Generated SQL Evidence

- how did you capture or inspect the generated SQL?
- what proves the current path is one-query or otherwise avoids classic N+1 behavior?

## Disposable N+1 Experiment

- what temporary change or scratch query created the risk?
- what repeated SQL, extra round trips, or materialization pattern exposed the problem?

## Why The Current Path Is Safer

- what exact part of the current query shape prevents the regression?
- what would have to change before the path became risky?

## Regression Guard

- what test, logging workflow, or code-review rule would you keep after the investigation?
- what signal would tell you the safe shape regressed later?