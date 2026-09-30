using Ayudas.Application.Abstracciones;
using Ayudas.Application.Dtos;

namespace Ayudas.Application.Servicios;

public class AyudaService : IAyudaService
{
    public const int TamanoPagina = 20;

    private readonly IAyudaRepository _ayudas;

    public AyudaService(IAyudaRepository ayudas)
    {
        _ayudas = ayudas;
    }

    public Task<PaginaResultado<AyudaListadoDto>> ListarAsync(int pagina, CancellationToken ct = default)
    {
        if (pagina < 1)
        {
            pagina = 1;
        }

        return _ayudas.ListarPaginadoAsync(pagina, TamanoPagina, ct);
    }
}
