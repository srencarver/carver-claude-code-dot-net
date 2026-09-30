# Gestión de ayudas a entidades locales

Aplicación MVC (.NET 8) que recoge las remesas de solicitudes de ayuda que envían las entidades
locales para cada convocatoria. Aplicación de formación: todos los datos son ficticios.

## Comandos

- Compilar: `dotnet build`
- Tests: `dotnet test` (unitarios en `tests/Ayudas.Tests.Unit`, integración con SQLite en memoria en `tests/Ayudas.Tests.Integration`)
- Recrear la base de datos LocalDB con los datos de prueba: `.\scripts\reset-bd.ps1`
- Web: `dotnet run --project src/Ayudas.Web` (http://localhost:5080)
- API: `dotnet run --project src/Ayudas.Api` (http://localhost:5090); peticiones de ejemplo en `src/Ayudas.Api/Ayudas.Api.http`

## Capas

- `Ayudas.Domain`: entidades (`EntidadLocal`, `Convocatoria`, `Remesa`, `Ayuda`, `Justificacion`, `Municipio`) y `Nif`. Sin dependencias.
- `Ayudas.Application`: servicios (`Servicios/`), DTOs (`Dtos/`), interfaces de repositorio (`Abstracciones/`) y excepciones de negocio.
- `Ayudas.Infrastructure`: `AyudasDbContext` (EF Core, SQL Server), configuraciones en `Persistencia/Configuraciones/`, repositorios y datos de prueba (`DatosSemilla`).
- `Ayudas.Web`: controladores MVC y vistas. `Ayudas.Api`: API REST sobre los mismos servicios.
- `Ayudas.Importacion`: importación de remesas en fichero. `Lectores/` (uno por formato, `IAyudasLector`) y `Modelos/` (datos en bruto, como llegan).
- `Ayudas.Infrastructure/Externos/AyudasApiClient`: cliente del Registro de ayudas (API externa). En local la simula `RegistroSimuladoController` de `Ayudas.Api`, que falla una de cada tres peticiones con 503.
- Flujo normal: controlador → servicio (interfaz `IxxxService`) → repositorio (interfaz `IxxxRepository`) → `AyudasDbContext`.

## Convenciones

- Los controladores no usan `AyudasDbContext`: llaman a un servicio de `Ayudas.Application`.
- Los servicios devuelven DTOs, no entidades de dominio.
- Consultas EF: `AsNoTracking()` en lecturas, filtrar y proyectar en la base de datos antes de `ToListAsync()`.
- Todo es asíncrono (`...Async` con `CancellationToken`).
- Errores: `catch` con `ILogger` y mensaje estructurado, como en `EntidadesController`. Nada de `catch` vacíos ni `Console.WriteLine`.
- Errores de negocio: `ValidacionException` (el controlador la pasa al `ModelState`) y `EntidadNoEncontradaException` (404).
- Nombres en español; campos privados con `_`; espacios de nombres de fichero (`namespace X;`).
- Tests: xUnit, nombre `Metodo_Escenario_Resultado`, repositorios falsos en `tests/Ayudas.Tests.Unit/Fakes`.

## Cosas que no son obvias

- No hay migraciones: el esquema se crea con `EnsureCreated`. Si cambias el modelo, ejecuta `reset-bd.ps1`.
- `AuditoriaInterceptor` rellena `FechaModificacion` y `UsuarioModificacion` de `EntidadLocal` al guardar.
- `EntidadMapeos` (Web) limpia el código de municipio antes de llegar al servicio; el servicio limpia y valida el NIF.
- La tabla heredada `JUSTIFICACIONES` usa nombres de columna antiguos (`ID_JUSTIF`, `COD_ESTADO`…).
- Las vistas del módulo heredado están en `Legacy/Justificaciones/Views/` (ruta añadida en `Program.cs`).

## Importación de remesas

- Contrato: `docs/ayudas.xsd` y `docs/especificacion-ayudas.pdf`. Las entidades no siempre lo cumplen.
- Muestras sintéticas en `muestras/`: son la referencia de lo que llega de verdad.

## Especificaciones

- Los cambios medianos o grandes empiezan por una spec en `specs/NNN-nombre.md` con la plantilla `specs/_plantilla.md`.
- Plan y tareas junto a la spec: `NNN-nombre.plan.md` y `NNN-nombre.tasks.md`. Un commit por tarea.

## No tocar

- `src/Ayudas.Web/Legacy/`: módulo heredado sin mantenimiento. Se puede leer y documentar, no modificar.
- `docs/antiguo/`: documentación antigua; puede no ser correcta.

## Datos

- Nunca datos reales: NIF, nombres y direcciones de prueba, como los de `DatosSemilla`.
- Nada de cadenas de conexión, contraseñas ni tokens en el código ni en este fichero.
