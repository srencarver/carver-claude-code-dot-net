using Ayudas.Application.Dtos;
using Ayudas.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class InformesController : Controller
{
    private readonly IInformeService _informes;

    public InformesController(IInformeService informes)
    {
        _informes = informes;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var modelo = new InformeResumenDto();
        try
        {
            modelo = await _informes.ObtenerResumenAsync(ct);
        }
        catch
        {
        }

        return View(modelo);
    }
}
