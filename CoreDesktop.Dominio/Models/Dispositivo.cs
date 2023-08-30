using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public class Dispositivo : BaseEntity
{    
    public string DISPOSITIVOS_ID_ESTADO_REG
    {
        get;
        set;
    }

    public string DISPOSITIVOS_DESCRIP
    {
        get;
        set;
    }

    public string DISPOSITIVOS_CONTAINER
    {
        get;
        set;
    }

    public string DISPOSITIVOS_FECHA_ACTUALIZACION
    {
        get;
        set;
    }
}
