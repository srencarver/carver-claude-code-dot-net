# Recorrido de /entidades/editar

Qué ficheros intervienen cuando se edita una entidad local, de la petición a la tabla.
Líneas verificadas en la rama `s2-inicio`.

| Paso | Capa | Fichero y línea | Qué hace |
|---|---|---|---|
| 1 | Vista | `src/Ayudas.Web/Views/Entidades/Index.cshtml:33` | Enlace «Editar» a `/Entidades/Editar/{id}` |
| 2 | Controlador | `src/Ayudas.Web/Controllers/EntidadesController.cs:36` | `Editar` (GET): pide la entidad al servicio |
| 3 | Mapeo | `src/Ayudas.Web/Mapeos/EntidadMapeos.cs:11` | `AViewModel`: DTO → modelo de la vista |
| 4 | Vista | `src/Ayudas.Web/Views/Entidades/Editar.cshtml:8` | Formulario POST con antiforgery |
| 5 | Controlador | `src/Ayudas.Web/Controllers/EntidadesController.cs:49` | `Editar` (POST): valida el `ModelState` y llama al servicio (línea 63) |
| 6 | Mapeo | `src/Ayudas.Web/Mapeos/EntidadMapeos.cs:26` | `ADto`: deja solo los dígitos del código de municipio (línea 33) |
| 7 | Servicio | `src/Ayudas.Application/Servicios/EntidadService.cs:58` | `ActualizarAsync`: carga la entidad, limpia y valida el NIF, comprueba duplicados |
| 8 | Dominio | `src/Ayudas.Domain/Comun/Nif.cs:13` | `Nif.EsValido`: DNI, NIE y NIF de organización |
| 9 | Repositorio | `src/Ayudas.Infrastructure/Repositorios/EntidadRepository.cs:31` | `ActualizarAsync`: `SaveChangesAsync` (línea 38) |
| 10 | Persistencia | `src/Ayudas.Infrastructure/Persistencia/AuditoriaInterceptor.cs:18` | Antes de guardar, rellena `FechaModificacion` y `UsuarioModificacion` |
| 11 | Tabla | `src/Ayudas.Infrastructure/Persistencia/Configuraciones/EntidadLocalConfiguration.cs:11` | Tabla `EntidadesLocales` |

## Saltos de capa

- Web → Application: `EntidadesController` depende de `IEntidadService` (registro en `src/Ayudas.Application/DependencyInjection.cs:10`).
- Application → Infrastructure: `EntidadService` depende de `IEntidadRepository`, implementada por `EntidadRepository` (registro en `src/Ayudas.Infrastructure/DependencyInjection.cs:20`).
- Infrastructure → EF Core: el interceptor se engancha al contexto en `src/Ayudas.Infrastructure/DependencyInjection.cs:18`.

Los dos pasos que no se ven leyendo solo el controlador y el servicio: el mapeo del paso 6,
que modifica el dato, y el interceptor del paso 10, que escribe columnas que nadie asigna en el servicio.
