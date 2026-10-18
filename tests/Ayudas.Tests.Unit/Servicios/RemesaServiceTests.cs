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
    public void CalcularCofinanciacion_RedondeoPorLineaYSobreTotalDistintos_RedondeaSobreElTotal()
    {
        // Por línea: 10,01 × 0,75 = 7,5075 → 7,51 (× 3 = 22,53). Sobre el total: 30,03 × 0,75 = 22,5225 → 22,52.
        var remesa = Datos.Remesa(1, Datos.Entidad(), Datos.Convocatoria(0.75m), 10.01m, 10.01m, 10.01m);

        var cofinanciado = _servicio.CalcularCofinanciacion(remesa);

        Assert.Equal(22.52m, cofinanciado);
    }

    [Fact]
    public void CalcularCofinanciacion_MedioCentimo_RedondeaHaciaArriba()
    {
        // 0,10 × 0,85 = 0,085 → 0,09 (el redondeo bancario daría 0,08).
        var remesa = Datos.Remesa(1, Datos.Entidad(), Datos.Convocatoria(0.85m), 0.10m);

        var cofinanciado = _servicio.CalcularCofinanciacion(remesa);

        Assert.Equal(0.09m, cofinanciado);
    }

    [Theory]
    [InlineData("10000.00", true)]
    [InlineData("10000.01", true)]
    [InlineData("9999.99", false)]
    [InlineData("0", false)]
    public async Task ObtenerDetalleAsync_ImporteConcedido_MarcaFiscalizacionPreviaDesdeElUmbral(string importe, bool esperado)
    {
        var concedido = decimal.Parse(importe, System.Globalization.CultureInfo.InvariantCulture);
        _repositorio.Remesas.Add(Datos.Remesa(1, Datos.Entidad(), Datos.Convocatoria(), concedido));

        var detalle = await _servicio.ObtenerDetalleAsync(1);

        Assert.NotNull(detalle);
        Assert.Equal(esperado, Assert.Single(detalle.Ayudas).RequiereFiscalizacion);
    }

    [Fact]
    public async Task ObtenerDetalleAsync_RemesaRechazada_NingunaAyudaVaAFiscalizacion()
    {
        var remesa = Datos.Remesa(1, Datos.Entidad(), Datos.Convocatoria(), 12_000m);
        remesa.Estado = Domain.Entidades.EstadoRemesa.Rechazada;
        remesa.Ayudas[0].ImporteConcedido = 0m;     // las rechazadas tienen concedido 0
        _repositorio.Remesas.Add(remesa);

        var detalle = await _servicio.ObtenerDetalleAsync(1);

        Assert.NotNull(detalle);
        Assert.False(detalle.Ayudas[0].RequiereFiscalizacion);
    }

    [Fact]
    public async Task ObtenerDetalleAsync_RemesaInexistente_DevuelveNull()
    {
        var detalle = await _servicio.ObtenerDetalleAsync(42);

        Assert.Null(detalle);
    }
}
