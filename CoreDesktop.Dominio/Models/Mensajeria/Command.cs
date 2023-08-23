using System;

namespace CoreDesktop.Dominio.Models.Mensajeria;
public class Command : Mensaje
{
    public DateTime TimeStamp
    {
        get; protected set; 
    }

    public Command()
    {
        TimeStamp = DateTime.Now;
    }
}
