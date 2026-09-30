using Ayudas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayudas.Infrastructure.Persistencia.Configuraciones;

public class AyudaConfiguration : IEntityTypeConfiguration<Ayuda>
{
    public void Configure(EntityTypeBuilder<Ayuda> builder)
    {
        builder.ToTable("Ayudas");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.NifBeneficiario).HasMaxLength(9).IsRequired();
        builder.Property(a => a.NombreBeneficiario).HasMaxLength(200).IsRequired();
        builder.Property(a => a.CodigoMunicipio).HasMaxLength(5).IsRequired();
        builder.Property(a => a.Concepto).HasMaxLength(300).IsRequired();
        builder.Property(a => a.ImporteSolicitado).HasPrecision(12, 2);
        builder.Property(a => a.ImporteConcedido).HasPrecision(12, 2);

        builder.HasOne(a => a.Remesa)
            .WithMany(r => r.Ayudas)
            .HasForeignKey(a => a.RemesaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
