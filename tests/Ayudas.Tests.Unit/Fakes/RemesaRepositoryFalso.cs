using Ayudas.Application.Abstracciones;
using Ayudas.Application.Dtos;
using Ayudas.Domain.Entidades;

namespace Ayudas.Tests.Unit.Fakes;

public class RemesaRepositoryFalso : IRemesaRepository
{
    public List<Remesa> Remesas { get; } = new();

    public Task<Remesa?> ObtenerConDetalleAsync(int id, CancellationToken ct = default) =>
        Task.FromResult(Remesas.FirstOrDefault(r => r.Id == id));

    public Task<PaginaResultado<RemesaListadoDto>> ListarPaginadoAsync(int? entidadId, int pagina, int tamanoPagina, CancellationToken ct = default)
    {
        var filtradas = Remesas
            .Where(r => entidadId == null || r.EntidadLocalId == entidadId)
            .OrderByDescending(r => r.FechaEnvio)
            .ThenBy(r => r.Id)
            .ToList();

        var elementos = filtradas
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .Select(r => new RemesaListadoDto
            {
                Id = r.Id,
                Referencia = r.Referencia,
                EntidadNombre = r.EntidadLocal.Nombre,
                ConvocatoriaCodigo = r.Convocatoria.Codigo,
                FechaEnvio = r.FechaEnvio,
                Estado = r.Estado,
                NumeroAyudas = r.Ayudas.Count,
                ImporteConcedido = r.Ayudas.Sum(a => a.ImporteConcedido)
            })
            .ToList();

        return Task.FromResult(new PaginaResultado<RemesaListadoDto>(elementos, pagina, tamanoPagina, filtradas.Count));
    }
}
