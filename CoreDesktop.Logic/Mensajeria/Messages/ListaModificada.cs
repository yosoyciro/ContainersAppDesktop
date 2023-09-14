using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Mensajeria;

namespace ContainersDesktop.Logica.Mensajeria.Messages;
public class ListaModificada : Message
{
    public int ID
    {
        get;
        set;
    }

    public string? Estado
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

    public string? FechaActualizacion
    {
        get;
        set;
    }
}
