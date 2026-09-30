using System.Xml.Linq;
using Ayudas.Importacion.Lectores;

namespace Ayudas.Tests.Unit.Importacion;

public class DiputacionXmlReaderTests
{
    private const string Envio = """
        <EnvioDiputacion diputacion="P2435900B" convocatoria="CONV-2026-02" referencia="REM-2026-200099" fecha="15/07/2026">
          <Beneficiario nif="12345678Z" municipio="24089">
            <Nombre>Lucía Prueba Ejemplo</Nombre>
            <Actuacion>Comercio de proximidad</Actuacion>
            <Importe moneda="EUR">1.250,00</Importe>
            <Fecha>02/07/2026</Fecha>
          </Beneficiario>
          <Beneficiario nif="11111111H" municipio="24089">
            <Nombre>Hugo Ensayo Modelo</Nombre>
            <Actuacion>Empleo juvenil</Actuacion>
            <Importe moneda="EUR">980,50</Importe>
            <Fecha>03/07/2026</Fecha>
          </Beneficiario>
        </EnvioDiputacion>
        """;

    private readonly DiputacionXmlReader _lector = new();

    [Fact]
    public void Leer_Envio_CabeceraDesdeLosAtributosYSinImporteTotal()
    {
        var lectura = _lector.Leer(XDocument.Parse(Envio));

        Assert.Equal("REM-2026-200099", lectura.Cabecera.Referencia);
        Assert.Equal("P2435900B", lectura.Cabecera.NifEntidad);
        Assert.Equal("CONV-2026-02", lectura.Cabecera.CodigoConvocatoria);
        Assert.Equal("15/07/2026", lectura.Cabecera.FechaEnvio);
        Assert.Equal("2", lectura.Cabecera.NumeroSolicitudes);
        Assert.Null(lectura.Cabecera.ImporteTotal);
    }

    [Fact]
    public void Leer_Beneficiario_SeCopiaEnBrutoConSuFila()
    {
        var lectura = _lector.Leer(XDocument.Parse(Envio));

        var segunda = lectura.Solicitudes[1];
        Assert.Equal(2, segunda.Fila);
        Assert.Equal("11111111H", segunda.NifBeneficiario);
        Assert.Equal("24089", segunda.CodigoMunicipio);
        Assert.Equal("Empleo juvenil", segunda.Concepto);
        Assert.Equal("980,50", segunda.ImporteSolicitado);
        Assert.Equal("03/07/2026", segunda.FechaSolicitud);
        Assert.Empty(segunda.ElementosDesconocidos);
    }

    [Fact]
    public void Soporta_FormatoDelContrato_DevuelveFalse()
    {
        Assert.False(_lector.Soporta(XDocument.Parse("<RemesaAyudas version=\"1.0\" />")));
        Assert.True(_lector.Soporta(XDocument.Parse(Envio)));
    }
}
