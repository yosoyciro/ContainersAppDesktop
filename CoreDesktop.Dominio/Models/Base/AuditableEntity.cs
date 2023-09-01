using ContainersDesktop.Dominio.Models.Base;

namespace CoreDesktop.Dominio.Models.Base;
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
