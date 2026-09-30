using Ayudas.Application.Abstracciones;
using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Servicios;

public class CatalogoService : ICatalogoService
{
    private readonly IMunicipioRepository _municipios;

    public CatalogoService(IMunicipioRepository municipios)
    {
        _municipios = municipios;
    }

    public Task<List<Municipio>> ListarMunicipiosAsync(string? provincia, CancellationToken ct = default) =>
        _municipios.ListarAsync(string.IsNullOrWhiteSpace(provincia) ? null : provincia.Trim(), ct);
}
