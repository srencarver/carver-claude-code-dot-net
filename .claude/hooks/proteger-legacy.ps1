# Hook PreToolUse de Claude Code: impide modificar el módulo heredado src/Ayudas.Web/Legacy/.
# CLAUDE.md lo pide; este hook lo obliga.
# Recibe por la entrada estándar el JSON de la herramienta que Claude va a usar.
# Código de salida 2: la acción se cancela y el mensaje de error llega a Claude.
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

$entrada = [Console]::In.ReadToEnd() | ConvertFrom-Json
$ruta = $entrada.tool_input.file_path

if ($ruta -and (($ruta -replace '\\', '/') -match '(^|/)src/Ayudas\.Web/Legacy/')) {
    [Console]::Error.WriteLine('Bloqueado: src/Ayudas.Web/Legacy/ es el módulo heredado sin mantenimiento. Se puede leer y documentar, pero no se modifica (ver CLAUDE.md).')
    exit 2
}

exit 0
