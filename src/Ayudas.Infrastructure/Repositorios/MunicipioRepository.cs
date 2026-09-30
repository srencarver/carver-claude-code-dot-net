using Ayudas.Application.Abstracciones;
using Ayudas.Domain.Entidades;
using Ayudas.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Infrastructure.Repositorios;

public class MunicipioRepository : IMunicipioRepository
{
    private readonly AyudasDbContext _db;

    public MunicipioRepository(AyudasDbContext db)
    {
        _db = db;
    }

    public Task<List<Municipio>> ListarAsync(string? provincia, CancellationToken ct = default)
    {
        var consulta = _db.Municipios.AsNoTracking();

        if (provincia is not null)
        {
            consulta = consulta.Where(m => m.Provincia == provincia);
        }

        return consulta
            .OrderBy(m => m.Provincia)
            .ThenBy(m => m.Nombre)
            .ToListAsync(ct);
    }
}
