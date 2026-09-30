---
name: importador-xml
description: Crea importadores XML nuevos de ayudas; úsala con formatos aún no soportados.
argument-hint: <fichero de muestra del formato nuevo>
---

# Nuevo lector de remesas XML

Procedimiento del equipo para que el importador acepte un formato XML que todavía no entiende.
Muestra del formato: $ARGUMENTS

## Pasos

1. Lee la muestra y compárala con `docs/ayudas.xsd`: qué elemento o atributo corresponde a cada campo de `CabeceraRemesa` y de `AyudaEntrada`, y qué falta. Enséñame la tabla antes de escribir código.
2. Crea `src/Ayudas.Importacion/Lectores/<Nombre>XmlReader.cs` implementando `IAyudasLector`, con `AyudasXmlReader` como modelo:
   - `Soporta` mira solo el nombre del elemento raíz.
   - `Leer` copia los valores como texto, sin limpiar ni validar: de eso se encargan los normalizadores y `AyudaEntradaValidator`.
   - `Fila` empieza en 1, en el orden del fichero.
   - Lo que el formato no trae se deja a `null` (por ejemplo, `ImporteTotal`).
3. Regístralo en `src/Ayudas.Importacion/DependencyInjection.cs` junto a los demás lectores.
4. Tests en `tests/Ayudas.Tests.Unit/Importacion/<Nombre>XmlReaderTests.cs`: lectura de la cabecera, de una solicitud y `Soporta` con otro formato. Añade un caso a `ImportadorServiceTests` que importe la muestra de principio a fin.
5. Ejecuta `dotnet test`.

## Reglas

- No cambies los normalizadores ni el validador para adaptarlos a un formato: si hace falta, avisa.
- Muestras solo sintéticas, en `muestras/`.

## Comprobación final

- `dotnet test` en verde.
- La muestra se importa con el recuento de aceptadas y rechazadas que esperabas en el paso 1.
