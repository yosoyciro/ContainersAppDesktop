using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public class ClaList : BaseEntity
{    
    public string? CLALIST_ID_ESTADO_REG
    {
        get;
        set;
    }

    public string? CLALIST_DESCRIP
    {
        get;
        set;
    }

    public string? CLALIST_FECHA_ACTUALIZACION
    {
        get;
        set;
    }
}
