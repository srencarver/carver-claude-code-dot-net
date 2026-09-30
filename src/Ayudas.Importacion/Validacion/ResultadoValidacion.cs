namespace Ayudas.Importacion.Validacion;

/// <summary>
/// Resultado de validar una solicitud: la ayuda normalizada si es válida, o sus rechazos.
/// </summary>
public class ResultadoValidacion
{
    public int Fila { get; init; }

    public AyudaNormalizada? Ayuda { get; init; }

    public List<Rechazo> Rechazos { get; init; } = new();

    /// <summary>Campos que no cumplían el contrato y se han corregido.</summary>
    public List<string> Correcciones { get; init; } = new();

    public bool EsValida => Rechazos.Count == 0;
}
