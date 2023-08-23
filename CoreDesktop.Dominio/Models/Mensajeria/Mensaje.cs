using System;

namespace CoreDesktop.Dominio.Models.Mensajeria;

public abstract class Mensaje
{
    public string TipoMensaje
    {
        get; protected set;
    }

    public Mensaje()
    {
        TipoMensaje = GetType().Name;
    }
}
