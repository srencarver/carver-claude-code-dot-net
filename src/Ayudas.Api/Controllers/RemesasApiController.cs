using Ayudas.Application.Dtos;
using Ayudas.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Api.Controllers;

[ApiController]
[Route("api/remesas")]
public class RemesasApiController : ControllerBase
{
    private readonly IRemesaService _remesas;

    public RemesasApiController(IRemesaService remesas)
    {
        _remesas = remesas;
    }

    /// <summary>Detalle de una remesa con sus ayudas y la cofinanciación calculada.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RemesaDetalleDto>> Obtener(int id, CancellationToken ct)
    {
        var remesa = await _remesas.ObtenerDetalleAsync(id, ct);
        if (remesa is null)
        {
            return NotFound();
        }

        return remesa;
    }
}
