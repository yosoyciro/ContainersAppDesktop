using ContainersDesktop.Dominio.Models.Base;
using CoreDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public class Dispositivo : AuditableEntity
{        
    public string? DISPOSITIVOS_DESCRIP
    {
        get;
        set;
    }

    public string? DISPOSITIVOS_CONTAINER
    {
        get;
        set;
    }
}
