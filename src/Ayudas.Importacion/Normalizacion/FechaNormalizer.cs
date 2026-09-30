using System.Globalization;

namespace Ayudas.Importacion.Normalizacion;

/// <summary>
/// Convierte las fechas que envían las entidades a DateOnly. El contrato pide AAAA-MM-DD;
/// se aceptan también DD/MM/AAAA, fechas sin ceros a la izquierda y fechas con hora 00:00:00.
/// Una fecha que no existe (30/02, mes 13) no se corrige: se rechaza.
/// </summary>
public static class FechaNormalizer
{
    private const string FormatoContrato = "yyyy-MM-dd";

    private static readonly string[] FormatosAdmitidos =
    {
        FormatoContrato,
        "yyyy-M-d",
        "yyyy-MM-dd'T'HH:mm:ss",
        "dd/MM/yyyy",
        "d/M/yyyy"
    };

    public static ResultadoNormalizacion<DateOnly> Normalizar(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return ResultadoNormalizacion<DateOnly>.Error("Fecha vacía");
        }

        var texto = valor.Trim();

        if (!DateTime.TryParseExact(texto, FormatosAdmitidos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
        {
            return ResultadoNormalizacion<DateOnly>.Error("Fecha no válida");
        }

        if (fecha.TimeOfDay != TimeSpan.Zero)
        {
            return ResultadoNormalizacion<DateOnly>.Error("La fecha no puede llevar hora");
        }

        var corregido = !DateTime.TryParseExact(valor, FormatoContrato, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        return ResultadoNormalizacion<DateOnly>.Ok(DateOnly.FromDateTime(fecha), corregido);
    }
}
