# Local Setup Troubleshooting

Use this page when the first-run path fails before you can reach a predictable query against `LearningDb`.

Work from the smallest failing surface outward:

1. can Docker start the relevant containers?
2. can SQL Server accept a local connection?
3. did `sqlserver-init` create and seed `LearningDb`?
4. only after that, does the full API stack become relevant?

## Fast Structural Check

Run this first when the Compose file itself may be wrong:

```bash
docker compose config
```

If this fails, fix the configuration issue before debugging containers.

## Preferred First-Query Startup Path

If you only need the database for the first session, start the minimal services instead of the whole platform:

```bash
docker compose up -d sqlserver sqlserver-init
```

That starts SQL Server and the database bootstrap step without the API, worker, Prometheus, Grafana, or OpenTelemetry collector.

## Check Container State

Use this command to see whether the relevant services are up:

```bash
docker compose ps
```

For a database-first session, focus on:

- `sqlserver`
- `sqlserver-init`

For the full platform, also inspect:

- `api`
- `worker`
- `prometheus`
- `grafana`

## SQL Server Is Not Starting

Common causes:

- Docker Desktop is not running
- the local SQL Server port is already in use
- the `sa` password in `.env` is invalid or inconsistent

Useful checks:

```bash
docker compose logs sqlserver
```

On Windows, check whether the configured port is already taken:

```powershell
Get-NetTCPConnection -LocalPort 14333 -ErrorAction SilentlyContinue
```

If another process is using the port, change `MSSQL_PORT` in `.env` and reconnect with the new value.

## `LearningDb` Does Not Exist Yet

The most common cause is that `sqlserver-init` did not complete successfully.

Check its logs:

```bash
docker compose logs sqlserver-init
```

If `sqlserver` is healthy but the database is still missing, restart just the bootstrap step:

```bash
docker compose up sqlserver-init
```

Then reconnect and run:

```sql
SELECT COUNT(*) AS UserCount
FROM academy.Users;
```

## SQL Client Login Fails

Start with the defaults from [SQL Client Connection Guide](sql-client-connection-guide.md):

- server: `localhost,14333`
- login: `sa`
- password: `SqlAcademy_dev_2026!`
- database: `LearningDb`

If the client still fails:

- confirm you are using `localhost,14333`, not only `localhost`
- confirm the password matches your current `.env`
- trust the local server certificate if your client prompts for it
- verify that `sqlserver` is running before blaming the client

## API Readiness Never Turns Healthy

If the database-first path works but `http://localhost:8080/health/ready` does not, narrow the problem to the API rather than the database.

Check:

```bash
docker compose logs api
```

Common causes:

- `sqlserver-init` did not finish cleanly
- another process already uses port `8080`
- the API is waiting on dependencies you did not start yet during a partial startup

If your first goal is only a SQL query, stop here and use the reduced startup path instead of debugging the full platform prematurely.

## Resetting Local State

If the local volume is stale or half-initialized, you can fully reset the local stack.

Warning: this deletes the local SQL Server volume and reseeds the environment from scratch.

```bash
docker compose down -v
docker compose up -d sqlserver sqlserver-init
```

Use this only after simpler checks fail.

## Best Companions

1. [SQL-First Day One](sql-first-day-one.md)
2. [SQL Client Connection Guide](sql-client-connection-guide.md)
3. [Phase 0: Local Setup](../phases/phase-0-local-setup.md)