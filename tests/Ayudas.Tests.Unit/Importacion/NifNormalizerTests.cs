using Ayudas.Importacion.Normalizacion;

namespace Ayudas.Tests.Unit.Importacion;

public class NifNormalizerTests
{
    [Theory]
    [InlineData("12345678Z", "12345678Z", false)]
    [InlineData("12.345.678-z", "12345678Z", true)]
    [InlineData(" 11111111h ", "11111111H", true)]
    [InlineData("22222222 J", "22222222J", true)]
    [InlineData("x1234567l", "X1234567L", true)]
    [InlineData("p0821100e", "P0821100E", true)]
    public void Normalizar_NifRecuperable_DevuelveNifLimpio(string entrada, string esperado, bool corregido)
    {
        var resultado = NifNormalizer.Normalizar(entrada);

        Assert.True(resultado.EsValido);
        Assert.Equal(esperado, resultado.Valor);
        Assert.Equal(corregido, resultado.Corregido);
    }

    [Theory]
    [InlineData("12345678A")]
    [InlineData("1234567")]
    [InlineData("")]
    [InlineData(null)]
    public void Normalizar_NifIncorrecto_SeRechaza(string? entrada)
    {
        var resultado = NifNormalizer.Normalizar(entrada);

        Assert.False(resultado.EsValido);
        Assert.NotNull(resultado.Motivo);
    }
}
