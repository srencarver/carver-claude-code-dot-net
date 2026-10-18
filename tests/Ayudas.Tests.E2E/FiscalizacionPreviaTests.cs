using System.Globalization;
using Microsoft.Playwright;

namespace Ayudas.Tests.E2E;

/// <summary>
/// Regla de la práctica 05: las ayudas con importe concedido de 10.000 € o más se marcan con
/// «Fiscalización previa» en el detalle de la remesa, y las demás no.
/// Recorre las remesas del listado hasta encontrar alguna ayuda que supere el umbral.
/// </summary>
public class FiscalizacionPreviaTests : PruebaE2E
{
    private const decimal Umbral = 10_000m;
    private static readonly CultureInfo Es = CultureInfo.GetCultureInfo("es-ES");

    [Fact]
    public async Task DetalleRemesa_AyudasDesdeElUmbral_MarcanFiscalizacionPrevia()
    {
        var ayudasPorEncima = 0;

        for (var pagina = 1; pagina <= 5 && ayudasPorEncima == 0; pagina++)
        {
            await Page.GotoAsync($"{UrlBase}/Remesas?pagina={pagina}");

            var enlaces = await Page.GetByRole(AriaRole.Link, new() { Name = "Ver", Exact = true }).AllAsync();
            var detalles = new List<string>();
            foreach (var enlace in enlaces)
            {
                detalles.Add(await enlace.GetAttributeAsync("href") ?? string.Empty);
            }

            foreach (var detalle in detalles.Where(d => d.Length > 0))
            {
                await Page.GotoAsync(UrlBase + detalle);

                // La primera fila es la cabecera de la tabla de ayudas.
                var filas = await Page.GetByRole(AriaRole.Table).GetByRole(AriaRole.Row).AllAsync();
                foreach (var fila in filas.Skip(1))
                {
                    var celdas = fila.GetByRole(AriaRole.Cell);
                    var concedido = decimal.Parse(await celdas.Nth(5).InnerTextAsync(), NumberStyles.Number, Es);
                    var marca = (await celdas.Nth(6).InnerTextAsync()).Trim();

                    if (concedido >= Umbral)
                    {
                        Assert.Equal("Fiscalización previa", marca);
                        ayudasPorEncima++;
                    }
                    else
                    {
                        Assert.Equal(string.Empty, marca);
                    }
                }
            }
        }

        Assert.True(ayudasPorEncima > 0, "No se ha encontrado ninguna ayuda de 10.000 € o más en las primeras páginas del listado.");
    }
}
