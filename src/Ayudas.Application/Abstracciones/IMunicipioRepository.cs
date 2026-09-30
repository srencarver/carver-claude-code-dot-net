using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Abstracciones;

public interface IMunicipioRepository
{
    Task<List<Municipio>> ListarAsync(string? provincia, CancellationToken ct = default);
}
