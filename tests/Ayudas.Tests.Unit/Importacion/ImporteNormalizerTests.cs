using Ayudas.Importacion.Normalizacion;

namespace Ayudas.Tests.Unit.Importacion;

public class ImporteNormalizerTests
{
    [Theory]
    [InlineData("1234.56", "1234.56", false)]
    [InlineData("1.234,56", "1234.56", true)]
    [InlineData("1234,56", "1234.56", true)]
    [InlineData("2.500,00 €", "2500.00", true)]
    [InlineData("1,234.56", "1234.56", true)]
    [InlineData("950", "950", false)]
    [InlineData("-150.00", "-150.00", false)]
    public void Normalizar_ImporteRecuperable_DevuelveElDecimal(string entrada, string esperado, bool corregido)
    {
        var resultado = ImporteNormalizer.Normalizar(entrada);

        Assert.True(resultado.EsValido);
        Assert.Equal(decimal.Parse(esperado, System.Globalization.CultureInfo.InvariantCulture), resultado.Valor);
        Assert.Equal(corregido, resultado.Corregido);
    }

    [Theory]
    [InlineData("1.234")]
    [InlineData("12.500")]
    [InlineData("10.5555")]
    [InlineData("1,2,3")]
    [InlineData("mil euros")]
    [InlineData("")]
    public void Normalizar_ImporteAmbiguoONoValido_SeRechaza(string entrada)
    {
        var resultado = ImporteNormalizer.Normalizar(entrada);

        Assert.False(resultado.EsValido);
        Assert.NotNull(resultado.Motivo);
    }
}
