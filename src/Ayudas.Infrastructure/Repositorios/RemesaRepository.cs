using Ayudas.Application.Abstracciones;
using Ayudas.Application.Dtos;
using Ayudas.Domain.Entidades;
using Ayudas.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Infrastructure.Repositorios;

public class RemesaRepository : IRemesaRepository
{
    private readonly AyudasDbContext _db;

    public RemesaRepository(AyudasDbContext db)
    {
        _db = db;
    }

    public Task<Remesa?> ObtenerConDetalleAsync(int id, CancellationToken ct = default) =>
        _db.Remesas
            .AsNoTracking()
            .Include(r => r.EntidadLocal)
            .Include(r => r.Convocatoria)
            .Include(r => r.Ayudas)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<bool> ExisteReferenciaAsync(string referencia, CancellationToken ct = default) =>
        _db.Remesas.AnyAsync(r => r.Referencia == referencia, ct);

    public async Task AgregarAsync(Remesa remesa, CancellationToken ct = default)
    {
        _db.Remesas.Add(remesa);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<PaginaResultado<RemesaListadoDto>> ListarPaginadoAsync(int? entidadId, EstadoRemesa? estado, int pagina, int tamanoPagina, CancellationToken ct = default)
    {
        var consulta = _db.Remesas.AsNoTracking();

        if (entidadId.HasValue)
        {
            consulta = consulta.Where(r => r.EntidadLocalId == entidadId.Value);
        }

        if (estado.HasValue)
        {
            consulta = consulta.Where(r => r.Estado == estado.Value);
        }

        var total = await consulta.CountAsync(ct);

        var elementos = await consulta
            .OrderByDescending(r => r.FechaEnvio)
            .ThenBy(r => r.Id)
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
            .ToListAsync(ct);

        return new PaginaResultado<RemesaListadoDto>(elementos, pagina, tamanoPagina, total);
    }
}
