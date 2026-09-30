using Ayudas.Application.Servicios;
using Ayudas.Domain.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class CatalogosController : Controller
{
    private readonly ICatalogoService _catalogos;
    private readonly ILogger<CatalogosController> _logger;

    public CatalogosController(ICatalogoService catalogos, ILogger<CatalogosController> logger)
    {
        _catalogos = catalogos;
        _logger = logger;
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
            _logger.LogError(ex, "Error al listar los municipios de la provincia {Provincia}", provincia);
            TempData["Error"] = "No se ha podido cargar el catálogo de municipios.";
            return View(new List<Municipio>());
        }
    }
}
