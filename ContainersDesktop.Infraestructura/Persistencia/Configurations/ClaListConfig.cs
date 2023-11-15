using ContainersDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class ClaListConfig : IEntityTypeConfiguration<ClaList>
{
    public void Configure(EntityTypeBuilder<ClaList> builder)
    {
        builder
            .Property(x => x.Id)
            .HasColumnName("CLALIST_ID_REG")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Estado)
            .HasColumnName("CLALIST_ID_ESTADO_REG");

        builder
            .Property(x => x.FechaActualizacion)
            .HasColumnName("CLALIST_FECHA_ACTUALIZACION");

        builder.HasKey(x => x.Id);

        builder.ToTable("CLALIST");
    }
}
