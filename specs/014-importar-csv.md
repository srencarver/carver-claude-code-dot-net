# 014 · Importar remesas en CSV

- Estado: aprobada
- Autor: equipo de ayudas
- Revisa: responsable técnico
- Work item: 1402

## Contexto

Varios ayuntamientos pequeños no pueden generar el XML del contrato y envían sus remesas en una
hoja de cálculo exportada a CSV. Hoy se pasan a mano al formato XML.

## Alcance

- Un lector nuevo para CSV separado por punto y coma, con una fila de cabecera de columnas.
- Los datos de la remesa (referencia, NIF de la entidad, convocatoria, fecha de envío) se piden en la pantalla de importación, porque el CSV no los trae.
- Mismas reglas de validación que el XML (R1 a R5 de la especificación funcional).

## Fuera de alcance

- Ficheros Excel (.xlsx).
- Cambios en los normalizadores o en el validador.

## Criterios de aceptación

- **CA01**: Dado un CSV con las columnas `nif;nombre;municipio;concepto;importe;fecha`, cuando se importa, entonces cada fila es una solicitud y la fila 1 de datos es la línea 1 del informe.
- **CA02**: Dado un CSV con las columnas en otro orden, cuando se importa, entonces se leen por el nombre de la columna.
- **CA03**: Dado un CSV al que le falta una columna obligatoria, cuando se importa, entonces la remesa entera se rechaza indicando la columna que falta.
- **CA04**: Dado un CSV en ISO-8859-1 con acentos, cuando se importa, entonces los nombres se leen sin caracteres extraños.

## Datos de prueba

`muestras/csv/` (por crear): una remesa correcta, una con columnas desordenadas y otra sin la columna `importe`.

## Preguntas abiertas

- Ninguna.
