using Ayudas.Application.Abstracciones;
using Ayudas.Application.Dtos;
using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Servicios;

public class RemesaService : IRemesaService
{
    public const int TamanoPagina = 10;

    private readonly IRemesaRepository _remesas;

    public RemesaService(IRemesaRepository remesas)
    {
        _remesas = remesas;
    }

    public async Task<RemesaDetalleDto?> ObtenerDetalleAsync(int id, CancellationToken ct = default)
    {
        var remesa = await _remesas.ObtenerConDetalleAsync(id, ct);
        if (remesa is null)
        {
            return null;
        }

        return new RemesaDetalleDto
        {
            Id = remesa.Id,
            Referencia = remesa.Referencia,
            FechaEnvio = remesa.FechaEnvio,
            Estado = remesa.Estado,
            EntidadId = remesa.EntidadLocalId,
            EntidadNombre = remesa.EntidadLocal.Nombre,
            EntidadNif = remesa.EntidadLocal.Nif,
            DireccionNotificacion = FormatearDireccion(remesa.EntidadLocal.DireccionNotificacion),
            ConvocatoriaCodigo = remesa.Convocatoria.Codigo,
            ConvocatoriaTitulo = remesa.Convocatoria.Titulo,
            PorcentajeCofinanciacion = remesa.Convocatoria.PorcentajeCofinanciacion,
            ImporteTotal = remesa.Ayudas.Sum(a => a.ImporteConcedido),
            ImporteCofinanciado = CalcularCofinanciacion(remesa),
            Ayudas = remesa.Ayudas
                .OrderBy(a => a.Id)
                .Select(a => new AyudaDetalleDto
                {
                    Id = a.Id,
                    NifBeneficiario = a.NifBeneficiario,
                    NombreBeneficiario = a.NombreBeneficiario,
                    CodigoMunicipio = a.CodigoMunicipio,
                    Concepto = a.Concepto,
                    ImporteSolicitado = a.ImporteSolicitado,
                    ImporteConcedido = a.ImporteConcedido,
                    FechaSolicitud = a.FechaSolicitud
                })
                .ToList()
        };
    }

    public Task<PaginaResultado<RemesaListadoDto>> ListarPaginadoAsync(int? entidadId, EstadoRemesa? estado, int pagina, CancellationToken ct = default)
    {
        if (pagina < 1)
        {
            pagina = 1;
        }

        return _remesas.ListarPaginadoAsync(entidadId, estado, pagina, TamanoPagina, ct);
    }

    /// <summary>
    /// Importe que cofinancia el Estado: porcentaje de la convocatoria sobre lo concedido.
    /// </summary>
    public decimal CalcularCofinanciacion(Remesa remesa)
    {
        decimal total = 0;

        foreach (var ayuda in remesa.Ayudas)
        {
            // Se redondea cada línea a céntimos y se suman los redondeos.
            total += Math.Round(ayuda.ImporteConcedido * remesa.Convocatoria.PorcentajeCofinanciacion, 2);
        }

        return total;
    }

    private static string? FormatearDireccion(string? direccion)
    {
        // No todas las entidades tienen dirección de notificación: se devuelve null y no un error.
        if (string.IsNullOrWhiteSpace(direccion))
        {
            return null;
        }

        // Las direcciones llegan de las entidades con espacios de más.
        return string.Join(' ', direccion.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}
