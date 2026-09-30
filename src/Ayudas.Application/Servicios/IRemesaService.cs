using Ayudas.Application.Dtos;
using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Servicios;

public interface IRemesaService
{
    Task<RemesaDetalleDto?> ObtenerDetalleAsync(int id, CancellationToken ct = default);

    decimal CalcularCofinanciacion(Remesa remesa);
}
