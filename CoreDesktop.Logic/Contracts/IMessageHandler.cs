using CoreDesktop.Dominio.Models.Mensajeria;

namespace CoreDesktop.Logic.Contracts;
public interface IMessageHandler<in TMessage> : IMessageHandler where TMessage : Message
{
    Task Handle(TMessage @message);
}

public interface IMessageHandler
{
}
