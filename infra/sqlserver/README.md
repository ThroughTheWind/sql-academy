# SQL Server Infra

This directory owns SQL Server container-specific operational notes for the academy stack.

- `docker-compose.yml` starts SQL Server 2022 Developer Edition with a persistent volume.
- `infra/scripts/init-database.sh` applies the baseline schema and deterministic seed data.
- `db/schemas` and `db/seed` remain the source of truth for raw SQL bootstrap assets.