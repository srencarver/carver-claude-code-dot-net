using Ayudas.Application.Dtos;
using Ayudas.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Api.Controllers;

[ApiController]
[Route("api/entidades")]
public class EntidadesApiController : ControllerBase
{
    private readonly IEntidadService _entidades;

    public EntidadesApiController(IEntidadService entidades)
    {
        _entidades = entidades;
    }

    [HttpGet]
    public Task<List<EntidadListadoDto>> Listar(CancellationToken ct) => _entidades.ListarAsync(ct);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EntidadEdicionDto>> Obtener(int id, CancellationToken ct)
    {
        var entidad = await _entidades.ObtenerParaEdicionAsync(id, ct);
        if (entidad is null)
        {
            return NotFound();
        }

        return entidad;
    }
}
