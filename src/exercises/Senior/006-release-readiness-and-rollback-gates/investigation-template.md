# Investigation Template

## Go Or No-Go

- is the release ready to start, or should it be blocked?
- which specific migration or compatibility facts drove that decision?

## Preflight Gates

- which schema gate must pass before the enforcement step begins?
- which readiness or telemetry gate must pass before the window opens?
- what result would stop the release immediately?

## First Five Minutes Watch Plan

- which health endpoint do you check first?
- which metric, log, or trace signal would tell you the release is regressing even if readiness stays green?
- which one smoke test proves useful work rather than only process startup?

## Rollback And Roll-Forward Gates

- what would force an immediate rollback attempt?
- what would make controlled roll-forward safer than rollback?
- what compatibility or data-risk fact matters most in that decision?

## Post-Release Validation

- which check proves the application stayed compatible with the stricter schema?
- which check proves the migration did not only succeed technically but also preserve useful behavior under traffic?