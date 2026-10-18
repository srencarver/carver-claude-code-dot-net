using Microsoft.Playwright;

namespace Ayudas.Tests.E2E;

/// <summary>
/// Regla de la demo 13 (sesión 3): una entidad activa tiene que tener dirección de notificación.
/// El Ayuntamiento de Granada está activo y sin dirección en los datos semilla: el guardado falla
/// y la prueba no cambia nada en la base de datos.
/// </summary>
public class EntidadDireccionTests : PruebaE2E
{
    [Fact]
    public async Task Editar_EntidadActivaSinDireccion_MuestraErrorEnElCampo()
    {
        await Page.GotoAsync($"{UrlBase}/Entidades");

        await Page.GetByRole(AriaRole.Row, new() { Name = "Ayuntamiento de Granada" })
            .GetByRole(AriaRole.Link, new() { Name = "Editar" })
            .ClickAsync();

        await Page.GetByLabel("Dirección de notificación").FillAsync("");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar" }).ClickAsync();

        await Expect(Page.GetByText("La dirección de notificación es obligatoria para entidades activas.")).ToBeVisibleAsync();
    }
}
