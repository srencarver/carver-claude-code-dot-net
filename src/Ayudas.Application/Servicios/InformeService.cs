using Ayudas.Application.Abstracciones;
using Ayudas.Application.Dtos;

namespace Ayudas.Application.Servicios;

public class InformeService : IInformeService
{
    private readonly IInformeRepository _informes;

    public InformeService(IInformeRepository informes)
    {
        _informes = informes;
    }

    public async Task<InformeResumenDto> ObtenerResumenAsync(CancellationToken ct = default)
    {
        return new InformeResumenDto
        {
            Convocatorias = await _informes.ResumenPorConvocatoriaAsync(ct),
            JustificacionesPendientes = await _informes.JustificacionesPendientesAsync(ct)
        };
    }
}
