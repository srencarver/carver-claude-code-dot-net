using Ayudas.Application.Excepciones;
using Ayudas.Application.Servicios;
using Ayudas.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class ConvocatoriasController : Controller
{
    private readonly IConvocatoriaService _convocatorias;
    private readonly ILogger<ConvocatoriasController> _logger;

    public ConvocatoriasController(IConvocatoriaService convocatorias, ILogger<ConvocatoriasController> logger)
    {
        _convocatorias = convocatorias;
        _logger = logger;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        return View(await _convocatorias.ListarAsync(ct));
    }

    [HttpGet]
    public IActionResult Crear() => View("Editar", new ConvocatoriaViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ConvocatoriaViewModel modelo, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View("Editar", modelo);
        }

        try
        {
            await _convocatorias.CrearAsync(modelo.ADto(), ct);
            TempData["Mensaje"] = $"Convocatoria {modelo.Codigo} creada.";
            return RedirectToAction(nameof(Index));
        }
        catch (ValidacionException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return View("Editar", modelo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la convocatoria {Codigo}", modelo.Codigo);
            ModelState.AddModelError(string.Empty, "No se ha podido crear la convocatoria.");
            return View("Editar", modelo);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id, CancellationToken ct)
    {
        var convocatoria = await _convocatorias.ObtenerAsync(id, ct);
        if (convocatoria is null)
        {
            return NotFound();
        }

        return View(ConvocatoriaViewModel.DesdeDto(convocatoria));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ConvocatoriaViewModel modelo, CancellationToken ct)
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
            await _convocatorias.ActualizarAsync(modelo.ADto(), ct);
            TempData["Mensaje"] = $"Convocatoria {modelo.Codigo} actualizada.";
            return RedirectToAction(nameof(Index));
        }
        catch (ValidacionException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return View(modelo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la convocatoria {ConvocatoriaId}", modelo.Id);
            ModelState.AddModelError(string.Empty, "No se ha podido guardar la convocatoria.");
            return View(modelo);
        }
    }
}
