using Ayudas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayudas.Infrastructure.Persistencia.Configuraciones;

public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
{
    public void Configure(EntityTypeBuilder<Municipio> builder)
    {
        builder.ToTable("Municipios");
        builder.HasKey(m => m.CodigoIne);
        builder.Property(m => m.CodigoIne).HasMaxLength(5);
        builder.Property(m => m.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(m => m.Provincia).HasMaxLength(60).IsRequired();
    }
}
