using Ayudas.Application.Servicios;
using Ayudas.Tests.Unit.Fakes;

namespace Ayudas.Tests.Unit.Servicios;

public class RemesaServiceTests
{
    private readonly RemesaRepositoryFalso _repositorio = new();
    private readonly RemesaService _servicio;

    public RemesaServiceTests()
    {
        _servicio = new RemesaService(_repositorio);
    }

    [Fact]
    public async Task ObtenerDetalleAsync_RemesaExistente_DevuelveTotalesYAyudas()
    {
        _repositorio.Remesas.Add(Datos.Remesa(1, Datos.Entidad(direccion: "  Plaza   Mayor, 1 "), Datos.Convocatoria(0.80m), 100m, 200m));

        var detalle = await _servicio.ObtenerDetalleAsync(1);

        Assert.NotNull(detalle);
        Assert.Equal(300m, detalle.ImporteTotal);
        Assert.Equal(240m, detalle.ImporteCofinanciado);
        Assert.Equal("Plaza Mayor, 1", detalle.DireccionNotificacion);
        Assert.Equal(2, detalle.Ayudas.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task ObtenerDetalleAsync_EntidadSinDireccion_DevuelveDetalleSinDireccion(string? direccion)
    {
        _repositorio.Remesas.Add(Datos.Remesa(1, Datos.Entidad(direccion: direccion), Datos.Convocatoria(), 100m));

        var detalle = await _servicio.ObtenerDetalleAsync(1);

        Assert.NotNull(detalle);
        Assert.Null(detalle.DireccionNotificacion);
        Assert.Equal(100m, detalle.ImporteTotal);
    }

    [Fact]
    public async Task ObtenerDetalleAsync_RemesaInexistente_DevuelveNull()
    {
        var detalle = await _servicio.ObtenerDetalleAsync(42);

        Assert.Null(detalle);
    }
}
