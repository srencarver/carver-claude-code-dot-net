using Ayudas.Application.Abstracciones;
using Ayudas.Application.Dtos;
using Ayudas.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Infrastructure.Repositorios;

public class AyudaRepository : IAyudaRepository
{
    private readonly AyudasDbContext _db;

    public AyudaRepository(AyudasDbContext db)
    {
        _db = db;
    }

    public async Task<PaginaResultado<AyudaListadoDto>> ListarPaginadoAsync(int pagina, int tamanoPagina, CancellationToken ct = default)
    {
        var consulta = _db.Ayudas.AsNoTracking();

        var total = await consulta.CountAsync(ct);

        var elementos = await consulta
            .OrderByDescending(a => a.FechaSolicitud)
            .ThenBy(a => a.Id)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .Select(a => new AyudaListadoDto
            {
                Id = a.Id,
                RemesaReferencia = a.Remesa.Referencia,
                NifBeneficiario = a.NifBeneficiario,
                NombreBeneficiario = a.NombreBeneficiario,
                CodigoMunicipio = a.CodigoMunicipio,
                ImporteConcedido = a.ImporteConcedido,
                FechaSolicitud = a.FechaSolicitud
            })
            .ToListAsync(ct);

        return new PaginaResultado<AyudaListadoDto>(elementos, pagina, tamanoPagina, total);
    }
}
