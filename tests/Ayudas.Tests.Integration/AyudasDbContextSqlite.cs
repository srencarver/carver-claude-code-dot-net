using Ayudas.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Tests.Integration;

/// <summary>
/// Contexto de la aplicación para las pruebas con SQLite.
/// SQLite no tiene tipo decimal: EF Core no puede traducir Sum ni ordenar por importes.
/// En las pruebas los importes se guardan como double; en SQL Server siguen siendo decimal.
/// </summary>
public class AyudasDbContextSqlite : AyudasDbContext
{
    public AyudasDbContextSqlite(DbContextOptions<AyudasDbContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HaveConversion<double>();
    }
}
