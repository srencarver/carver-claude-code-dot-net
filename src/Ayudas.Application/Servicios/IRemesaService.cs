using Ayudas.Application.Dtos;
using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Servicios;

public interface IRemesaService
{
    Task<RemesaDetalleDto?> ObtenerDetalleAsync(int id, CancellationToken ct = default);

    Task<PaginaResultado<RemesaListadoDto>> ListarPaginadoAsync(int? entidadId, EstadoRemesa? estado, int pagina, CancellationToken ct = default);

    decimal CalcularCofinanciacion(Remesa remesa);
}
