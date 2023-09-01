using ContainersDesktop.Dominio.Models.Base;
using CoreDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public class Lista : AuditableEntity
{      
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
}
