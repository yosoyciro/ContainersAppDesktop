using ContainersDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class SincronizacionesConfig : IEntityTypeConfiguration<Sincronizacion>
{
    public void Configure(EntityTypeBuilder<Sincronizacion> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("SINCRONIZACIONES_ID_REG")
            .ValueGeneratedOnAdd();       

        builder.HasKey(x => x.ID);

        builder.ToTable("SINCRONIZACIONES");
    }
}
