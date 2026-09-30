using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Abstracciones;

public interface IEntidadRepository
{
    Task<List<EntidadLocal>> ListarAsync(CancellationToken ct = default);

    Task<EntidadLocal?> ObtenerAsync(int id, CancellationToken ct = default);

    Task<EntidadLocal?> ObtenerPorNifAsync(string nif, CancellationToken ct = default);

    /// <summary>Guarda los cambios de una entidad ya cargada.</summary>
    Task ActualizarAsync(EntidadLocal entidad, CancellationToken ct = default);
}
