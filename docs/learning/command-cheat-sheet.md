# Command Cheat Sheet

Use this page when you want the shortest working command lookup for first-run startup, connection checks, validation, or focused debugging.

If you changed `.env`, substitute your local port, password, or database name.

## First-Run Startup

### Database-First Path

In VS Code, run the task `sqlacademy: start database-first path`.

If you prefer the terminal directly, use:

```powershell
powershell -ExecutionPolicy Bypass -File infra/scripts/start-database-first.ps1
```

### Full Learning Stack

```bash
docker compose up --build
```

### Structural Check Before Startup

```bash
docker compose config
```

## Container State And Logs

### Show Current Container State

```bash
docker compose ps
```

### Show SQL Server Logs

```bash
docker compose logs sqlserver
```

### Show Database Bootstrap Logs

```bash
docker compose logs sqlserver-init
```

### Rerun Only The Bootstrap Step

```bash
docker compose up sqlserver-init
```

## Quick Connection Proof

### sqlcmd

```bash
sqlcmd -S localhost,14333 -U sa -P "SqlAcademy_dev_2026!" -C -d LearningDb -Q "SELECT COUNT(*) AS UserCount FROM academy.Users;"
```

### Expected Success Shape

```text
UserCount
---------
4

(1 rows affected)
```

## Validation Harness

### Run A Validation Pack

```powershell
powershell -ExecutionPolicy Bypass -File infra/scripts/run-exercise-validation.ps1 -Exercise src/exercises/Beginner/000-sql-fundamentals-and-safe-changes
```

### Replace The Exercise Path

Use the exercise folder you are currently working through, for example:

- `src/exercises/Intermediate/001-window-functions-and-pagination`
- `src/exercises/Advanced/001-indexing-parameter-sniffing-and-migration-safety`
- `src/exercises/Senior/003-transactional-outbox-and-delivery-consistency`

## Focused Test Commands

### Fast Unit-Test Slice

```bash
dotnet test tests/SqlAcademy.UnitTests/SqlAcademy.UnitTests.csproj -v minimal
```

### Focused API Contract Check

```bash
dotnet test tests/SqlAcademy.IntegrationTests/SqlAcademy.IntegrationTests.csproj -v minimal --filter "FullyQualifiedName~PostsEndpointTests"
```

### Focused Performance Lab Check

```bash
dotnet test tests/SqlAcademy.PerformanceTests/SqlAcademy.PerformanceTests.csproj -v minimal --filter "FullyQualifiedName~QueryStoreLabSmokeTests"
```

## Reset Commands

### Safe Retry First

```bash
docker compose up sqlserver-init
```

### Destructive Local Reset

Warning: this deletes the local SQL Server volume and reseeds the environment from scratch.

```bash
docker compose down -v
powershell -ExecutionPolicy Bypass -File infra/scripts/start-database-first.ps1
```

## Best Companions

1. [SQL-First Day One](sql-first-day-one.md)
2. [SQL Client Connection Guide](sql-client-connection-guide.md)
3. [Local Setup Troubleshooting](local-setup-troubleshooting.md)