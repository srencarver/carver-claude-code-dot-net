using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Api.Controllers;

/// <summary>
/// Simulación del Registro de ayudas (servicio externo) para probar la integración en local.
/// Una de cada tres peticiones responde 503, como un servicio que falla de forma puntual.
/// </summary>
[ApiController]
[Route("api/registro")]
public class RegistroSimuladoController : ControllerBase
{
    private static int _peticiones;
    private static int _registros;

    [HttpGet("convocatorias/{codigo}")]
    public IActionResult ObtenerConvocatoria(string codigo)
    {
        if (FalloTransitorio())
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        if (!codigo.StartsWith("CONV-", StringComparison.Ordinal))
        {
            return NotFound();
        }

        return Ok(new { codigo, abierta = true, importeDisponible = 250000.00m });
    }

    [HttpPost("remesas")]
    public IActionResult RegistrarRemesa(RemesaRegistroSimulada remesa)
    {
        if (FalloTransitorio())
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        if (string.IsNullOrWhiteSpace(remesa.Referencia) || remesa.NumeroSolicitudes <= 0)
        {
            return BadRequest(new { error = "Remesa incompleta" });
        }

        var numero = Interlocked.Increment(ref _registros);
        return Ok(new { numeroRegistro = $"RA-{DateTime.Today.Year}-{numero:D6}", fechaRegistro = DateTime.Now });
    }

    private static bool FalloTransitorio() => Interlocked.Increment(ref _peticiones) % 3 == 0;
}

public record RemesaRegistroSimulada(string Referencia, string NifEntidad, string CodigoConvocatoria, int NumeroSolicitudes, decimal ImporteTotal);
