<#
.SYNOPSIS
    Compila el proyecto de pruebas de interfaz y descarga Chromium para Playwright.
.DESCRIPTION
    Se ejecuta una vez por equipo. Necesita salida a internet (o el proxy configurado en HTTPS_PROXY).
#>
$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot
$proyecto = Join-Path $raiz 'tests/Ayudas.Tests.E2E'

dotnet build $proyecto
if ($LASTEXITCODE -ne 0) {
    throw 'No se ha podido compilar tests/Ayudas.Tests.E2E: revisa el acceso a NuGet (paquete Microsoft.Playwright.Xunit).'
}

$script = Join-Path $proyecto 'bin/Debug/net8.0/playwright.ps1'
# playwright.ps1 necesita PowerShell 7 (pwsh); Windows PowerShell 5.1 no sirve.
if (-not (Get-Command pwsh -ErrorAction SilentlyContinue)) {
    throw 'Falta PowerShell 7. Instálalo con: winget install Microsoft.PowerShell (y abre una terminal nueva).'
}
pwsh -NoProfile -ExecutionPolicy Bypass -File $script install chromium
if ($LASTEXITCODE -ne 0) {
    throw 'No se ha podido descargar Chromium: revisa la salida a internet (o HTTPS_PROXY).'
}

Write-Host 'Playwright listo: dotnet test tests/Ayudas.Tests.E2E con la web levantada.' -ForegroundColor Green
