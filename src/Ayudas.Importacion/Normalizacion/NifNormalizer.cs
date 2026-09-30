using Ayudas.Domain.Comun;

namespace Ayudas.Importacion.Normalizacion;

/// <summary>
/// Deja el NIF como pide el contrato: 9 caracteres en mayúsculas, sin espacios, puntos ni guiones.
/// Un NIF con el carácter de control incorrecto no se corrige: se rechaza.
/// </summary>
public static class NifNormalizer
{
    public static ResultadoNormalizacion<string> Normalizar(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return ResultadoNormalizacion<string>.Error("NIF vacío");
        }

        var limpio = new string(valor.Where(c => c is not (' ' or '.' or '-')).ToArray()).ToUpperInvariant();

        if (!Nif.EsValido(limpio))
        {
            return ResultadoNormalizacion<string>.Error("NIF no válido");
        }

        return ResultadoNormalizacion<string>.Ok(limpio, corregido: limpio != valor);
    }
}
