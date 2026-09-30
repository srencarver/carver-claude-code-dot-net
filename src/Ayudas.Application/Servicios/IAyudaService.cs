using Ayudas.Application.Dtos;

namespace Ayudas.Application.Servicios;

public interface IAyudaService
{
    Task<PaginaResultado<AyudaListadoDto>> ListarAsync(int pagina, CancellationToken ct = default);
}
