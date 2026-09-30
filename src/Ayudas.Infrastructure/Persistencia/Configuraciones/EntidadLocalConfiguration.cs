using Ayudas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayudas.Infrastructure.Persistencia.Configuraciones;

public class EntidadLocalConfiguration : IEntityTypeConfiguration<EntidadLocal>
{
    public void Configure(EntityTypeBuilder<EntidadLocal> builder)
    {
        builder.ToTable("EntidadesLocales");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Nif).HasMaxLength(9).IsRequired();
        builder.HasIndex(e => e.Nif).IsUnique();
        builder.Property(e => e.Nombre).HasMaxLength(200).IsRequired();
        builder.Property(e => e.CodigoMunicipio).HasMaxLength(5).IsRequired();
        builder.Property(e => e.Provincia).HasMaxLength(60).IsRequired();
        builder.Property(e => e.DireccionNotificacion).HasMaxLength(300);
        builder.Property(e => e.CorreoNotificacion).HasMaxLength(150);
        builder.Property(e => e.UsuarioModificacion).HasMaxLength(100);
    }
}
