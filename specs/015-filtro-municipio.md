# 015 · Filtro por municipio en el listado de ayudas

- Estado: aprobada
- Autor: equipo de ayudas
- Revisa: responsable técnico
- Work item: 1517

## Contexto

Las diputaciones piden ver solo las ayudas de un municipio para contrastarlas con sus datos.
Hoy el listado de `/Ayudas` no se puede filtrar y hay que exportarlo y filtrar a mano.

## Alcance

- Campo de búsqueda por código INE de municipio en `/Ayudas`.
- El filtro se aplica en la consulta a la base de datos, no en memoria.

## Fuera de alcance

- Filtros por fecha o por importe.
- Buscar por nombre de municipio.

## Criterios de aceptación

- **CA01**: Dado el listado de ayudas, cuando se filtra por un código de municipio, entonces solo aparecen ayudas de ese municipio.
- **CA02**: Dado el listado de ayudas, cuando no se indica municipio, entonces aparecen todas, como hasta ahora.
- **CA03**: Dado un código de municipio sin ayudas (por ejemplo 99999), cuando se filtra, entonces se muestra el mensaje «Sin resultados».
- **CA04**: Dado un filtro activo, cuando se pasa a la página 2, entonces el filtro se conserva.

## Datos de prueba

Los de `DatosSemilla`: el municipio 28079 (Madrid) tiene ayudas en varias páginas.

## Preguntas abiertas

- Ninguna.
