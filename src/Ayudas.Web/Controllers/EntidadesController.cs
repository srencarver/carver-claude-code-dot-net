using Ayudas.Application.Excepciones;
using Ayudas.Application.Servicios;
using Ayudas.Web.Mapeos;
using Ayudas.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class EntidadesController : Controller
{
    private readonly IEntidadService _entidades;
    private readonly ILogger<EntidadesController> _logger;

    public EntidadesController(IEntidadService entidades, ILogger<EntidadesController> logger)
    {
        _entidades = entidades;
        _logger = logger;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        try
        {
            var entidades = await _entidades.ListarAsync(ct);
            return View(entidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar las entidades locales");
            TempData["Error"] = "No se ha podido cargar el listado de entidades.";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id, CancellationToken ct)
    {
        var entidad = await _entidades.ObtenerParaEdicionAsync(id, ct);
        if (entidad is null)
        {
            return NotFound();
        }

        return View(entidad.AViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, EntidadEdicionViewModel modelo, CancellationToken ct)
    {
        if (id != modelo.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        try
        {
            await _entidades.ActualizarAsync(modelo.ADto(), ct);
            TempData["Mensaje"] = $"Entidad «{modelo.Nombre}» actualizada.";
            return RedirectToAction(nameof(Index));
        }
        catch (EntidadNoEncontradaException)
        {
            return NotFound();
        }
        catch (ValidacionException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return View(modelo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la entidad local {EntidadId}", modelo.Id);
            ModelState.AddModelError(string.Empty, "No se ha podido guardar la entidad. Inténtalo de nuevo.");
            return View(modelo);
        }
    }
}
