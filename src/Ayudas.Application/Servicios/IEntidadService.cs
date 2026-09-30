using Ayudas.Application.Dtos;

namespace Ayudas.Application.Servicios;

public interface IEntidadService
{
    Task<List<EntidadListadoDto>> ListarAsync(CancellationToken ct = default);

    Task<EntidadEdicionDto?> ObtenerParaEdicionAsync(int id, CancellationToken ct = default);

    Task ActualizarAsync(EntidadEdicionDto datos, CancellationToken ct = default);
}
