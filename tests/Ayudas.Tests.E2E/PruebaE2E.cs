using Microsoft.Playwright.Xunit;

namespace Ayudas.Tests.E2E;

/// <summary>
/// Base de las pruebas de interfaz. La web tiene que estar levantada antes de ejecutarlas
/// (dotnet run --project src/Ayudas.Web o docker compose up -d).
/// </summary>
public abstract class PruebaE2E : PageTest
{
    /// <summary>URL de la web: variable AYUDAS_URL; por defecto, la de dotnet run.</summary>
    protected static string UrlBase =>
        (Environment.GetEnvironmentVariable("AYUDAS_URL") ?? "http://localhost:5080").TrimEnd('/');
}
