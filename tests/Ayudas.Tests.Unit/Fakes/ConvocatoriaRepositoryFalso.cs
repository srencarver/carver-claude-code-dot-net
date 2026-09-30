using Ayudas.Application.Abstracciones;
using Ayudas.Domain.Entidades;

namespace Ayudas.Tests.Unit.Fakes;

public class ConvocatoriaRepositoryFalso : IConvocatoriaRepository
{
    public List<Convocatoria> Convocatorias { get; } = new();

    public Task<List<Convocatoria>> ListarAsync(CancellationToken ct = default) => Task.FromResult(Convocatorias.ToList());

    public Task<Convocatoria?> ObtenerAsync(int id, CancellationToken ct = default) =>
        Task.FromResult(Convocatorias.FirstOrDefault(c => c.Id == id));

    public Task<Convocatoria?> ObtenerPorCodigoAsync(string codigo, CancellationToken ct = default) =>
        Task.FromResult(Convocatorias.FirstOrDefault(c => c.Codigo == codigo));

    public Task AgregarAsync(Convocatoria convocatoria, CancellationToken ct = default)
    {
        convocatoria.Id = Convocatorias.Count + 1;
        Convocatorias.Add(convocatoria);
        return Task.CompletedTask;
    }

    public Task ActualizarAsync(Convocatoria convocatoria, CancellationToken ct = default) => Task.CompletedTask;
}
