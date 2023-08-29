using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Contracts.Services;
using CoreDesktop.Logic.Contracts;
using CoreDesktopLogica.Mensajeria.Messages;

namespace CoreDesktop.Logic.Mensajeria.MessageHandlers;
public class ContainerModificadoHandler : IMessageHandler<ContainerModificado>
{
    private readonly IObjetosServicio _objetosServicio;

    public ContainerModificadoHandler(IObjetosServicio objetosServicio)
    {
        _objetosServicio = objetosServicio;
    }

    public Task Handle(ContainerModificado @message)
    {
        var objeto = new Objetos()
        {
            OBJ_ID_REG = @message.OBJ_ID_REG,
            OBJ_SIGLAS = @message.OBJ_SIGLAS,
            OBJ_SIGLAS_LISTA = message.OBJ_SIGLAS_LISTA,
        };

        _objetosServicio.ActualizarObjeto(objeto);
        return Task.CompletedTask;
    }
}
