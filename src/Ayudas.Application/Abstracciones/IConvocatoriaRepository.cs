using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Abstracciones;

public interface IConvocatoriaRepository
{
    Task<List<Convocatoria>> ListarAsync(CancellationToken ct = default);

    Task<Convocatoria?> ObtenerAsync(int id, CancellationToken ct = default);

    Task<Convocatoria?> ObtenerPorCodigoAsync(string codigo, CancellationToken ct = default);

    Task AgregarAsync(Convocatoria convocatoria, CancellationToken ct = default);

    /// <summary>Guarda los cambios de una convocatoria ya cargada.</summary>
    Task ActualizarAsync(Convocatoria convocatoria, CancellationToken ct = default);
}
