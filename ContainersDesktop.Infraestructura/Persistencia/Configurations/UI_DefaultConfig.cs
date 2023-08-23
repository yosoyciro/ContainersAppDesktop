using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class UI_DefaultConfig : IEntityTypeConfiguration<UI_Default>
{
    public void Configure(EntityTypeBuilder<UI_Default> builder)
    {
        builder
            .Property(x => x.Id)
            .HasColumnName("UI_DEFAULT_ID");

        builder
            .Property(x => x.Clave)
            .HasColumnName("UI_DEFAULT_CLAVE");

        builder
            .Property(x => x.Valor)
            .HasColumnName("UI_DEFAULT_VALOR");       
    }
}
