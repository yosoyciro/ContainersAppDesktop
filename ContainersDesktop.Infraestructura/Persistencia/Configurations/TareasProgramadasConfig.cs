using ContainersDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class TareasProgramadasConfig : IEntityTypeConfiguration<TareaProgramada>
{
    public void Configure(EntityTypeBuilder<TareaProgramada> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("TAREAS_PROGRAMADAS_ID_REG")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Estado)
            .HasColumnName("TAREAS_PROGRAMADAS_ID_ESTADO_REG");

        builder
            .Property(x => x.FechaActualizacion)
            .HasColumnName("TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION");

        builder.HasKey(x => x.ID);

        builder.ToTable("TAREAS_PROGRAMADAS");
    }
}
