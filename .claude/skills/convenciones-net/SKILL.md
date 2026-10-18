---
name: convenciones-net
description: Convenciones de la solución Ayudas para crear o modificar controladores MVC, endpoints de la API, servicios, DTOs, repositorios y consultas EF Core. Úsala al añadir una pantalla, un endpoint, una consulta o un servicio nuevo, o al cambiar uno existente.
---

# Convenciones .NET del equipo (solución Ayudas)

## Al añadir o cambiar una pantalla o un endpoint

1. Controlador fino: recibe, llama a un servicio (`IxxxService` de `Ayudas.Application`) y devuelve la vista o el resultado. Nunca usa `AyudasDbContext`.
2. Servicio en `src/Ayudas.Application/Servicios/` con su interfaz; se registra en `src/Ayudas.Application/DependencyInjection.cs`.
3. El servicio devuelve DTOs de `src/Ayudas.Application/Dtos/`, no entidades de dominio.
4. Acceso a datos en un repositorio de `src/Ayudas.Infrastructure/Repositorios/` con su interfaz en `Abstracciones/`; se registra en `src/Ayudas.Infrastructure/DependencyInjection.cs`.
5. Consultas EF: `AsNoTracking()` en lecturas; filtrar, contar y proyectar en la base de datos antes de `ToListAsync()`.
6. Todo asíncrono, con `CancellationToken` desde el controlador hasta el repositorio.
7. Errores de negocio: `ValidacionException` (el controlador la pasa al `ModelState`) y `EntidadNoEncontradaException` (404). Errores técnicos: `catch` con `ILogger` y mensaje estructurado, como en `EntidadesController`.
8. Vistas Razor con el layout común y los tag helpers (`asp-for`, `asp-action`); sin Bootstrap ni CDN: estilos en `wwwroot/css/site.css`.
9. Nombres en español; campos privados con `_`; espacio de nombres de fichero (`namespace X;`).

## Pruebas

- Una prueba por regla en `tests/Ayudas.Tests.Unit`, con los repositorios falsos de `Fakes/`.
- Si la consulta es nueva, una prueba de integración en `tests/Ayudas.Tests.Integration` con `BaseDatosPruebas` (SQLite en memoria, nunca EF InMemory).

## Comprobación final

- `dotnet build` sin errores y `dotnet test` en verde.
- Ningún controlador nuevo inyecta `AyudasDbContext`.
