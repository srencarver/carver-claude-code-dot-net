using Ayudas.Infrastructure.Repositorios;

namespace Ayudas.Tests.Integration.Repositorios;

public class AyudaRepositoryTests : IDisposable
{
    private readonly BaseDatosPruebas _bd = new();

    [Fact]
    public async Task ListarPaginadoAsync_DevuelveLaPaginaPedidaYElTotal()
    {
        using var db = _bd.CrearContexto();
        var total = db.Ayudas.Count();
        var repositorio = new AyudaRepository(db);

        var pagina = await repositorio.ListarPaginadoAsync(2, 20);

        Assert.Equal(total, pagina.TotalElementos);
        Assert.Equal(2, pagina.Pagina);
        Assert.Equal(20, pagina.Elementos.Count);
        Assert.All(pagina.Elementos, a => Assert.False(string.IsNullOrEmpty(a.RemesaReferencia)));
    }

    public void Dispose() => _bd.Dispose();
}
