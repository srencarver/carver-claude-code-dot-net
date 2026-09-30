using Ayudas.Application.Servicios;
using Ayudas.Infrastructure.Persistencia;
using Ayudas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Web.Controllers;

public class RemesasController : Controller
{
    private readonly AyudasDbContext _db;
    private readonly IRemesaService _remesas;
    private readonly IEntidadService _entidades;

    public RemesasController(AyudasDbContext db, IRemesaService remesas, IEntidadService entidades)
    {
        _db = db;
        _remesas = remesas;
        _entidades = entidades;
    }

    public async Task<IActionResult> Index(int? entidadId, int pagina = 1, CancellationToken ct = default)
    {
        var modelo = new RemesasIndexViewModel
        {
            Resultado = await _remesas.ListarPaginadoAsync(entidadId, pagina, ct),
            EntidadId = entidadId,
            Entidades = await _entidades.ListarAsync(ct)
        };

        return View(modelo);
    }

    public IActionResult Detalle(int id)
    {
        var remesa = _db.Remesas
            .Include(r => r.EntidadLocal)
            .Include(r => r.Convocatoria)
            .Include(r => r.Ayudas)
            .FirstOrDefault(r => r.Id == id);

        if (remesa == null)
        {
            return NotFound();
        }

        return View(remesa);
    }
}
