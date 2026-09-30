using Ayudas.Application.Dtos;

namespace Ayudas.Application.Servicios;

public interface IInformeService
{
    Task<InformeResumenDto> ObtenerResumenAsync(CancellationToken ct = default);
}
