using Ayudas.Application.Abstracciones;
using Ayudas.Domain.Entidades;

namespace Ayudas.Tests.Unit.Fakes;

public class EntidadRepositoryFalso : IEntidadRepository
{
    public List<EntidadLocal> Entidades { get; } = new();

    public int Guardados { get; private set; }

    public Task<List<EntidadLocal>> ListarAsync(CancellationToken ct = default) =>
        Task.FromResult(Entidades.OrderBy(e => e.Nombre).ToList());

    public Task<EntidadLocal?> ObtenerAsync(int id, CancellationToken ct = default) =>
        Task.FromResult(Entidades.FirstOrDefault(e => e.Id == id));

    public Task<EntidadLocal?> ObtenerPorNifAsync(string nif, CancellationToken ct = default) =>
        Task.FromResult(Entidades.FirstOrDefault(e => e.Nif == nif));

    public Task ActualizarAsync(EntidadLocal entidad, CancellationToken ct = default)
    {
        Guardados++;
        return Task.CompletedTask;
    }
}
