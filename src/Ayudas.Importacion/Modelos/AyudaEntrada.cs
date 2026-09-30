namespace Ayudas.Importacion.Modelos;

/// <summary>
/// Solicitud tal y como llega en el fichero: todos los campos como texto, sin limpiar ni validar.
/// </summary>
public class AyudaEntrada
{
    /// <summary>Posición de la solicitud en el fichero, empezando en 1.</summary>
    public int Fila { get; set; }

    public string? NifBeneficiario { get; set; }

    public string? NombreBeneficiario { get; set; }

    public string? CodigoMunicipio { get; set; }

    public string? Concepto { get; set; }

    public string? ImporteSolicitado { get; set; }

    public string? FechaSolicitud { get; set; }

    /// <summary>Elementos de la solicitud que no forman parte del contrato.</summary>
    public List<string> ElementosDesconocidos { get; set; } = new();
}
