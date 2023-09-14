using System;

namespace ContainersDesktop.Dominio.Models.Mensajeria;

public abstract class Message
{
    //public Guid Id
    //{
    //    get; 
    //}
    public string TipoMensaje
    {
        get;
    }

    public Message()
    {
        TipoMensaje = GetType().Name;
    }
}
