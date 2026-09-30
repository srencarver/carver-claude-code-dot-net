using Ayudas.Domain.Entidades;
using Ayudas.Infrastructure.Repositorios;

namespace Ayudas.Tests.Integration.Repositorios;

public class RemesaRepositoryTests : IDisposable
{
    private readonly BaseDatosPruebas _bd = new();

    [Fact]
    public async Task ListarPaginadoAsync_SinFiltros_DevuelveTodasPorFechaDescendente()
    {
        using var db = _bd.CrearContexto();
        var repositorio = new RemesaRepository(db);

        var pagina = await repositorio.ListarPaginadoAsync(null, null, 1, 10);

        Assert.Equal(db.Remesas.Count(), pagina.TotalElementos);
        Assert.Equal(10, pagina.Elementos.Count);
        Assert.Equal(pagina.Elementos.OrderByDescending(r => r.FechaEnvio).Select(r => r.Id), pagina.Elementos.Select(r => r.Id));
    }

    [Fact]
    public async Task ListarPaginadoAsync_FiltroPorEstado_SoloDevuelveEseEstadoYCuentaBien()
    {
        using var db = _bd.CrearContexto();
        var esperadas = db.Remesas.Count(r => r.Estado == EstadoRemesa.Pagada);
        var repositorio = new RemesaRepository(db);

        var pagina = await repositorio.ListarPaginadoAsync(null, EstadoRemesa.Pagada, 1, 10);

        Assert.Equal(esperadas, pagina.TotalElementos);
        Assert.All(pagina.Elementos, r => Assert.Equal(EstadoRemesa.Pagada, r.Estado));
    }

    [Fact]
    public async Task ListarPaginadoAsync_FiltroPorEstado_SeConservaEnLaSegundaPagina()
    {
        using var db = _bd.CrearContexto();
        var repositorio = new RemesaRepository(db);

        var pagina = await repositorio.ListarPaginadoAsync(null, EstadoRemesa.Pagada, 2, 5);

        Assert.Equal(2, pagina.Pagina);
        Assert.NotEmpty(pagina.Elementos);
        Assert.All(pagina.Elementos, r => Assert.Equal(EstadoRemesa.Pagada, r.Estado));
    }

    [Fact]
    public async Task ListarPaginadoAsync_FiltroPorEntidadYEstado_AplicaLosDos()
    {
        using var db = _bd.CrearContexto();
        var entidadId = db.EntidadesLocales.OrderBy(e => e.Id).First().Id;
        var esperadas = db.Remesas.Count(r => r.EntidadLocalId == entidadId && r.Estado == EstadoRemesa.Pagada);
        var repositorio = new RemesaRepository(db);

        var pagina = await repositorio.ListarPaginadoAsync(entidadId, EstadoRemesa.Pagada, 1, 50);

        Assert.Equal(esperadas, pagina.TotalElementos);
        Assert.All(pagina.Elementos, r => Assert.Equal(EstadoRemesa.Pagada, r.Estado));
    }

    public void Dispose() => _bd.Dispose();
}
