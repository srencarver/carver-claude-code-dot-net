namespace Ayudas.Importacion.Normalizacion;

/// <summary>
/// Resultado de normalizar un campo: el valor limpio o el motivo por el que no se puede aceptar.
/// </summary>
public sealed class ResultadoNormalizacion<T>
{
    private ResultadoNormalizacion(bool esValido, T? valor, bool corregido, string? motivo)
    {
        EsValido = esValido;
        Valor = valor;
        Corregido = corregido;
        Motivo = motivo;
    }

    public bool EsValido { get; }

    public T? Valor { get; }

    /// <summary>True si el valor recibido no cumplía el contrato y se ha corregido.</summary>
    public bool Corregido { get; }

    public string? Motivo { get; }

    public static ResultadoNormalizacion<T> Ok(T valor, bool corregido) => new(true, valor, corregido, null);

    public static ResultadoNormalizacion<T> Error(string motivo) => new(false, default, false, motivo);
}
