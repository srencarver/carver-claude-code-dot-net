using Ayudas.Importacion.Validacion;

namespace Ayudas.Importacion;

/// <summary>
/// Resultado de importar un fichero de remesa.
/// </summary>
public class ResultadoImportacion
{
    public string? Referencia { get; set; }

    public string? Formato { get; set; }

    public string? NifEntidad { get; set; }

    public string? CodigoConvocatoria { get; set; }

    /// <summary>Id de la remesa creada; null si la remesa entera se ha rechazado.</summary>
    public int? RemesaId { get; set; }

    public string? MotivoRechazoRemesa { get; set; }

    public bool RemesaRechazada => MotivoRechazoRemesa is not null;

    public List<ResultadoValidacion> Resultados { get; set; } = new();

    public List<string> Avisos { get; set; } = new();

    public int Aceptadas => Resultados.Count(r => r.EsValida);

    public int Rechazadas => Resultados.Count(r => !r.EsValida);

    public decimal ImporteAceptado => Resultados.Where(r => r.EsValida).Sum(r => r.Ayuda!.ImporteSolicitado);

    public static ResultadoImportacion Rechazo(string motivo, string? referencia = null, string? formato = null) =>
        new() { MotivoRechazoRemesa = motivo, Referencia = referencia, Formato = formato };
}
