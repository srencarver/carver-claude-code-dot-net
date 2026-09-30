using Ayudas.Infrastructure.Persistencia;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Tests.Integration;

/// <summary>
/// Base de datos SQLite en memoria con el esquema real y los datos de prueba de la aplicación.
/// Cada instancia es independiente: se crea al construir y se destruye al liberar.
/// No se usa el proveedor EF InMemory porque no se comporta como una base de datos relacional.
/// Los importes se guardan como double: ver AyudasDbContextSqlite.
/// </summary>
public sealed class BaseDatosPruebas : IDisposable
{
    private readonly SqliteConnection _conexion;

    public BaseDatosPruebas(bool cargarSemilla = true)
    {
        _conexion = new SqliteConnection("DataSource=:memory:");
        _conexion.Open();

        using var db = CrearContexto();
        db.Database.EnsureCreated();

        if (cargarSemilla)
        {
            DatosSemilla.Cargar(db);
            db.SaveChanges();
        }
    }

    /// <summary>Crea un contexto nuevo sobre la misma base de datos (sin caché de seguimiento compartida).</summary>
    public AyudasDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<AyudasDbContext>()
            .UseSqlite(_conexion)
            .AddInterceptors(new AuditoriaInterceptor())
            .Options;

        return new AyudasDbContextSqlite(opciones);
    }

    public void Dispose() => _conexion.Dispose();
}
