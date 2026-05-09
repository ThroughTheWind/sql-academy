# Maintainer Workflow

Use this workflow when changing docs, exercises, code, infrastructure, or planning surfaces for the repository.

## Core Contract

1. Name the concrete outcome and whether it is learner-facing, maintainer-facing, or both.
2. Pick one owner surface before editing. Use [repo-map.md](repo-map.md) if the owner is unclear.
3. State one falsifiable local hypothesis and one cheap check that could disprove it.
4. Make the smallest change that tests or satisfies that hypothesis.
5. Run the first focused validation from [validation-matrix.md](validation-matrix.md) before widening scope.
6. Update neighboring docs or tracked backlog state only when the change alters a contract, route, or execution slice.

## Boundary Rules

- [../README.md](../README.md) stays learner-oriented and should not become a maintainer workflow dump.
- [../docs/learning](../docs/learning/README.md) owns the ordered learner route and lesson depth.
- [../docs/README.md](../docs/README.md) owns documentation routing and should stay a router instead of a second curriculum map.
- [../docs/exercises/README.md](../docs/exercises/README.md) owns exercise modes and validation-pack rules.
- [.](README.md) owns maintainer workflow notes, repo maps, and internal execution tracking.

## Update Rules

- Update [academy-enhancements-backlog.md](academy-enhancements-backlog.md) when a maintainer slice changes learner-path simplification, onboarding, validation-support, or capstone-quality follow-on work.
- Update [sql-efcore-mastery-backlog.md](sql-efcore-mastery-backlog.md) when a maintainer slice changes mastery-scope depth, optional-specialization coverage, historical completion state, or the old track's acceptance checks.
- Update [../docs/learning/repository-improvement-suggestions.md](../docs/learning/repository-improvement-suggestions.md) only when the learner-facing public summary changes.
- Update [repo-map.md](repo-map.md) when a new owner surface becomes important enough that future maintainers or agents should route there first.
- Update [validation-matrix.md](validation-matrix.md) when you add a reusable narrow check such as a focused test, script, or smoke command.

## Before Closing A Slice

- Did the change land in the authoritative owner surface rather than a summary file?
- Did the first validation match the claim instead of defaulting to a broad build or CI imitation?
- Did the teaching surface, practice surface, and validation surface stay aligned?
- Did the change avoid pushing maintainer execution state into learner-facing docs?
- Did the repo workflow docs gain anything reusable that should be captured immediately?