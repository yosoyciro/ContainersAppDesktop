using CoreDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainersDesktop.Infraestructura.Persistencia.Configurations;
public class DispCalendarConfig : IEntityTypeConfiguration<DispCalendar>
{
    public void Configure(EntityTypeBuilder<DispCalendar> builder)
    {
        builder
            .Property(x => x.ID)
            .HasColumnName("DISP_CALENDAR_ID_REG")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Estado)
            .HasColumnName("DISP_CALENDAR_ID_ESTADO_REG");

        builder
            .Property(x => x.FechaActualizacion)
            .HasColumnName("DISP_CALENDAR_FECHA_ACTUALIZACION");

        builder.HasKey(x => x.ID);
        builder.HasIndex(x => new { x.DISP_CALENDAR_ID_DISPOSITIVO, x.DISP_CALENDAR_FECHA, x.Estado }).IsUnique();

        builder.ToTable("DISP_CALENDAR");
    }
}
