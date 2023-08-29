using ContainersDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class TareaProgramadaConfig : IEntityTypeConfiguration<TareaProgramada>
{
    public void Configure(EntityTypeBuilder<TareaProgramada> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("TAREAS_PROGRAMADAS_ID_REG")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.ID);

        builder.ToTable("TAREAS_PROGRAMADAS");
    }
}
