using Ayudas.Application.Dtos;

namespace Ayudas.Application.Abstracciones;

public interface IInformeRepository
{
    Task<List<ResumenConvocatoriaDto>> ResumenPorConvocatoriaAsync(CancellationToken ct = default);

    Task<List<JustificacionPendienteDto>> JustificacionesPendientesAsync(CancellationToken ct = default);
}
