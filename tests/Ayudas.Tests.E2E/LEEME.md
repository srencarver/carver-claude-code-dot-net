# Pruebas de interfaz (Playwright)

Una vez por equipo, para descargar Chromium (necesita PowerShell 7: `winget install Microsoft.PowerShell`):

```powershell
.\scripts\instalar-playwright.ps1
```

Con la web levantada (`dotnet run --project src/Ayudas.Web`):

```powershell
dotnet test tests/Ayudas.Tests.E2E
```

- URL de la web: variable `AYUDAS_URL` (por defecto `http://localhost:5080`; con Docker Compose, `http://localhost:8080`).
- Ver el navegador mientras se ejecuta: `$env:HEADED = "1"` antes de `dotnet test`.
- Las pruebas heredan de `PruebaE2E` (que hereda de `PageTest`): tienen `Page`, `Expect(...)` y `UrlBase`.
- Selectores por rol o etiqueta (`GetByRole`, `GetByLabel`), nunca por CSS; nada de esperas fijas: `Expect(...)` espera solo.
