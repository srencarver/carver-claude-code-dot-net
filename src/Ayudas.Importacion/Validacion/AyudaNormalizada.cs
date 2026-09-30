namespace Ayudas.Importacion.Validacion;

/// <summary>
/// Solicitud que ha superado la normalización y la validación, con sus valores ya tipados.
/// </summary>
public class AyudaNormalizada
{
    public int Fila { get; set; }
    public string NifBeneficiario { get; set; } = string.Empty;
    public string NombreBeneficiario { get; set; } = string.Empty;
    public string CodigoMunicipio { get; set; } = string.Empty;
    public string Concepto { get; set; } = string.Empty;
    public decimal ImporteSolicitado { get; set; }
    public DateOnly FechaSolicitud { get; set; }
}
