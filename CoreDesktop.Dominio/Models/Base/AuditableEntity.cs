using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.Dominio.Models.Base;
public abstract class AuditableEntity : BaseEntity
{
    public string? Estado
    {
        get; set;
    }

    public string? FechaActualizacion
    {
        get; set;
    }
}
