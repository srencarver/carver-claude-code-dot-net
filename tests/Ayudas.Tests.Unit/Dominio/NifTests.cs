using Ayudas.Domain.Comun;

namespace Ayudas.Tests.Unit.Dominio;

public class NifTests
{
    [Theory]
    [InlineData("12345678Z")]
    [InlineData("00000000T")]
    [InlineData("X1234567L")]
    [InlineData("P2807900B")]
    public void EsValido_NifCorrecto_DevuelveTrue(string nif)
    {
        Assert.True(Nif.EsValido(nif));
    }

    [Theory]
    [InlineData("12345678A")]
    [InlineData("P2807900C")]
    [InlineData("1234567Z")]
    [InlineData("")]
    [InlineData(null)]
    public void EsValido_NifIncorrecto_DevuelveFalse(string? nif)
    {
        Assert.False(Nif.EsValido(nif));
    }
}
