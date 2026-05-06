[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$Exercise
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-DotEnvValue {
    param(
        [Parameter(Mandatory)]
        [string]$Path,
        [Parameter(Mandatory)]
        [string]$Name,
        [Parameter(Mandatory)]
        [string]$DefaultValue
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        return $DefaultValue
    }

    $pattern = '^{0}=(.*)$' -f [Regex]::Escape($Name)

    foreach ($line in Get-Content -LiteralPath $Path) {
        if ($line -match $pattern) {
            return $Matches[1].Trim().Trim('"')
        }
    }

    return $DefaultValue
}

$repoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$tempScriptPath = $null

Push-Location $repoRoot

try {
    $exercisePath = if ([System.IO.Path]::IsPathRooted($Exercise)) {
        [System.IO.Path]::GetFullPath($Exercise)
    }
    else {
        [System.IO.Path]::GetFullPath((Join-Path $repoRoot $Exercise))
    }

    if (-not $exercisePath.StartsWith($repoRoot, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Exercise path '$Exercise' must be inside the repository."
    }

    foreach ($requiredFile in @('starter.sql', 'answer.sql', 'validation.sql')) {
        $requiredPath = Join-Path $exercisePath $requiredFile

        if (-not (Test-Path -LiteralPath $requiredPath)) {
            throw "Expected '$requiredFile' under '$exercisePath'."
        }
    }

    $relativeExercise = $exercisePath.Substring($repoRoot.Length).TrimStart('\', '/')
    $workspaceExercise = $relativeExercise -replace '\\', '/'

    $envPath = Join-Path $repoRoot '.env'
    $saPassword = Get-DotEnvValue -Path $envPath -Name 'MSSQL_SA_PASSWORD' -DefaultValue 'SqlAcademy_dev_2026!'
    $databaseName = Get-DotEnvValue -Path $envPath -Name 'LEARNING_DB_NAME' -DefaultValue 'LearningDb'

    $tempScriptName = '.exercise-validation.{0}.sql' -f [Guid]::NewGuid().ToString('N')
    $tempScriptPath = Join-Path $repoRoot $tempScriptName
    $tempScriptContainerPath = '/workspace/{0}' -f $tempScriptName
    $starterContainerPath = '/workspace/{0}/starter.sql' -f $workspaceExercise
    $answerContainerPath = '/workspace/{0}/answer.sql' -f $workspaceExercise
    $validationContainerPath = '/workspace/{0}/validation.sql' -f $workspaceExercise

    $sqlcmdScript = @"
PRINT N'Running starter.sql for $workspaceExercise';
:r $starterContainerPath

PRINT N'Running answer.sql for $workspaceExercise';
:r $answerContainerPath

PRINT N'Running validation.sql for $workspaceExercise';
:r $validationContainerPath
"@

    Set-Content -LiteralPath $tempScriptPath -Value $sqlcmdScript -Encoding utf8NoBOM

    Write-Host 'Ensuring SQL Server is running...' -ForegroundColor Cyan
    & docker compose up -d sqlserver | Out-Host

    Write-Host 'Ensuring LearningDb is initialized...' -ForegroundColor Cyan
    & docker compose run --rm sqlserver-init /bin/bash /workspace/infra/scripts/init-database.sh | Out-Host

    Write-Host ("Validating exercise '{0}'..." -f $relativeExercise) -ForegroundColor Cyan
    & docker compose run --rm --no-deps sqlserver-init /opt/mssql-tools18/bin/sqlcmd -S sqlserver,1433 -U sa -P $saPassword -d $databaseName -C -b -i $tempScriptContainerPath

    if ($LASTEXITCODE -ne 0) {
        throw "Exercise validation failed for '$relativeExercise'."
    }

    Write-Host 'Exercise validation succeeded.' -ForegroundColor Green
}
finally {
    if ($null -ne $tempScriptPath -and (Test-Path -LiteralPath $tempScriptPath)) {
        Remove-Item -LiteralPath $tempScriptPath -Force -ErrorAction SilentlyContinue
    }

    Pop-Location
}