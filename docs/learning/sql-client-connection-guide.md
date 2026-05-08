# SQL Client Connection Guide

Use this guide when the missing step is not SQL itself but the practical act of connecting your client to the local SQL Server container.

This page assumes the default local `.env` values unless stated otherwise.

## Default Local Connection

If you have not changed `.env`, use these values:

| Setting | Default Value |
| --- | --- |
| Server | `localhost,14333` |
| Database | `LearningDb` |
| Login | `sa` |
| Password | `SqlAcademy_dev_2026!` |

If your SQL client asks about encryption or certificate trust for the local container, allow the connection and trust the local self-signed certificate.

## Azure Data Studio

Use these values in a new connection:

| Field | Value |
| --- | --- |
| Connection type | `Microsoft SQL Server` |
| Server | `localhost,14333` |
| Authentication type | `SQL Login` |
| User name | `sa` |
| Password | `SqlAcademy_dev_2026!` |
| Database | `LearningDb` |
| Encrypt | default is fine for local use |
| Trust server certificate | `true` if prompted |

After the connection succeeds, run:

```sql
SELECT COUNT(*) AS UserCount
FROM academy.Users;
```

## SQL Server Management Studio

Use these values in the Connect to Server dialog:

| Field | Value |
| --- | --- |
| Server type | `Database Engine` |
| Server name | `localhost,14333` |
| Authentication | `SQL Server Authentication` |
| Login | `sa` |
| Password | `SqlAcademy_dev_2026!` |

If SSMS shows encryption warnings for the local container, open the connection options and trust the server certificate.

After the connection succeeds, set the database context to `LearningDb` and run:

```sql
SELECT TOP (4) Id, UserName, Email
FROM academy.Users
ORDER BY Id;
```

## sqlcmd

If `sqlcmd` is available locally, this command proves the connection and seeded data in one step:

```bash
sqlcmd -S localhost,14333 -U sa -P "SqlAcademy_dev_2026!" -C -d LearningDb -Q "SELECT COUNT(*) AS UserCount FROM academy.Users;"
```

If you changed `.env`, substitute your local port, password, or database name.

## What A Good First Connection Proves

- the SQL Server container is reachable from your machine
- login credentials are correct
- the initialization step created `LearningDb`
- the seeded `academy` schema is available

## If The Connection Fails

- use [Local Setup Troubleshooting](local-setup-troubleshooting.md) for port, Docker, readiness, and login checks
- use [SQL-First Day One](sql-first-day-one.md) if you want the smallest working path before the full course route
- use [Schema Quick Reference](schema-quick-reference.md) once the connection works but the table layout still feels abstract