using Ayudas.Application.Abstracciones;
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
}
