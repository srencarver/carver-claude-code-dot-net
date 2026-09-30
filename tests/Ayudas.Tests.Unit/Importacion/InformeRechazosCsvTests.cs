using Ayudas.Importacion.Informes;
using Ayudas.Importacion.Validacion;

namespace Ayudas.Tests.Unit.Importacion;

public class InformeRechazosCsvTests
{
    [Fact]
    public void Generar_ConRechazos_UnaLineaPorRechazoOrdenadasPorFila()
    {
        var resultados = new[]
        {
            new ResultadoValidacion { Fila = 3, Rechazos = { new Rechazo(3, "FechaSolicitud", "30/02/2026", "Fecha no válida") } },
            new ResultadoValidacion { Fila = 1, Ayuda = new AyudaNormalizada { Fila = 1 } },
            new ResultadoValidacion { Fila = 2, Rechazos = { new Rechazo(2, "ImporteSolicitado", "1.234", "Importe ambiguo") } }
        };

        var lineas = InformeRechazosCsv.Generar(resultados).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(new[]
        {
            "linea;campo;valor;motivo",
            "2;ImporteSolicitado;1.234;Importe ambiguo",
            "3;FechaSolicitud;30/02/2026;Fecha no válida"
        }, lineas);
    }

    [Fact]
    public void Generar_ValorConPuntoYComaOEspacios_SeEntrecomilla()
    {
        var resultados = new[]
        {
            new ResultadoValidacion { Fila = 1, Rechazos = { new Rechazo(1, "NifBeneficiario", " 1;2\"3 ", "NIF no válido") } }
        };

        var csv = InformeRechazosCsv.Generar(resultados);

        Assert.Contains("1;NifBeneficiario;\" 1;2\"\"3 \";NIF no válido", csv);
    }
}
