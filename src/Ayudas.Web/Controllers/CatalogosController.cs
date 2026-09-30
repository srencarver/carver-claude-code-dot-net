using Ayudas.Application.Servicios;
using Ayudas.Domain.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class CatalogosController : Controller
{
    private readonly ICatalogoService _catalogos;

    public CatalogosController(ICatalogoService catalogos)
    {
        _catalogos = catalogos;
    }

    public async Task<IActionResult> Municipios(string? provincia, CancellationToken ct)
    {
        ViewBag.Provincia = provincia;
        try
        {
            return View(await _catalogos.ListarMunicipiosAsync(provincia, ct));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(new List<Municipio>());
        }
    }
}
