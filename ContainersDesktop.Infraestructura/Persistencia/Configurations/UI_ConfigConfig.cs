using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class UI_ConfigConfig : IEntityTypeConfiguration<UI_Config>
{
    public void Configure(EntityTypeBuilder<UI_Config> builder)
    {
        builder
            .Property(x => x.Id)
            .HasColumnName("UI_CONFIG_ID");

        builder
            .Property(x => x.Clave)
            .HasColumnName("UI_CONFIG_CLAVE");

        builder
            .Property(x => x.Valor)
            .HasColumnName("UI_CONFIG_VALOR");
    }
}
