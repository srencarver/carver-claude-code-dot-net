using System.Xml.Linq;
using Ayudas.Importacion.Lectores;

namespace Ayudas.Tests.Unit.Importacion;

public class AyudasXmlReaderTests
{
    private const string RemesaMinima = """
        <RemesaAyudas version="1.0">
          <Cabecera>
            <Referencia>REM-2026-100099</Referencia>
            <NifEntidad>P2839600J</NifEntidad>
            <CodigoConvocatoria>CONV-2026-01</CodigoConvocatoria>
            <FechaEnvio>2026-07-01</FechaEnvio>
            <NumeroSolicitudes>1</NumeroSolicitudes>
            <ImporteTotal>1500.00</ImporteTotal>
          </Cabecera>
          <Solicitudes>
            <Solicitud>
              <NifBeneficiario>12345678Z</NifBeneficiario>
              <NombreBeneficiario>Lucía Prueba Ejemplo</NombreBeneficiario>
              <CodigoMunicipio>28079</CodigoMunicipio>
              <Concepto>Eficiencia energética</Concepto>
              <ImporteSolicitado>1500.00</ImporteSolicitado>
              <FechaSolicitud>2026-05-14</FechaSolicitud>
              <Telefono>600000000</Telefono>
            </Solicitud>
          </Solicitudes>
        </RemesaAyudas>
        """;

    private readonly AyudasXmlReader _lector = new();

    [Fact]
    public void Leer_RemesaDelContrato_DevuelveCabeceraYSolicitudesEnBruto()
    {
        var lectura = _lector.Leer(XDocument.Parse(RemesaMinima));

        Assert.Equal("REM-2026-100099", lectura.Cabecera.Referencia);
        Assert.Equal("1500.00", lectura.Cabecera.ImporteTotal);
        var solicitud = Assert.Single(lectura.Solicitudes);
        Assert.Equal(1, solicitud.Fila);
        Assert.Equal("12345678Z", solicitud.NifBeneficiario);
        Assert.Equal("2026-05-14", solicitud.FechaSolicitud);
    }

    [Fact]
    public void Leer_ElementoFueraDelContrato_SeAnotaComoDesconocido()
    {
        var lectura = _lector.Leer(XDocument.Parse(RemesaMinima));

        Assert.Equal(new[] { "Telefono" }, lectura.Solicitudes[0].ElementosDesconocidos);
    }

    [Fact]
    public void Soporta_OtroFormato_DevuelveFalse()
    {
        Assert.False(_lector.Soporta(XDocument.Parse("<EnvioDiputacion />")));
    }
}
