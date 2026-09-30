using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Servicios;

public interface ICatalogoService
{
    Task<List<Municipio>> ListarMunicipiosAsync(string? provincia, CancellationToken ct = default);
}
