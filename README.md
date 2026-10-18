# Gestión de ayudas a entidades locales · aplicación del curso

Aplicación de formación del curso **Claude Code para .NET**. Es una aplicación MVC que recoge las
remesas de solicitudes de ayuda que envían las entidades locales para cada convocatoria.

Todos los datos son ficticios.

## Requisitos

- SDK de .NET 8.
- SQL Server Express LocalDB (se instala con Visual Studio 2022, carga de trabajo «Almacenamiento y procesamiento de datos»).
- Visual Studio 2022 o VS Code, y Claude Code.
- Sesión 4: Node.js 18 o superior (servidor MCP de Playwright), PowerShell 7 (`winget install Microsoft.PowerShell`, para descargar Chromium) y Docker.

## Primer arranque

```powershell
# Crea la base de datos AyudasFormacion en LocalDB con los datos de prueba
.\scripts\reset-bd.ps1

# Arranca la web (http://localhost:5080)
dotnet run --project src/Ayudas.Web

# Arranca la API (http://localhost:5090), en otro terminal
dotnet run --project src/Ayudas.Api
```

La base de datos se crea sola al arrancar si no existe. No hay migraciones: si el esquema cambia
entre sesiones, `reset-bd.ps1` la vuelve a crear.

## Pruebas

```powershell
dotnet test
```

## Ramas del curso

El curso tiene cinco sesiones de 3 h. Cada sesión empieza en su rama: `main` para la sesión 1 y
`s2-inicio` … `s5-inicio` para las siguientes. Cada rama trae la solución de la sesión anterior.

| Sesión | Rama | Bloques |
|---|---|---|
| 1 | `main` | 01 Flujo de trabajo · 02 CLAUDE.md y memoria |
| 2 | `s2-inicio` | 02 Permisos y datos sensibles · 03 Contexto y tokens · 04 Comandos |
| 3 | `s3-inicio` | 04 Skills, subagentes y hooks · 05 Especificación, plan y pruebas |
| 4 | `s4-inicio` | 05 Playwright, diff y Git · 06 Docker |
| 5 | `s5-inicio` | 07 Taller integrador |

```powershell
# Al empezar una sesión (cambia de rama, actualiza y recrea la base de datos)
.\scripts\empezar-sesion.ps1 -Sesion 2

# Al terminar una sesión (guarda tu trabajo en una rama tuya)
.\scripts\guardar-trabajo.ps1 -Sesion 2
```
