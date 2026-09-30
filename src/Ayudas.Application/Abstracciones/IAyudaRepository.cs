using Ayudas.Application.Dtos;

namespace Ayudas.Application.Abstracciones;

public interface IAyudaRepository
{
    Task<PaginaResultado<AyudaListadoDto>> ListarPaginadoAsync(int pagina, int tamanoPagina, CancellationToken ct = default);
}
