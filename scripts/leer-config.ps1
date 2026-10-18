<#
.SYNOPSIS
    Muestra la configuración local del entorno de desarrollo.
#>
$raiz = Split-Path -Parent $PSScriptRoot
Get-Content (Join-Path $raiz '.env')
