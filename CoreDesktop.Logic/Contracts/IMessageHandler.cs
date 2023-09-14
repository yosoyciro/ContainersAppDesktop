using ContainersDesktop.Dominio.Models.Mensajeria;

namespace ContainersDesktop.Logica.Contracts;
public interface IMessageHandler<in TMessage> : IMessageHandler where TMessage : Message
{
    Task Handle(TMessage @message);
}

public interface IMessageHandler
{
}
