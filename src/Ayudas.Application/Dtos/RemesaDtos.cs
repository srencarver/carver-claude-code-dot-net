using Ayudas.Domain.Entidades;

namespace Ayudas.Application.Dtos;

public class RemesaListadoDto
{
    public int Id { get; set; }
    public string Referencia { get; set; } = string.Empty;
    public string EntidadNombre { get; set; } = string.Empty;
    public string ConvocatoriaCodigo { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; }
    public EstadoRemesa Estado { get; set; }
    public int NumeroAyudas { get; set; }
    public decimal ImporteConcedido { get; set; }
}

public class RemesaDetalleDto
{
    public int Id { get; set; }
    public string Referencia { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; }
    public EstadoRemesa Estado { get; set; }

    public int EntidadId { get; set; }
    public string EntidadNombre { get; set; } = string.Empty;
    public string EntidadNif { get; set; } = string.Empty;
    /// <summary>Null si la entidad no tiene dirección de notificación informada.</summary>
    public string? DireccionNotificacion { get; set; }

    public string ConvocatoriaCodigo { get; set; } = string.Empty;
    public string ConvocatoriaTitulo { get; set; } = string.Empty;
    public decimal PorcentajeCofinanciacion { get; set; }

    public decimal ImporteTotal { get; set; }
    public decimal ImporteCofinanciado { get; set; }

    public List<AyudaDetalleDto> Ayudas { get; set; } = new();
}

public class AyudaDetalleDto
{
    public int Id { get; set; }
    public string NifBeneficiario { get; set; } = string.Empty;
    public string NombreBeneficiario { get; set; } = string.Empty;
    public string CodigoMunicipio { get; set; } = string.Empty;
    public string Concepto { get; set; } = string.Empty;
    public decimal ImporteSolicitado { get; set; }
    public decimal ImporteConcedido { get; set; }
    public DateOnly FechaSolicitud { get; set; }
}
