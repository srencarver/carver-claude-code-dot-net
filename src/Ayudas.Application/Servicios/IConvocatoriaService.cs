using Ayudas.Application.Dtos;

namespace Ayudas.Application.Servicios;

public interface IConvocatoriaService
{
    Task<List<ConvocatoriaDto>> ListarAsync(CancellationToken ct = default);

    Task<ConvocatoriaDto?> ObtenerAsync(int id, CancellationToken ct = default);

    Task<int> CrearAsync(ConvocatoriaDto datos, CancellationToken ct = default);

    Task ActualizarAsync(ConvocatoriaDto datos, CancellationToken ct = default);
}
