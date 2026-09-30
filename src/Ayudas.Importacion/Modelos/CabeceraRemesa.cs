namespace Ayudas.Importacion.Modelos;

/// <summary>
/// Cabecera de la remesa tal y como llega en el fichero, sin limpiar ni validar.
/// </summary>
public class CabeceraRemesa
{
    public string? Referencia { get; set; }

    public string? NifEntidad { get; set; }

    public string? CodigoConvocatoria { get; set; }

    public string? FechaEnvio { get; set; }

    public string? NumeroSolicitudes { get; set; }

    public string? ImporteTotal { get; set; }
}
