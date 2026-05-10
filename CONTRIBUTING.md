# Contributing

SqlAcademy is learner-facing on the surface and maintainer-oriented underneath. Keep the root README, lesson docs, phase guides, and exercise docs readable for learners. Put maintainer-only planning, AI workflow guidance, and execution tracking under [.ai](.ai/README.md).

## Maintainer Start

If you are changing the repository rather than studying it:

1. Read [.ai/maintainer-workflow.md](.ai/maintainer-workflow.md).
2. Use [.ai/repo-map.md](.ai/repo-map.md) to find the owning surface before editing.
3. Use [.ai/validation-matrix.md](.ai/validation-matrix.md) to pick the smallest validation command that can falsify the change.
4. Use [.ai/templates/change-brief.md](.ai/templates/change-brief.md) when the slice touches more than one surface or needs a handoff.
5. Update [.ai/lesson-exercise-quality-backlog.md](.ai/lesson-exercise-quality-backlog.md) for per-surface lesson and exercise audit findings, update [.ai/academy-enhancements-backlog.md](.ai/academy-enhancements-backlog.md) for current route, onboarding, validation, or capstone-quality execution state, update [.ai/sql-efcore-mastery-backlog.md](.ai/sql-efcore-mastery-backlog.md) only for the older mastery-scope track, and update [docs/learning/repository-improvement-suggestions.md](docs/learning/repository-improvement-suggestions.md) only when a learner-facing public summary should change.

## Working Rules

- Start from one authoritative owner surface instead of editing multiple summaries in parallel.
- Keep the learner route in [docs/learning](docs/learning/README.md), the documentation router in [docs/README.md](docs/README.md), and maintainer execution state in [.ai](.ai/README.md).
- Keep every advanced topic aligned across teaching surface, practice surface, and validation surface.
- Prefer the smallest reversible change and the narrowest executable validation.
- If you discover a new reusable validation command or a recurring AI workflow pitfall, update the `.ai` workflow docs in the same change or the next immediate slice.

## Common Ownership Decisions

- Learner sequence or lesson depth: start in [docs/learning/curriculum-map.md](docs/learning/curriculum-map.md) and the specific lesson or phase file.
- Exercise behavior or mode: start in [docs/exercises/README.md](docs/exercises/README.md) and the specific pack under [src/exercises](src/exercises).
- Lesson or exercise quality audit: start in [.ai/lesson-exercise-quality-backlog.md](.ai/lesson-exercise-quality-backlog.md), then move to the owning lesson or pack surface once a finding is concrete.
- SQL bootstrap, seed, or migration behavior: start in [db](db) and [src/libs/SqlAcademy.Migrations](src/libs/SqlAcademy.Migrations).
- EF Core or Dapper query shape: start in [docs/learning/09-ef-core-dapper-and-query-shape.md](docs/learning/09-ef-core-dapper-and-query-shape.md) plus the owning query service under [src/libs/SqlAcademy.Persistence](src/libs/SqlAcademy.Persistence).
- AI workflow or maintainer planning: start in [.ai/README.md](.ai/README.md), not in learner docs.

## Fast Validation Commands

- Markdown and docs links: `powershell -ExecutionPolicy Bypass -File infra/scripts/check-markdown-links.ps1`
- Validation-ready SQL packs: `powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise <relative-exercise-path>`
- Fast unit slice: `dotnet test tests/SqlAcademy.UnitTests/SqlAcademy.UnitTests.csproj -v minimal`
- Solution build: `dotnet build SqlAcademy.slnx -v minimal`
- Docker shape: `docker compose config`

Use [.ai/validation-matrix.md](.ai/validation-matrix.md) when you need the nearest check for a specific change type.