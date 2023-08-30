using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public class Lista : BaseEntity
{    
    public string? LISTAS_ID_ESTADO_REG
    {
        get;
        set;
    }

    public int LISTAS_ID_LISTA
    {
        get;
        set;
    }

    public int LISTAS_ID_LISTA_ORDEN
    {
        get;
        set;
    }

    public string? LISTAS_ID_LISTA_DESCRIP
    {
        get;
        set;
    }

    public string? LISTAS_FECHA_ACTUALIZACION
    {
        get;
        set;
    }
}
