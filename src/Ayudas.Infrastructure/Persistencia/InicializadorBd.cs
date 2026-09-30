using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ayudas.Infrastructure.Persistencia;

/// <summary>
/// Crea la base de datos de formación si no existe y carga los datos de prueba.
/// No hay migraciones: para cambiar el esquema se recrea la base (scripts/reset-bd.ps1).
/// </summary>
public static class InicializadorBd
{
    public static async Task InicializarAsync(IServiceProvider servicios, bool recrear = false)
    {
        using var scope = servicios.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AyudasDbContext>();

        if (recrear)
        {
            await db.Database.EnsureDeletedAsync();
        }

        await db.Database.EnsureCreatedAsync();

        if (!await db.Municipios.AnyAsync())
        {
            DatosSemilla.Cargar(db);
            await db.SaveChangesAsync();
        }
    }
}
