using Ayudas.Application.Abstracciones;
using Ayudas.Application.Dtos;
using Ayudas.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Infrastructure.Repositorios;

public class InformeRepository : IInformeRepository
{
    private readonly AyudasDbContext _db;

    public InformeRepository(AyudasDbContext db)
    {
        _db = db;
    }

    public Task<List<ResumenConvocatoriaDto>> ResumenPorConvocatoriaAsync(CancellationToken ct = default) =>
        _db.Convocatorias
            .AsNoTracking()
            .OrderBy(c => c.Codigo)
            .Select(c => new ResumenConvocatoriaDto
            {
                Codigo = c.Codigo,
                Titulo = c.Titulo,
                NumeroRemesas = c.Remesas.Count,
                ImporteConcedido = c.Remesas.SelectMany(r => r.Ayudas).Sum(a => a.ImporteConcedido)
            })
            .ToListAsync(ct);

    /// <summary>
    /// Justificaciones en estado P (pendiente) de la tabla heredada JUSTIFICACIONES.
    /// </summary>
    public Task<List<JustificacionPendienteDto>> JustificacionesPendientesAsync(CancellationToken ct = default) =>
        _db.Justificaciones
            .AsNoTracking()
            .Where(j => j.CodigoEstado == "P")
            .OrderBy(j => j.FechaPresentacion)
            .Take(50)
            .Select(j => new JustificacionPendienteDto
            {
                IdJustificacion = j.Id,
                ReferenciaRemesa = j.Ayuda.Remesa.Referencia,
                NombreBeneficiario = j.Ayuda.NombreBeneficiario,
                FechaPresentacion = j.FechaPresentacion,
                ImporteJustificado = j.ImporteJustificado
            })
            .ToListAsync(ct);
}
