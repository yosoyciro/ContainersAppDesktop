using ContainersDesktop.Dominio.Models;
using CoreDesktop.Logic.Contracts;
using CoreDesktopLogica.Mensajeria.Messages;

namespace CoreDesktop.Logic.Mensajeria.MessageHandlers;
public class ContainerModificadoHandler : IMessageHandler<ContainerModificado>
{
    private readonly IServiciosRepositorios<Objeto> _objetosServicio;

    public ContainerModificadoHandler(IServiciosRepositorios<Objeto> objetosServicio)
    {
        _objetosServicio = objetosServicio;
    }

    public Task Handle(ContainerModificado @message)
    {
        var objeto = new Objeto()
        {
            ID = @message.OBJ_ID_REG,
            OBJ_SIGLAS = @message.OBJ_SIGLAS,
            OBJ_SIGLAS_LISTA = message.OBJ_SIGLAS_LISTA,
        };

        _objetosServicio.UpdateAsync(objeto);
        return Task.CompletedTask;
    }
}
