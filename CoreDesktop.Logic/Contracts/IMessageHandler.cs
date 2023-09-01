using CoreDesktop.Dominio.Models.Mensajeria;

namespace CoreDesktop.Logica.Contracts;
public interface IMessageHandler<in TMessage> : IMessageHandler where TMessage : Message
{
    Task Handle(TMessage @message);
}

public interface IMessageHandler
{
}
