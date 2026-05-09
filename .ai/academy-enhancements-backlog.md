# Academy Enhancements Backlog

This is the maintainer execution backlog for improving learner-path clarity, onboarding ergonomics, validation support, and final-assessment quality after the current mastery-scope backlog reached a stable stopping point.

## Target

Keep the academy focused on working backend and application engineers, but make the current route easier to enter, navigate, validate, and complete without widening the audience or diluting the core SQL Server plus .NET scope.

## Scope Boundary

- in scope: learner-path clarity, documentation consistency, onboarding friction, validation ergonomics, capstone quality, and specialization packaging
- out of scope: turning the academy into a zero-assumption beginner course, folding optional DBA or DBRE work into the default route, or broad platform expansion before the current route is easier to use

## Status Legend

- `not-started`: planned but not yet underway
- `in-progress`: active implementation slice
- `completed`: merged into the repository
- `blocked`: waiting on a prerequisite decision or missing surface

## Backlog

| ID | Priority | Status | Area | Outcome | Notes |
| --- | --- | --- | --- | --- | --- |
| E001 | P0 | completed | learner routing | align learner entry surfaces around one route-selection block and one authoritative progression map | implemented a shared route-selection pattern in `README.md`, `docs/README.md`, `docs/learning/README.md`, and `docs/learning/how-to-start.md`, while keeping `docs/learning/curriculum-map.md` as the deciding surface |
| E002 | P0 | completed | route inventory | decide whether the root README markets the core route or the full exercise inventory, and resolve `Advanced 004` placement consistently | root `README.md` now shows a core-route practice snapshot, `docs/exercises/README.md` owns the full inventory, and `Advanced 004` is explicitly marked as optional performance deepening in the curriculum, follow-up, and performance surfaces |
| E003 | P1 | not-started | startup ergonomics | add a dedicated Compose profile, script, or VS Code task for the reduced database-first startup path | owner surface starts at `docs/learning/sql-first-day-one.md`; validate both reduced-startup and full-stack first-run paths |
| E004 | P1 | not-started | onboarding support | add a compact command cheat sheet, a local reset FAQ, and screenshot or transcript support for early setup recovery | owner surfaces start at `docs/learning/sql-client-connection-guide.md` and `docs/learning/local-setup-troubleshooting.md` |
| E005 | P1 | not-started | early-route reinforcement | add printable one-page checklists for Phase 0, Lessons 01 through 03, and the beginner and intermediate packs | keep these as learner aids rather than a competing curriculum map |
| E006 | P1 | not-started | completion clarity | extend sample outputs into deterministic advanced packs and add structured completion checklists or rubrics to guided labs and investigation packs | start with the advanced and senior packs that currently require the most cross-referencing to know what done means |
| E007 | P2 | not-started | validation ergonomics | add or surface narrow smoke or asset-contract validation anchors for remaining manual-heavy packs where realistic | route those checks directly from learner docs instead of leaving them discoverable only from tests or track pages |
| E008 | P2 | not-started | final assessment quality | expand Lesson 11 and the senior assessment pack with richer capstone scoring rubrics and clearer design-defense templates | keep the focus on defendable engineering judgment rather than solution-key prose |
| E009 | P2 | not-started | review-style practice | add pull-request-style review exercises for migration safety, indexing tradeoffs, and release readiness | prefer existing repo anchors and narrow validation paths over synthetic review artifacts |
| E010 | P3 | not-started | specialization packaging | repackage the DBA and DBRE modules as a clearer post-core specialization menu with visible entry bar, effort estimate, and prerequisite evidence | preserve the current boundary that specialization stays optional after the core route |
| E011 | P3 | not-started | performance follow-on depth | add statistics-drift and skew-focused performance scenarios after route and validation improvements land | reuse the existing workload variant, Query Store, and performance-track anchors instead of widening the default seed path |
| E012 | P3 | not-started | scenario expansion | add deeper incident or release follow-ons and multi-domain capstone expansion only after earlier ergonomics work stabilize | avoid adding more breadth before the route is easier to use end to end |

## Current Slice

### Completed

- E001: aligned the entry surfaces around one route-selection pattern and one authoritative curriculum contract
- E002: clarified that the root README shows the core-route practice snapshot, while `Advanced 004` remains optional performance deepening instead of a default-route step

### Next Recommended Slice

- E003: ship a database-first startup task or profile immediately after the route wording is stable
- E004: add the command cheat sheet and recovery-support surfaces once the startup-task shape is chosen
- E005: add printable early-route checklists after the route wording and first-run workflow stop moving

## Acceptance Checks

1. `README.md`, `docs/README.md`, `docs/learning/README.md`, and `docs/learning/curriculum-map.md` must not present competing route order or conflicting core versus optional pack placement.
2. Every enhancement should make the next learner decision faster or the completion state clearer; do not add alternate maps without removing earlier ambiguity.
3. Advanced guided labs and investigation packs should name an explicit completion contract and, where realistic, one narrow validation anchor.
4. `Advanced 004`, `Advanced 005`, `Senior 005`, and `Senior 006` must have deliberate placement across the core route, performance track, exercise index, and assessment surfaces.
5. Keep maintainer execution state in `.ai`; update `docs/learning/repository-improvement-suggestions.md` only when the public learner-facing summary changes.