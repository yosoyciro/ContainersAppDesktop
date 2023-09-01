using ContainersDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class DispositivosConfig : IEntityTypeConfiguration<Dispositivo>
{
    public void Configure(EntityTypeBuilder<Dispositivo> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("DISPOSITIVOS_ID_REG")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Estado)
            .HasColumnName("DISPOSITIVOS_ID_ESTADO_REG");

        builder
            .Property(x => x.FechaActualizacion)
            .HasColumnName("DISPOSITIVOS_FECHA_ACTUALIZACION");

        builder.HasKey(x => x.ID);

        builder.ToTable("DISPOSITIVOS");
    }
}
