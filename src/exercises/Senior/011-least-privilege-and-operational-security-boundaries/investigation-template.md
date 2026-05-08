# Investigation Template

## Boundary Decision

- is the current posture acceptable only for local learning, or is it already too permissive for any shared environment?
- which one environment or ownership fact makes that answer non-negotiable?

## Privilege Surface Review

- which runtime credential fact matters most?
- which migration or deployment authority fact matters most?
- which operator-admin surface would still be ambiguous today?

## Separation-Of-Duties Requirement

- what minimum boundary would you require between application runtime, migration execution, and operator-admin work?
- where does least privilege matter first in this repo?
- what local-learning shortcut must stop before shared use?

## Operational Proof

- which one configuration, deployment step, or review check would prove the privileged boundary actually changed?
- what evidence should be recorded so the next operator can see who owns runtime credentials, migration authority, and admin access?
- what result would force you to stop calling the environment defensible?
