using Ayudas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayudas.Infrastructure.Persistencia.Configuraciones;

public class RemesaConfiguration : IEntityTypeConfiguration<Remesa>
{
    public void Configure(EntityTypeBuilder<Remesa> builder)
    {
        builder.ToTable("Remesas");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Referencia).HasMaxLength(30).IsRequired();
        builder.HasIndex(r => r.Referencia).IsUnique();
        builder.Property(r => r.Observaciones).HasMaxLength(500);

        builder.HasOne(r => r.EntidadLocal)
            .WithMany(e => e.Remesas)
            .HasForeignKey(r => r.EntidadLocalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Convocatoria)
            .WithMany(c => c.Remesas)
            .HasForeignKey(r => r.ConvocatoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
