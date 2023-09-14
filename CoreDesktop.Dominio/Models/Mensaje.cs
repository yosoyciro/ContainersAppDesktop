using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public class Mensaje : BaseEntity
{
    public Mensaje(string body, string tipoMensaje, string fechaHora, string estado)
    {
        MENSAJES_BODY = body;
        MENSAJES_TIPOMENSAJE = tipoMensaje;
        MENSAJES_FECHAHORA = fechaHora;
        MENSAJES_ESTADO = estado;
    }
    public Mensaje()
    {
        
    }

    public string? MENSAJES_BODY
    {
        get;
        set;
    }

    public string? MENSAJES_TIPOMENSAJE
    {
        get;
        set;
    }

    public string? MENSAJES_FECHAHORA
    {
        get;
        set;
    }

    public string? MENSAJES_ESTADO
    {
        get;
        set;
    }
}
