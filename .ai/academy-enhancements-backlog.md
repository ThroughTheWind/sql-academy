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
| E003 | P1 | completed | startup ergonomics | add a dedicated Compose profile, script, or VS Code task for the reduced database-first startup path | implemented `infra/scripts/start-database-first.ps1`, added the `sqlacademy: start database-first path` task in `.vscode/tasks.json`, and routed the first-query docs through that entry point |
| E004 | P1 | completed | onboarding support | add a compact command cheat sheet, a local reset FAQ, and screenshot or transcript support for early setup recovery | implemented `docs/learning/command-cheat-sheet.md`, added transcript snippets to the connection and troubleshooting guides, and added a reset FAQ to `docs/learning/local-setup-troubleshooting.md` |
| E005 | P1 | completed | early-route reinforcement | add printable one-page checklists for Phase 0, Lessons 01 through 03, and the beginner and intermediate packs | implemented `docs/learning/early-route-printable-checklists.md` and linked the relevant phase, lesson, and checkpoint pack surfaces back to it without changing the curriculum order |
| E006 | P1 | completed | completion clarity | extend sample outputs into deterministic advanced packs and add structured completion checklists or rubrics to guided labs and investigation packs | added sample-output cues to `Advanced 001` and `Advanced 002`, plus completion checklists across the core-route guided labs and investigation packs that previously required more cross-referencing |
| E007 | P2 | completed | validation ergonomics | add or surface narrow smoke or asset-contract validation anchors for remaining manual-heavy packs where realistic | surfaced focused companion checks directly in the remaining core manual-heavy pack READMEs and reinforced that routing in `docs/exercises/README.md` |
| E008 | P2 | completed | final assessment quality | expand Lesson 11 and the senior assessment pack with richer capstone scoring rubrics and clearer design-defense templates | added reusable scoring grids, a design-defense template, and a final-defense checklist without turning the lesson into a solution-key dump |
| E009 | P2 | completed | review-style practice | add pull-request-style review exercises for migration safety, indexing tradeoffs, and release readiness | implemented `docs/learning/review-style-practice.md` using the existing pack surfaces and narrow validation anchors instead of synthetic review artifacts |
| E010 | P3 | completed | specialization packaging | repackage the DBA and DBRE modules as a clearer post-core specialization menu with visible entry bar, effort estimate, and prerequisite evidence | refreshed the extension-track doc into a specialization menu and routed the main docs tables to that entry bar instead of treating it like a vague side path |
| E011 | P3 | completed | performance follow-on depth | add statistics-drift and skew-focused performance scenarios after route and validation improvements land | implemented `docs/performance/statistics-drift-and-skew-follow-ons.md` and routed the performance and lesson surfaces to it instead of widening the default seed path |
| E012 | P3 | completed | scenario expansion | add deeper incident or release follow-ons and multi-domain capstone expansion only after earlier ergonomics work stabilize | expanded Phase 9, Lesson 11, and the senior assessment surfaces with deeper incident, release, and multi-domain capstone scenarios without opening another route map |

## Current Slice

### Completed

- E001: aligned the entry surfaces around one route-selection pattern and one authoritative curriculum contract
- E002: clarified that the root README shows the core-route practice snapshot, while `Advanced 004` remains optional performance deepening instead of a default-route step
- E003: added a dedicated database-first startup script and VS Code task so learners no longer need to remember the minimal service names by hand
- E004: added a compact command cheat sheet, a reset FAQ, and short transcript-style examples for first-run connection and recovery flows
- E005: added printable early-route completion checklists for Phase 0, Lessons 01 through 03, and the first checkpoint packs without creating another route map
- E006: added sample-output cues to the deterministic advanced validation packs and completion checklists to the core manual-heavy guided labs and investigation packs
- E007: surfaced explicit focused companion checks directly in the remaining core manual-heavy pack READMEs and the exercise-system guidance
- E008: added richer capstone scoring rubrics and reusable design-defense templates to the final lesson and senior assessment surfaces
- E009: added learner-facing review-style drills that reuse the existing migration, indexing, and release anchors plus their narrow validation checks
- E010: repackaged the DBA and DBRE extension into a clearer menu with entry-bar evidence, branch choices, and effort guidance
- E011: added an optional performance follow-on guide for statistics drift and skew using the existing workload variant, Query Store, and performance anchors
- E012: expanded Phase 9 and the final assessment surfaces with deeper incident, release, and multi-domain capstone follow-ons

### Next Recommended Slice

- no further recommended slice remains in this backlog; start a new backlog only when a new concrete owner surface and narrow validation path are clear

## Acceptance Checks

1. `README.md`, `docs/README.md`, `docs/learning/README.md`, and `docs/learning/curriculum-map.md` must not present competing route order or conflicting core versus optional pack placement.
2. Every enhancement should make the next learner decision faster or the completion state clearer; do not add alternate maps without removing earlier ambiguity.
3. Advanced guided labs and investigation packs should name an explicit completion contract and, where realistic, one narrow validation anchor.
4. `Advanced 004`, `Advanced 005`, `Senior 005`, and `Senior 006` must have deliberate placement across the core route, performance track, exercise index, and assessment surfaces.
5. Keep maintainer execution state in `.ai`; update `docs/learning/repository-improvement-suggestions.md` only when the public learner-facing summary changes.