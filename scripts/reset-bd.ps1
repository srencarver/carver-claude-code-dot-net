<#
.SYNOPSIS
    Recrea la base de datos de formación (LocalDB) con los datos de prueba.
.DESCRIPTION
    Borra la base de datos AyudasFormacion, la vuelve a crear con el esquema actual y carga
    los datos ficticios. Se usa al empezar cada sesión o si la base de datos queda en mal estado.
#>
$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot

dotnet run --project (Join-Path $raiz 'src/Ayudas.Web') -- --reset-bd --solo-bd
if ($LASTEXITCODE -ne 0) {
    throw 'No se ha podido recrear la base de datos: revisa el error de arriba. Si es de conexión, comprueba LocalDB con: sqllocaldb info. Si falta un runtime de .NET, compara: dotnet --list-runtimes'
}
