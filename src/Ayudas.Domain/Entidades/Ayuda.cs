namespace Ayudas.Domain.Entidades;

/// <summary>
/// Solicitud de ayuda de un beneficiario, incluida en una remesa.
/// </summary>
public class Ayuda
{
    public int Id { get; set; }

    public int RemesaId { get; set; }
    public Remesa Remesa { get; set; } = null!;

    public string NifBeneficiario { get; set; } = string.Empty;

    public string NombreBeneficiario { get; set; } = string.Empty;

    /// <summary>Código INE del municipio del beneficiario (5 dígitos).</summary>
    public string CodigoMunicipio { get; set; } = string.Empty;

    public string Concepto { get; set; } = string.Empty;

    public decimal ImporteSolicitado { get; set; }

    public decimal ImporteConcedido { get; set; }

    public DateOnly FechaSolicitud { get; set; }

    public List<Justificacion> Justificaciones { get; set; } = new();
}
