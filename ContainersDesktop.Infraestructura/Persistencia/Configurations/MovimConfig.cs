using ContainersDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class MovimConfig : IEntityTypeConfiguration<Movim>
{
    public void Configure(EntityTypeBuilder<Movim> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("MOVIM_ID_REG")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.ID);

        builder.ToTable("MOVIM");
    }
}
