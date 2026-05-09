[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    throw "Docker CLI not found. Install Docker Desktop or ensure `docker` is on PATH before starting the database-first path."
}

$previousErrorActionPreference = $ErrorActionPreference
$ErrorActionPreference = 'Continue'
& docker info *> $null
$dockerInfoExitCode = $LASTEXITCODE
$ErrorActionPreference = $previousErrorActionPreference

if ($dockerInfoExitCode -ne 0) {
    throw "Docker daemon is not reachable. Start Docker Desktop or the Docker engine, then rerun the database-first startup task or script."
}

Write-Host "Starting the SqlAcademy database-first path (sqlserver + sqlserver-init)..."
& docker compose up -d sqlserver sqlserver-init
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "Current database-first service state:"
& docker compose ps sqlserver sqlserver-init
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "Next steps:"
Write-Host "1. Connect to LearningDb in your SQL client."
Write-Host "2. Run the first read-only queries from docs/learning/sql-first-day-one.md."
Write-Host "3. Use 'docker compose up --build' later when you want the full API and observability stack."