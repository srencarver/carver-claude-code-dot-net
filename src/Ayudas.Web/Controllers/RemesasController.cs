using Ayudas.Application.Servicios;
using Ayudas.Domain.Entidades;
using Ayudas.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class RemesasController : Controller
{
    private readonly IRemesaService _remesas;
    private readonly IEntidadService _entidades;

    public RemesasController(IRemesaService remesas, IEntidadService entidades)
    {
        _remesas = remesas;
        _entidades = entidades;
    }

    public async Task<IActionResult> Index(int? entidadId, EstadoRemesa? estado, int pagina = 1, CancellationToken ct = default)
    {
        var modelo = new RemesasIndexViewModel
        {
            Resultado = await _remesas.ListarPaginadoAsync(entidadId, estado, pagina, ct),
            EntidadId = entidadId,
            Estado = estado,
            Entidades = await _entidades.ListarAsync(ct)
        };

        return View(modelo);
    }

    public async Task<IActionResult> Detalle(int id, CancellationToken ct)
    {
        var remesa = await _remesas.ObtenerDetalleAsync(id, ct);
        if (remesa is null)
        {
            return NotFound();
        }

        return View(remesa);
    }
}
