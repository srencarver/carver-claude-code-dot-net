using Ayudas.Importacion.Normalizacion;

namespace Ayudas.Tests.Unit.Importacion;

public class FechaNormalizerTests
{
    [Theory]
    [InlineData("2026-02-01", 2026, 2, 1, false)]
    [InlineData("01/02/2026", 2026, 2, 1, true)]
    [InlineData("5/6/2026", 2026, 6, 5, true)]
    [InlineData("2026-4-5", 2026, 4, 5, true)]
    [InlineData("2026-05-12T00:00:00", 2026, 5, 12, true)]
    public void Normalizar_FechaRecuperable_DevuelveLaFecha(string entrada, int anio, int mes, int dia, bool corregido)
    {
        var resultado = FechaNormalizer.Normalizar(entrada);

        Assert.True(resultado.EsValido);
        Assert.Equal(new DateOnly(anio, mes, dia), resultado.Valor);
        Assert.Equal(corregido, resultado.Corregido);
    }

    [Theory]
    [InlineData("30/02/2026")]
    [InlineData("2026-13-01")]
    [InlineData("2026-05-12T10:30:00")]
    [InlineData("mañana")]
    [InlineData("")]
    public void Normalizar_FechaImposible_SeRechazaSinCorregir(string entrada)
    {
        var resultado = FechaNormalizer.Normalizar(entrada);

        Assert.False(resultado.EsValido);
    }
}
