using Ayudas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Ayudas.Infrastructure.Persistencia;

public class AyudasDbContext : DbContext
{
    public AyudasDbContext(DbContextOptions<AyudasDbContext> options) : base(options)
    {
    }

    public DbSet<EntidadLocal> EntidadesLocales => Set<EntidadLocal>();
    public DbSet<Convocatoria> Convocatorias => Set<Convocatoria>();
    public DbSet<Remesa> Remesas => Set<Remesa>();
    public DbSet<Ayuda> Ayudas => Set<Ayuda>();
    public DbSet<Justificacion> Justificaciones => Set<Justificacion>();
    public DbSet<Municipio> Municipios => Set<Municipio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AyudasDbContext).Assembly);
    }
}
