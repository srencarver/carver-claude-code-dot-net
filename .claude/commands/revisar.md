---
description: Revisa cambios con el checklist del equipo y devuelve hallazgos por fichero y línea
argument-hint: [rama, commit o fichero .diff; vacío = cambios sin confirmar]
disable-model-invocation: true
allowed-tools: Read, Grep, Glob, Bash(git diff *), Bash(git log *), PowerShell(git diff *), PowerShell(git log *)
---
Revisa los cambios de: $ARGUMENTS
Si no hay argumento, revisa los cambios sin confirmar (git diff y git diff --staged).
Si es una rama o un commit, compáralo con main (git diff main...$ARGUMENTS) o con su padre.
Si es un fichero .diff, léelo.

No edites nada. Solo revisa.

## Checklist del equipo

1. Capas: los controladores no usan AyudasDbContext; los servicios devuelven DTOs.
2. Consultas EF: AsNoTracking en lecturas; filtrar, contar y proyectar en la base de datos (nada de ToList() antes del Where o del Count).
3. Asíncrono con CancellationToken de punta a punta.
4. Errores: ILogger con mensaje estructurado; ningún catch vacío ni Console.WriteLine.
5. Pruebas: cada regla nueva tiene su test (Metodo_Escenario_Resultado); ningún test existente debilitado, omitido o borrado.
6. Cambios no pedidos: configuración, formato masivo, renombrados, Legacy/ o docs/antiguo/.
7. Datos y secretos: nada de cadenas de conexión, claves ni datos personales reales; DatosSemilla sin tocar salvo que se pida.
8. Valores fijos: URLs, importes o porcentajes que deberían ser constantes con nombre o configuración.

## Formato

Tabla con: fichero · línea · hallazgo · gravedad (alta, media, baja) · regla del checklist.
Al final, una línea con tu valoración: «se puede fusionar», «con cambios» o «no se puede fusionar».
Si no hay hallazgos, dilo; no rellenes.
