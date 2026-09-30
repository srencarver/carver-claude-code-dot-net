using System.Globalization;
using System.Text.RegularExpressions;

namespace Ayudas.Importacion.Normalizacion;

/// <summary>
/// Convierte los importes a decimal. El contrato pide punto decimal y dos decimales (1234.56);
/// se aceptan también coma decimal (1234,56), separador de miles (1.234,56) y el símbolo €.
/// Se rechazan los importes ambiguos (1.234 puede ser 1234 o 1,234) y los de más de dos decimales.
/// </summary>
public static partial class ImporteNormalizer
{
    public static ResultadoNormalizacion<decimal> Normalizar(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return ResultadoNormalizacion<decimal>.Error("Importe vacío");
        }

        var texto = valor.Replace("€", string.Empty).Replace(" ", string.Empty).Trim();
        var tienePunto = texto.Contains('.');
        var tieneComa = texto.Contains(',');

        if (tienePunto && tieneComa)
        {
            // El último separador es el decimal; el otro es de miles.
            var decimalEsComa = texto.LastIndexOf(',') > texto.LastIndexOf('.');
            texto = decimalEsComa
                ? texto.Replace(".", string.Empty).Replace(',', '.')
                : texto.Replace(",", string.Empty);
        }
        else if (tieneComa)
        {
            if (texto.Count(c => c == ',') > 1)
            {
                return ResultadoNormalizacion<decimal>.Error("Importe no válido");
            }

            texto = texto.Replace(',', '.');
        }
        else if (tienePunto && SoloMiles().IsMatch(texto))
        {
            return ResultadoNormalizacion<decimal>.Error($"Importe ambiguo: «{valor.Trim()}» puede leerse con punto de miles o decimal");
        }

        if (!decimal.TryParse(texto, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var importe))
        {
            return ResultadoNormalizacion<decimal>.Error("Importe no válido");
        }

        if (importe.Scale > 2)
        {
            return ResultadoNormalizacion<decimal>.Error("El importe tiene más de dos decimales");
        }

        var cumpleContrato = ContratoImporte().IsMatch(valor);
        return ResultadoNormalizacion<decimal>.Ok(importe, corregido: !cumpleContrato);
    }

    [GeneratedRegex(@"^-?\d{1,3}(\.\d{3})+$")]
    private static partial Regex SoloMiles();

    [GeneratedRegex(@"^-?\d+(\.\d{1,2})?$")]
    private static partial Regex ContratoImporte();
}
