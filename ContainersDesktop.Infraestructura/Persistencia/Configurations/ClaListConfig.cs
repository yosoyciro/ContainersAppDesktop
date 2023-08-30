using ContainersDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class ClaListConfig : IEntityTypeConfiguration<ClaList>
{
    public void Configure(EntityTypeBuilder<ClaList> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("CLALIST_ID_REG")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.ID);

        builder.ToTable("CLALIST");
    }
}
