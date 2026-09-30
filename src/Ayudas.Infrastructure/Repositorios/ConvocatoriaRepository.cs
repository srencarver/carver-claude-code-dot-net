using Ayudas.Application.Abstracciones;
using Ayudas.Domain.Entidades;
using Ayudas.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Infrastructure.Repositorios;

public class ConvocatoriaRepository : IConvocatoriaRepository
{
    private readonly AyudasDbContext _db;

    public ConvocatoriaRepository(AyudasDbContext db)
    {
        _db = db;
    }

    public Task<List<Convocatoria>> ListarAsync(CancellationToken ct = default) =>
        _db.Convocatorias
            .AsNoTracking()
            .OrderByDescending(c => c.Ejercicio)
            .ThenBy(c => c.Codigo)
            .ToListAsync(ct);

    public Task<Convocatoria?> ObtenerAsync(int id, CancellationToken ct = default) =>
        _db.Convocatorias.FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Convocatoria?> ObtenerPorCodigoAsync(string codigo, CancellationToken ct = default) =>
        _db.Convocatorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Codigo == codigo, ct);

    public async Task AgregarAsync(Convocatoria convocatoria, CancellationToken ct = default)
    {
        _db.Convocatorias.Add(convocatoria);
        await _db.SaveChangesAsync(ct);
    }

    public Task ActualizarAsync(Convocatoria convocatoria, CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}
