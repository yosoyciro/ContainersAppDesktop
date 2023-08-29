using CoreDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class MensajeConfig : IEntityTypeConfiguration<Mensaje>
{
    public void Configure(EntityTypeBuilder<Mensaje> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("MENSAJES_ID")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.ID);

        builder.ToTable("MENSAJES");
    }
}
