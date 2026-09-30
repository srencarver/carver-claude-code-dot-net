<#
.SYNOPSIS
    Prepara el repositorio para empezar una sesión del curso.
.DESCRIPTION
    Comprueba que no hay cambios sin guardar, cambia a la rama de la sesión, la actualiza,
    recrea la base de datos y compila la solución.
.EXAMPLE
    .\scripts\empezar-sesion.ps1 -Sesion 3
#>
param(
    [Parameter(Mandatory = $true)]
    [ValidateRange(1, 6)]
    [int] $Sesion
)

$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot
Set-Location $raiz

if (git status --porcelain) {
    Write-Host 'Tienes cambios sin guardar. Guárdalos antes de cambiar de sesión:' -ForegroundColor Yellow
    Write-Host "    .\scripts\guardar-trabajo.ps1 -Sesion $($Sesion - 1)"
    exit 1
}

$rama = if ($Sesion -eq 1) { 'main' } else { "s$Sesion-inicio" }

git fetch origin
git switch $rama
git pull --ff-only

& (Join-Path $PSScriptRoot 'reset-bd.ps1')

dotnet build
Write-Host "Listo: rama $rama con la base de datos recreada." -ForegroundColor Green
