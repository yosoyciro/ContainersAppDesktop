using ContainersDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class ListasConfig : IEntityTypeConfiguration<Lista>
{
    public void Configure(EntityTypeBuilder<Lista> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("LISTAS_ID_REG")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Estado)
            .HasColumnName("LISTAS_ID_ESTADO_REG");

        builder
            .Property(x => x.FechaActualizacion)
            .HasColumnName("LISTAS_FECHA_ACTUALIZACION");

        builder.HasKey(x => x.ID);

        builder.ToTable("LISTAS");
    }
}
