using Ayudas.Application.Dtos;
using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Abstracciones;

public interface IRemesaRepository
{
    /// <summary>Remesa con su entidad, su convocatoria y sus ayudas.</summary>
    Task<Remesa?> ObtenerConDetalleAsync(int id, CancellationToken ct = default);

    /// <summary>Página de remesas, de la más reciente a la más antigua, filtrada y proyectada en la base de datos.</summary>
    Task<PaginaResultado<RemesaListadoDto>> ListarPaginadoAsync(int? entidadId, EstadoRemesa? estado, int pagina, int tamanoPagina, CancellationToken ct = default);
}
