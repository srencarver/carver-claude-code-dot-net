using Ayudas.Application.Abstracciones;
using Ayudas.Domain.Entidades;

namespace Ayudas.Tests.Unit.Fakes;

public class RemesaRepositoryFalso : IRemesaRepository
{
    public List<Remesa> Remesas { get; } = new();

    public Task<Remesa?> ObtenerConDetalleAsync(int id, CancellationToken ct = default) =>
        Task.FromResult(Remesas.FirstOrDefault(r => r.Id == id));
}
