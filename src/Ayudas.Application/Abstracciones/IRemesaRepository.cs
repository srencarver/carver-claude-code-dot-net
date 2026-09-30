using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Abstracciones;

public interface IRemesaRepository
{
    /// <summary>Remesa con su entidad, su convocatoria y sus ayudas.</summary>
    Task<Remesa?> ObtenerConDetalleAsync(int id, CancellationToken ct = default);
}
