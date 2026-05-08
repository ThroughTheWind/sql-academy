# SQL Server Infra

This directory owns SQL Server container-specific operational notes for the academy stack.

- `docker-compose.yml` starts SQL Server 2022 Developer Edition with a persistent volume.
- `infra/scripts/init-database.sh` applies the baseline schema and deterministic seed data.
- `db/schemas` and `db/seed` remain the source of truth for raw SQL bootstrap assets.
- [Senior 007: Backup, Restore, And Recovery Verification](../../src/exercises/Senior/007-backup-restore-and-recovery-verification/README.md) is the nearest optional DBA/DBRE drill when you need to reason about backup posture and restore verification instead of only container bootstrap.