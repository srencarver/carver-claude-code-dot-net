using Ayudas.Importacion.Modelos;
using Ayudas.Importacion.Validacion;

namespace Ayudas.Tests.Unit.Importacion;

public class AyudaEntradaValidatorTests
{
    private static readonly ContextoValidacion Contexto = new(new DateOnly(2026, 1, 15), new DateOnly(2026, 12, 15), new DateOnly(2026, 7, 1));

    private readonly AyudaEntradaValidator _validador = new();

    [Fact]
    public void Validar_SolicitudDelContrato_EsValidaSinCorrecciones()
    {
        var resultado = _validador.Validar(Entrada(), Contexto);

        Assert.True(resultado.EsValida);
        Assert.Empty(resultado.Correcciones);
        Assert.Equal(1500.00m, resultado.Ayuda!.ImporteSolicitado);
        Assert.Equal(new DateOnly(2026, 5, 14), resultado.Ayuda.FechaSolicitud);
    }

    [Fact]
    public void Validar_CamposCorregibles_EsValidaYAnotaLasCorrecciones()
    {
        var entrada = Entrada(nif: "12.345.678-z", importe: "1.500,00", fecha: "14/05/2026", municipio: " 28079 ");

        var resultado = _validador.Validar(entrada, Contexto);

        Assert.True(resultado.EsValida);
        Assert.Equal("12345678Z", resultado.Ayuda!.NifBeneficiario);
        Assert.Equal("28079", resultado.Ayuda.CodigoMunicipio);
        Assert.Equal(4, resultado.Correcciones.Count);
    }

    [Theory]
    [InlineData("-150.00", "El importe debe ser mayor que 0")]
    [InlineData("0.00", "El importe debe ser mayor que 0")]
    [InlineData("60000.01", "El importe supera el máximo de 60.000,00 €")]
    public void Validar_ImporteFueraDeRango_SeRechaza(string importe, string motivo)
    {
        var resultado = _validador.Validar(Entrada(importe: importe), Contexto);

        var rechazo = Assert.Single(resultado.Rechazos);
        Assert.Equal(nameof(AyudaEntrada.ImporteSolicitado), rechazo.Campo);
        Assert.Equal(motivo, rechazo.Motivo);
    }

    [Theory]
    [InlineData("2025-12-20", "Fuera del plazo de la convocatoria")]
    [InlineData("2026-12-31", "Fuera del plazo de la convocatoria")]
    [InlineData("2026-07-03", "Posterior a la fecha de envío de la remesa")]
    [InlineData("30/02/2026", "Fecha no válida")]
    public void Validar_FechaNoAceptable_SeRechaza(string fecha, string motivo)
    {
        var resultado = _validador.Validar(Entrada(fecha: fecha), Contexto);

        var rechazo = Assert.Single(resultado.Rechazos);
        Assert.Equal(motivo, rechazo.Motivo);
    }

    [Fact]
    public void Validar_VariosErrores_LosDevuelveTodosConSuFila()
    {
        var entrada = Entrada(nif: "12345678A", municipio: "8019");
        entrada.Fila = 7;
        entrada.NombreBeneficiario = "";
        entrada.Concepto = null;

        var resultado = _validador.Validar(entrada, Contexto);

        Assert.False(resultado.EsValida);
        Assert.Equal(4, resultado.Rechazos.Count);
        Assert.All(resultado.Rechazos, r => Assert.Equal(7, r.Fila));
        Assert.Null(resultado.Ayuda);
    }

    private static AyudaEntrada Entrada(
        string nif = "12345678Z",
        string importe = "1500.00",
        string fecha = "2026-05-14",
        string municipio = "28079") => new()
    {
        Fila = 1,
        NifBeneficiario = nif,
        NombreBeneficiario = "Lucía Prueba Ejemplo",
        CodigoMunicipio = municipio,
        Concepto = "Eficiencia energética",
        ImporteSolicitado = importe,
        FechaSolicitud = fecha
    };
}
