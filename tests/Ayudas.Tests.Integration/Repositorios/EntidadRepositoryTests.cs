using Ayudas.Infrastructure.Repositorios;

namespace Ayudas.Tests.Integration.Repositorios;

public class EntidadRepositoryTests : IDisposable
{
    private readonly BaseDatosPruebas _bd = new();

    [Fact]
    public async Task ListarAsync_DevuelveTodasOrdenadasPorNombre()
    {
        using var db = _bd.CrearContexto();
        var repositorio = new EntidadRepository(db);

        var entidades = await repositorio.ListarAsync();

        Assert.Equal(12, entidades.Count);

        // SQLite ordena por bytes (Murcia antes que Málaga); SQL Server, según la intercalación de la base de datos.
        // Por eso aquí se compara con un orden ordinal y no con el de la cultura del equipo.
        Assert.Equal(entidades.OrderBy(e => e.Nombre, StringComparer.Ordinal).Select(e => e.Id), entidades.Select(e => e.Id));
    }

    [Fact]
    public async Task ActualizarAsync_RellenaLosCamposDeAuditoria()
    {
        int id;
        using (var db = _bd.CrearContexto())
        {
            var repositorio = new EntidadRepository(db);
            var entidad = (await repositorio.ListarAsync()).First();
            id = entidad.Id;

            var cargada = await repositorio.ObtenerAsync(id);
            cargada!.Nombre = "Nombre cambiado";
            await repositorio.ActualizarAsync(cargada);
        }

        using (var db = _bd.CrearContexto())
        {
            var guardada = await new EntidadRepository(db).ObtenerAsync(id);
            Assert.Equal("Nombre cambiado", guardada!.Nombre);
            Assert.NotNull(guardada.FechaModificacion);
            Assert.False(string.IsNullOrEmpty(guardada.UsuarioModificacion));
        }
    }

    public void Dispose() => _bd.Dispose();
}
