<#
.SYNOPSIS
    Guarda el trabajo de una sesión en una rama propia, para poder cambiar de sesión.
.EXAMPLE
    .\scripts\guardar-trabajo.ps1 -Sesion 3
#>
param(
    [Parameter(Mandatory = $true)]
    [ValidateRange(1, 6)]
    [int] $Sesion
)

$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot
Set-Location $raiz

$rama = "mi-trabajo-s$Sesion"
$actual = git branch --show-current

if ($actual -ne $rama) {
    if (git branch --list $rama) {
        git switch $rama
    }
    else {
        git switch -c $rama
    }
}

git add -A
git commit -m "Mi trabajo de la sesion $Sesion"
Write-Host "Trabajo guardado en la rama $rama." -ForegroundColor Green
