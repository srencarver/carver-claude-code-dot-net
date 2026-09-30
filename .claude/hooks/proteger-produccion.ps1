# Hook PreToolUse de Claude Code: impide editar la configuración de producción.
# Recibe por la entrada estándar el JSON de la herramienta que Claude va a usar.
# Código de salida 2: la acción se cancela y el mensaje de error llega a Claude.
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

$entrada = [Console]::In.ReadToEnd() | ConvertFrom-Json
$ruta = $entrada.tool_input.file_path

if ($ruta -and (($ruta -replace '\\', '/') -match '(^|/)appsettings\.Production\.json$')) {
    [Console]::Error.WriteLine('Bloqueado: appsettings.Production.json es la configuración de producción y no se edita desde Claude Code. Si hace falta un cambio, se pide a Sistemas.')
    exit 2
}

exit 0
