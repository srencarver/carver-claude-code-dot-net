---
name: documentar-modulo
description: Documenta un módulo o carpeta de la solución contrastando cada afirmación con el código. Úsala cuando pidan documentar, describir o actualizar la documentación de un módulo.
argument-hint: <ruta o nombre del módulo>
---

# Documentar un módulo

Documenta el módulo $ARGUMENTS.

## Pasos

1. Inventario: lista los ficheros del módulo y la documentación que ya exista sobre él (en `docs/` y junto al código).
2. Entradas: rutas, acciones de controlador o métodos públicos por los que se entra al módulo.
3. Lógica: reglas de negocio, con fichero y línea de cada una.
4. Datos: tablas y columnas que lee o escribe, y cómo accede (EF Core, ADO.NET…).
5. Impacto: busca en toda la solución quién más usa sus entidades o tablas; tabla con capa, fichero:línea y uso.
6. Contraste: compara la documentación existente con el código y lista lo que ya no es verdad.
7. Escribe `docs/<modulo>.md` con estos apartados y un `flowchart` de Mermaid que solo nombre ficheros o clases que existen.

## Reglas

- Cada afirmación lleva ruta y línea. Lo que no hayas podido comprobar va en un apartado «Sin verificar».
- No modifiques el código del módulo; solo documentación.
- Un fichero por petición: primero el inventario y la tabla de impacto, después el documento.

## Comprobación final

- Todos los ficheros citados existen (compruébalo con una búsqueda).
- El diagrama se pinta en la vista previa de Mermaid.
