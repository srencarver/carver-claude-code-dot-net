using Ayudas.Application.Abstracciones;
using Ayudas.Domain.Entidades;
using Ayudas.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Infrastructure.Repositorios;

public class EntidadRepository : IEntidadRepository
{
    private readonly AyudasDbContext _db;

    public EntidadRepository(AyudasDbContext db)
    {
        _db = db;
    }

    public Task<List<EntidadLocal>> ListarAsync(CancellationToken ct = default) =>
        _db.EntidadesLocales
            .AsNoTracking()
            .OrderBy(e => e.Nombre)
            .ToListAsync(ct);

    public Task<EntidadLocal?> ObtenerAsync(int id, CancellationToken ct = default) =>
        _db.EntidadesLocales.FirstOrDefaultAsync(e => e.Id == id, ct);

    public Task<EntidadLocal?> ObtenerPorNifAsync(string nif, CancellationToken ct = default) =>
        _db.EntidadesLocales
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Nif == nif, ct);

    public async Task ActualizarAsync(EntidadLocal entidad, CancellationToken ct = default)
    {
        if (_db.Entry(entidad).State == EntityState.Detached)
        {
            _db.EntidadesLocales.Update(entidad);
        }

        await _db.SaveChangesAsync(ct);
    }
}
