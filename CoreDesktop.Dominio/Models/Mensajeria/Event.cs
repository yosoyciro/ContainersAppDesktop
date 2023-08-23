using System;

namespace CoreDesktop.Dominio.Models.Mensajeria;
public class Event
{
    public DateTime TimeStamp
    {
        get; protected set;
    }

    public Event()
    {
        TimeStamp = DateTime.Now;
    }
}
