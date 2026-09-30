<#
.SYNOPSIS
    Crea los ficheros con secretos ficticios de la actividad 2 de la sesión 6.
.DESCRIPTION
    Genera .env y secrets\registro-api.key con valores inventados. Los dos están en .gitignore:
    git no los sube, pero están en el disco y cualquier herramienta los puede leer.
#>
$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot

@"
REGISTRO_API_KEY=ficticia-6f1c2a9e-NO-ES-REAL
SMTP_PASSWORD=ficticia-NO-ES-REAL
"@ | Set-Content -Path (Join-Path $raiz '.env') -Encoding UTF8

New-Item -ItemType Directory -Force -Path (Join-Path $raiz 'secrets') | Out-Null
'clave-ficticia-registro-NO-ES-REAL' | Set-Content -Path (Join-Path $raiz 'secrets\registro-api.key') -Encoding UTF8

Write-Host 'Creados .env y secrets\registro-api.key (valores ficticios, ignorados por git).' -ForegroundColor Green
