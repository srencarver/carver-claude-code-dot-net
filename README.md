# Gestión de ayudas a entidades locales · aplicación del curso

Aplicación de formación del curso **Claude Code para .NET**. Es una aplicación MVC que recoge las
remesas de solicitudes de ayuda que envían las entidades locales para cada convocatoria.

Todos los datos son ficticios.

## Requisitos

- SDK de .NET 8.
- SQL Server Express LocalDB (se instala con Visual Studio 2022, carga de trabajo «Almacenamiento y procesamiento de datos»).
- Visual Studio 2022 o VS Code, y Claude Code.

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

Cada sesión empieza en su rama: `main` para la sesión 1 y `s2-inicio` … `s6-inicio` para las siguientes.

```powershell
# Al empezar una sesión (cambia de rama, actualiza y recrea la base de datos)
.\scripts\empezar-sesion.ps1 -Sesion 2

# Al terminar una sesión (guarda tu trabajo en una rama tuya)
.\scripts\guardar-trabajo.ps1 -Sesion 2
```
