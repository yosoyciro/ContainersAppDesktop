using ContainersDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class ObjetosConfig : IEntityTypeConfiguration<Objeto>
{
    public void Configure(EntityTypeBuilder<Objeto> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("OBJ_ID_REG")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Estado)
            .HasColumnName("OBJ_ID_ESTADO_REG");

        builder
            .Property(x => x.FechaActualizacion)
            .HasColumnName("OBJ_FECHA_ACTUALIZACION");

        builder.HasKey(x => x.ID);

        builder.ToTable("OBJETOS");
    }
}
