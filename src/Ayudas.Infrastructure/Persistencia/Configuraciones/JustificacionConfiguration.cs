using Ayudas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayudas.Infrastructure.Persistencia.Configuraciones;

/// <summary>
/// Tabla heredada: conserva los nombres de columna de la aplicación anterior.
/// </summary>
public class JustificacionConfiguration : IEntityTypeConfiguration<Justificacion>
{
    public void Configure(EntityTypeBuilder<Justificacion> builder)
    {
        builder.ToTable("JUSTIFICACIONES");
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).HasColumnName("ID_JUSTIF");
        builder.Property(j => j.AyudaId).HasColumnName("ID_AYUDA");
        builder.Property(j => j.FechaPresentacion).HasColumnName("F_PRESENT");
        builder.Property(j => j.ImporteJustificado).HasColumnName("IMP_JUSTIF").HasPrecision(12, 2);
        builder.Property(j => j.CodigoEstado).HasColumnName("COD_ESTADO").HasMaxLength(1).IsRequired();
        builder.Property(j => j.Observaciones).HasColumnName("OBSERV").HasMaxLength(500);

        builder.HasOne(j => j.Ayuda)
            .WithMany(a => a.Justificaciones)
            .HasForeignKey(j => j.AyudaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
