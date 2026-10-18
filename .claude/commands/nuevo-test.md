---
description: Crea un test xUnit con las convenciones del proyecto para el método indicado
argument-hint: <Clase.Metodo> <escenario>
disable-model-invocation: true
---
Escribe un test xUnit para $ARGUMENTS.

- Ubicación: tests/Ayudas.Tests.Unit, en la carpeta de la capa que corresponda (Servicios/, Dominio/, Importacion/).
- Nombre: Metodo_Escenario_Resultado.
- Usa los repositorios falsos y los datos de tests/Ayudas.Tests.Unit/Fakes; nada de librerías de mocks nuevas.
- Arrange / Act / Assert, con aserciones sobre valores, no solo «no lanza excepción».
- Ejecuta solo ese test (dotnet test --filter "FullyQualifiedName~<NombreDelTest>") y enséñame el resultado.
