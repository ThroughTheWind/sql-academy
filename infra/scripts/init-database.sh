#!/usr/bin/env bash
set -euo pipefail

SQLCMD_BIN="/opt/mssql-tools18/bin/sqlcmd"
SQL_SERVER="${MSSQL_SERVER:-sqlserver}"
DATABASE_NAME="${LEARNING_DB_NAME:-LearningDb}"

if [[ -z "${MSSQL_SA_PASSWORD:-}" ]]; then
  echo "MSSQL_SA_PASSWORD must be provided to initialize the database."
  exit 1
fi

echo "Waiting for SQL Server at ${SQL_SERVER}..."
until "${SQLCMD_BIN}" -S "${SQL_SERVER},1433" -U sa -P "${MSSQL_SA_PASSWORD}" -C -Q "SELECT 1" >/dev/null 2>&1; do
  echo "SQL Server is not ready yet. Retrying in 2 seconds."
  sleep 2
done

echo "Creating schema for ${DATABASE_NAME}."
"${SQLCMD_BIN}" -S "${SQL_SERVER},1433" -U sa -P "${MSSQL_SA_PASSWORD}" -C -b -i /workspace/db/schemas/001_create_learning_db.sql -v DatabaseName="${DATABASE_NAME}"

echo "Seeding reference data."
"${SQLCMD_BIN}" -S "${SQL_SERVER},1433" -U sa -P "${MSSQL_SA_PASSWORD}" -C -b -i /workspace/db/seed/001_seed_reference_data.sql -v DatabaseName="${DATABASE_NAME}"

echo "Seeding relational sample data."
"${SQLCMD_BIN}" -S "${SQL_SERVER},1433" -U sa -P "${MSSQL_SA_PASSWORD}" -C -b -i /workspace/db/seed/002_seed_social_and_orders.sql -v DatabaseName="${DATABASE_NAME}"

echo "LearningDb initialization completed successfully."