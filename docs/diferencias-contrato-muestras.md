# Diferencias entre el contrato y las muestras

Contrato: `docs/ayudas.xsd` (versión 1.0) y `docs/especificacion-ayudas.pdf`.
Muestras: las 20 remesas de `muestras/`. Validadas contra el XSD, 8 lo cumplen (01, 05, 11, 12, 13, 16, 19 y 20), aunque 12, 13, 16 y 19 incumplen reglas de la especificación que el XSD no recoge.

## Tabla de diferencias

| Muestra | Campo | Valor recibido | Qué pide el contrato | Decisión |
|---|---|---|---|---|
| 02 | NifBeneficiario | `12.345.678-z`, `87654321-x` | 9 caracteres, mayúsculas, sin separadores | Corrige |
| 03 | FechaSolicitud | `01/05/2026`, `15/06/2026` | AAAA-MM-DD | Corrige |
| 03 | FechaSolicitud | `03/07/2026` | Anterior o igual a la fecha de envío (2026-07-01) | Corrige el formato; la rechaza R5 |
| 04 | ImporteSolicitado | `1234,56`, `1.234,56`, `2.500,00 €` | Punto decimal, 2 decimales | Corrige |
| 05 | (fichero) | Codificación ISO-8859-1 declarada | UTF-8 (se admite la declarada) | Se lee bien si se carga como XML y no como texto |
| 06 | NifBeneficiario | ` 11111111h `, `22222222 J` | Sin espacios, mayúsculas | Corrige |
| 07 | FechaSolicitud | `30/02/2026`, `2026-13-01` | Fecha real | Rechaza |
| 07 | FechaSolicitud | `2026-05-12T00:00:00` | AAAA-MM-DD | Corrige (hora 00:00:00) |
| 08 | (solicitud) | Elementos `Telefono` y `Observaciones` | No se admiten elementos fuera del contrato | Se ignoran y se anotan |
| 08 | CodigoMunicipio | `8019` (cero inicial perdido) | 5 dígitos | Rechaza (no se adivina el cero) |
| 09 | NombreBeneficiario | vacío | Obligatorio | Rechaza |
| 09 | Concepto | no viene | Obligatorio | Rechaza |
| 10 | ImporteSolicitado | `-150.00`, `0.00` | Mayor que 0 | Rechaza (R4) |
| 10 | ImporteSolicitado | `950` | 2 decimales | Corrige (950,00) |
| 10 | ImporteSolicitado | `100.555` | 2 decimales | Rechaza (ambiguo) |
| 12 | NifBeneficiario | `12345678A` | Carácter de control correcto | Rechaza |
| 13 | NumeroSolicitudes, ImporteTotal | 5 y un total que no cuadra (hay 4) | Coinciden con el fichero | Aviso, no rechazo |
| 14 | FechaSolicitud | `2026-4-5`, `5/6/2026` | AAAA-MM-DD | Corrige |
| 15 | ImporteSolicitado | `1.234`, `12.500` | Punto decimal | Rechaza (ambiguo: miles o decimales) |
| 16 | FechaSolicitud | `2025-12-20`, `2026-12-31` | Dentro del plazo de la convocatoria | Rechaza (R5) |
| 17 | CodigoMunicipio | ` 01059 ` | 5 dígitos | Corrige (espacios) |
| 18 | NifEntidad | `p0821100e` | Mayúsculas | Corrige |
| 19 | CodigoConvocatoria | `CONV-2026-09` | Convocatoria existente | Rechaza la remesa entera |

## Lo que se corrige

- NIF: espacios, puntos, guiones y minúsculas.
- Fechas: DD/MM/AAAA, sin ceros a la izquierda y con hora 00:00:00.
- Importes: coma decimal, separador de miles, símbolo € y enteros sin decimales.
- Espacios alrededor de cualquier campo.

## Lo que se rechaza

- NIF con el carácter de control incorrecto.
- Fechas que no existen o con hora distinta de 00:00:00.
- Importes ambiguos (`1.234`), con más de dos decimales, cero o negativos.
- Código de municipio que no tiene 5 dígitos.
- Campos obligatorios vacíos o ausentes.
- Fechas fuera del plazo de la convocatoria o posteriores al envío.
- La remesa entera si la entidad o la convocatoria no existen, o si la referencia ya se importó.
