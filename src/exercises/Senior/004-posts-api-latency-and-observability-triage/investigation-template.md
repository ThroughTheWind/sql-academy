# Investigation Template

## First Signal

- which one signal made you start here?

## Narrowed Boundary

- which controller, service, or query path looks hottest?
- what evidence ruled out a full-stack outage?

## Query Hypothesis

- what SQL shape do you think became expensive?
- which filter, join, aggregate, or sort is most suspicious?

## Immediate Containment

- what is the safest action you would take in the incident window?
- what risk does that containment avoid?

## Durable Fix

- what code or SQL path needs a deeper redesign?
- what schema, index, or read-model work would you evaluate after the incident?

## Validation Plan

- which test proves correctness?
- which measurement proves performance improved?
- which telemetry signals would you compare before and after the fix?