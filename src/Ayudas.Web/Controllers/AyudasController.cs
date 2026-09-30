using Ayudas.Application.Dtos;
using Ayudas.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class AyudasController : Controller
{
    private readonly IAyudaService _ayudas;
    private readonly ILogger<AyudasController> _logger;

    public AyudasController(IAyudaService ayudas, ILogger<AyudasController> logger)
    {
        _ayudas = ayudas;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int pagina = 1, CancellationToken ct = default)
    {
        try
        {
            var resultado = await _ayudas.ListarAsync(pagina, ct);
            return View(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar las ayudas (página {Pagina})", pagina);
            TempData["Error"] = "No se ha podido cargar el listado de ayudas.";
            return View(new PaginaResultado<AyudaListadoDto>(new List<AyudaListadoDto>(), 1, AyudaService.TamanoPagina, 0));
        }
    }
}
