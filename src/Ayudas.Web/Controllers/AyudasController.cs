using Ayudas.Application.Dtos;
using Ayudas.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class AyudasController : Controller
{
    private readonly IAyudaService _ayudas;

    public AyudasController(IAyudaService ayudas)
    {
        _ayudas = ayudas;
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
            Console.WriteLine("Error en ayudas: " + ex.Message);
            return View(new PaginaResultado<AyudaListadoDto>(new List<AyudaListadoDto>(), 1, AyudaService.TamanoPagina, 0));
        }
    }
}
