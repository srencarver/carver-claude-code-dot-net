namespace Ayudas.Importacion.Modelos;

/// <summary>
/// Resultado de leer un fichero de remesa: la cabecera y las solicitudes en bruto.
/// </summary>
public class LecturaRemesa
{
    public string Formato { get; set; } = string.Empty;

    public CabeceraRemesa Cabecera { get; set; } = new();

    public List<AyudaEntrada> Solicitudes { get; set; } = new();
}
