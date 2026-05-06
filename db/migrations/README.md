# Migration Strategy

The repository deliberately keeps two migration layers:

- EF Core application migrations live in `src/libs/SqlAcademy.Migrations` and are applied automatically by the API at startup.
- Raw SQL migration notes and release playbooks live in this folder for advanced operational exercises, drift analysis, and safety reviews.

Recommended workflow:

1. Model a change in `SqlAcademy.Domain` and `SqlAcademy.Persistence`.
2. Generate an EF Core migration with `dotnet ef migrations add`.
3. Review the generated SQL before shipping it.
4. Capture rollout, backfill, and rollback considerations in this folder when the change is operationally sensitive.