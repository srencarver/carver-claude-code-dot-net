using Ayudas.Application.Dtos;
using Ayudas.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class InformesController : Controller
{
    private readonly IInformeService _informes;
    private readonly ILogger<InformesController> _logger;

    public InformesController(IInformeService informes, ILogger<InformesController> logger)
    {
        _informes = informes;
        _logger = logger;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var modelo = new InformeResumenDto();
        try
        {
            modelo = await _informes.ObtenerResumenAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el resumen de informes");
            TempData["Error"] = "No se ha podido cargar el informe.";
        }

        return View(modelo);
    }
}
