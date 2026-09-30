using Ayudas.Importacion.Validacion;

namespace Ayudas.Importacion.Informes;

/// <summary>
/// Informe de rechazos en CSV separado por punto y coma: línea, campo, valor y motivo.
/// </summary>
public static class InformeRechazosCsv
{
    public const string Cabecera = "linea;campo;valor;motivo";

    public static void Escribir(TextWriter destino, IEnumerable<ResultadoValidacion> resultados)
    {
        destino.WriteLine(Cabecera);

        foreach (var rechazo in resultados.SelectMany(r => r.Rechazos).OrderBy(r => r.Fila))
        {
            destino.WriteLine(string.Join(';',
                rechazo.Fila.ToString(),
                Escapar(rechazo.Campo),
                Escapar(rechazo.Valor ?? string.Empty),
                Escapar(rechazo.Motivo)));
        }
    }

    public static string Generar(IEnumerable<ResultadoValidacion> resultados)
    {
        using var escritor = new StringWriter();
        Escribir(escritor, resultados);
        return escritor.ToString();
    }

    private static string Escapar(string valor)
    {
        if (valor.IndexOfAny(new[] { ';', '"', '\n', '\r' }) < 0 && valor.Trim() == valor)
        {
            return valor;
        }

        return "\"" + valor.Replace("\"", "\"\"") + "\"";
    }
}
