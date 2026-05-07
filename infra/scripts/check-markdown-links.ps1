[CmdletBinding()]
param(
    [string[]]$Paths = @(
        'README.md',
        'CONTRIBUTING.md',
        '.ai',
        'docs',
        'src/exercises'
    )
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$excludedPathPattern = '[\\/](?:\.git|bin|obj)(?:[\\/]|$)'

function Get-AbsolutePath {
    param(
        [Parameter(Mandatory)]
        [string]$Path,
        [Parameter(Mandatory)]
        [string]$BasePath
    )

    if ([System.IO.Path]::IsPathRooted($Path)) {
        return [System.IO.Path]::GetFullPath($Path)
    }

    return [System.IO.Path]::GetFullPath((Join-Path $BasePath $Path))
}

function Get-RepoRelativePath {
    param(
        [Parameter(Mandatory)]
        [string]$FullPath
    )

    return $FullPath.Substring($repoRoot.Length).TrimStart('\', '/') -replace '\\', '/'
}

$markdownFiles = New-Object System.Collections.Generic.List[string]

foreach ($path in $Paths) {
    $absolutePath = Get-AbsolutePath -Path $path -BasePath $repoRoot

    if (-not (Test-Path -LiteralPath $absolutePath)) {
        throw "Path '$path' does not exist."
    }

    $item = Get-Item -LiteralPath $absolutePath -Force

    if ($item.PSIsContainer) {
        Get-ChildItem -LiteralPath $absolutePath -Filter '*.md' -Recurse -File -Force |
            Where-Object { $_.FullName -notmatch $excludedPathPattern } |
            ForEach-Object { $markdownFiles.Add($_.FullName) }
    }
    elseif ($item.Extension -ieq '.md') {
        $markdownFiles.Add($item.FullName)
    }
}

$failures = New-Object System.Collections.Generic.List[string]

foreach ($file in $markdownFiles | Sort-Object -Unique) {
    $content = Get-Content -LiteralPath $file -Raw
    $matches = [regex]::Matches($content, '\[[^\]]+\]\((?!https?://|mailto:|#)([^)]+)\)')
    $baseDirectory = Split-Path -Parent $file

    foreach ($match in $matches) {
        $target = $match.Groups[1].Value.Trim()

        if ([string]::IsNullOrWhiteSpace($target)) {
            continue
        }

        $targetPath = ($target -split '#', 2)[0]
        $targetPath = ($targetPath -split '\?', 2)[0]
        $targetPath = [System.Uri]::UnescapeDataString($targetPath.Trim())

        if ([string]::IsNullOrWhiteSpace($targetPath)) {
            continue
        }

        if ($targetPath.StartsWith('/')) {
            $resolvedTarget = Get-AbsolutePath -Path $targetPath.TrimStart('/') -BasePath $repoRoot
        }
        else {
            $resolvedTarget = Get-AbsolutePath -Path $targetPath -BasePath $baseDirectory
        }

        if (-not (Test-Path -LiteralPath $resolvedTarget)) {
            $failures.Add("$(Get-RepoRelativePath -FullPath $file) -> $targetPath")
        }
    }
}

if ($failures.Count -gt 0) {
    $failures | Sort-Object -Unique | ForEach-Object { Write-Output $_ }
    exit 1
}

Write-Output 'All checked markdown links resolve successfully.'