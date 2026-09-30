using Ayudas.Infrastructure.Persistencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Web.Controllers;

public class RemesasController : Controller
{
    private readonly AyudasDbContext _db;

    public RemesasController(AyudasDbContext db)
    {
        _db = db;
    }

    public IActionResult Index(int? entidadId, int pagina = 1)
    {
        const int tamanoPagina = 10;

        var remesas = _db.Remesas
            .Include(r => r.EntidadLocal)
            .Include(r => r.Convocatoria)
            .Include(r => r.Ayudas)
            .ToList();

        var filtradas = remesas
            .Where(r => entidadId == null || r.EntidadLocalId == entidadId)
            .OrderByDescending(r => r.FechaEnvio)
            .ToList();

        if (pagina < 1)
        {
            pagina = 1;
        }

        ViewBag.Pagina = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling(filtradas.Count / (double)tamanoPagina);
        ViewBag.EntidadId = entidadId;
        ViewBag.Entidades = _db.EntidadesLocales.OrderBy(e => e.Nombre).ToList();

        var paginaActual = filtradas
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToList();

        return View(paginaActual);
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
