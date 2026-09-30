using Ayudas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayudas.Infrastructure.Persistencia.Configuraciones;

public class ConvocatoriaConfiguration : IEntityTypeConfiguration<Convocatoria>
{
    public void Configure(EntityTypeBuilder<Convocatoria> builder)
    {
        builder.ToTable("Convocatorias");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Codigo).HasMaxLength(20).IsRequired();
        builder.HasIndex(c => c.Codigo).IsUnique();
        builder.Property(c => c.Titulo).HasMaxLength(250).IsRequired();
        builder.Property(c => c.PorcentajeCofinanciacion).HasPrecision(5, 4);
    }
}
