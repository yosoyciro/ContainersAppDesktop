using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models.Base;
public abstract class AuditableEntity : BaseEntity
{
    public string? Estado
    {
        get;
        set;
    }

    public string? FechaActualizacion
    {
        get;
        set;
    }
}
