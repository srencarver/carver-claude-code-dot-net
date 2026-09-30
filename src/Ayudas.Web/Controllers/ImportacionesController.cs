using System.Text;
using Ayudas.Importacion;
using Ayudas.Importacion.Informes;
using Ayudas.Infrastructure.Externos;
using Ayudas.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Controllers;

public class ImportacionesController : Controller
{
    private const string ClaveInforme = "InformeRechazosCsv";

    private readonly ImportadorService _importador;
    private readonly AyudasApiClient _registro;
    private readonly ILogger<ImportacionesController> _logger;

    public ImportacionesController(ImportadorService importador, AyudasApiClient registro, ILogger<ImportacionesController> logger)
    {
        _importador = importador;
        _registro = registro;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Importar(IFormFile? fichero, CancellationToken ct)
    {
        if (fichero is null || fichero.Length == 0)
        {
            ModelState.AddModelError(nameof(fichero), "Selecciona un fichero de remesa.");
            return View(nameof(Index));
        }

        ResultadoImportacion resultado;
        await using (var contenido = fichero.OpenReadStream())
        {
            resultado = await _importador.ImportarAsync(contenido, ct);
        }

        var modelo = new ImportacionViewModel { NombreFichero = fichero.FileName, Resultado = resultado };

        if (resultado.RemesaId is not null)
        {
            await RegistrarAsync(modelo, ct);
        }

        TempData[ClaveInforme] = InformeRechazosCsv.Generar(resultado.Resultados);
        return View("Resultado", modelo);
    }

    [HttpGet]
    public IActionResult DescargarInforme()
    {
        if (TempData[ClaveInforme] is not string csv)
        {
            return RedirectToAction(nameof(Index));
        }

        var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray();
        return File(bytes, "text/csv", "rechazos.csv");
    }

    private async Task RegistrarAsync(ImportacionViewModel modelo, CancellationToken ct)
    {
        var resultado = modelo.Resultado;
        try
        {
            var acuse = await _registro.RegistrarRemesaAsync(
                new RemesaRegistro(resultado.Referencia!, resultado.NifEntidad!, resultado.CodigoConvocatoria!, resultado.Aceptadas, resultado.ImporteAceptado), ct);
            modelo.NumeroRegistro = acuse.NumeroRegistro;
        }
        catch (Exception ex) when (ex is not OperationCanceledException || !ct.IsCancellationRequested)
        {
            _logger.LogError(ex, "Error al registrar la remesa {Referencia} en el Registro de ayudas", resultado.Referencia);
            modelo.AvisoRegistro = "La remesa se ha importado, pero no se ha podido comunicar al Registro de ayudas. Se reintentará más tarde.";
        }
    }
}
