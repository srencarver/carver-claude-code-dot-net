namespace Ayudas.Domain.Comun;

/// <summary>
/// Validación de NIF españoles: DNI, NIE y NIF de personas jurídicas (antiguo CIF).
/// Trabaja sobre el NIF ya limpio: mayúsculas, sin espacios, puntos ni guiones.
/// </summary>
public static class Nif
{
    private const string LetrasDni = "TRWAGMYFPDXBNJZSQVHLCKE";
    private const string LetrasControlCif = "JABCDEFGHI";
    private const string LetrasOrganizacion = "ABCDEFGHJNPQRSUVW";

    public static bool EsValido(string? nif)
    {
        if (string.IsNullOrWhiteSpace(nif) || nif.Length != 9)
        {
            return false;
        }

        var primera = nif[0];

        if (char.IsDigit(primera))
        {
            return EsDniValido(nif);
        }

        if (primera is 'X' or 'Y' or 'Z')
        {
            var prefijo = primera switch { 'X' => '0', 'Y' => '1', _ => '2' };
            return EsDniValido(prefijo + nif[1..]);
        }

        if (LetrasOrganizacion.Contains(primera))
        {
            return EsNifOrganizacionValido(nif);
        }

        return false;
    }

    /// <summary>Calcula la letra de control de un DNI a partir de sus 8 dígitos.</summary>
    public static char LetraDni(int numero) => LetrasDni[numero % 23];

    /// <summary>Calcula el carácter de control de un NIF de organización a partir de sus 7 dígitos.</summary>
    public static char ControlOrganizacion(char letra, string sieteDigitos)
    {
        var control = (10 - SumaControl(sieteDigitos) % 10) % 10;

        // Entidades públicas, organismos y no residentes llevan letra; el resto, dígito.
        return "PQRSNW".Contains(letra) ? LetrasControlCif[control] : (char)('0' + control);
    }

    private static bool EsDniValido(string nif)
    {
        if (!int.TryParse(nif[..8], out var numero) || !nif[..8].All(char.IsDigit))
        {
            return false;
        }

        return nif[8] == LetraDni(numero);
    }

    private static bool EsNifOrganizacionValido(string nif)
    {
        var digitos = nif.Substring(1, 7);
        if (!digitos.All(char.IsDigit))
        {
            return false;
        }

        var esperado = ControlOrganizacion(nif[0], digitos);
        var recibido = nif[8];

        // Para las letras que admiten las dos formas (C, D, F, G, J, U, V) se aceptan ambas.
        if ("CDFGJUV".Contains(nif[0]))
        {
            var control = (10 - SumaControl(digitos) % 10) % 10;
            return recibido == (char)('0' + control) || recibido == LetrasControlCif[control];
        }

        return recibido == esperado;
    }

    private static int SumaControl(string sieteDigitos)
    {
        var suma = 0;
        for (var i = 0; i < 7; i++)
        {
            var digito = sieteDigitos[i] - '0';
            if (i % 2 == 0)
            {
                var doble = digito * 2;
                suma += doble / 10 + doble % 10;
            }
            else
            {
                suma += digito;
            }
        }

        return suma;
    }
}
