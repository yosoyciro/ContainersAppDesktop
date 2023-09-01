using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using CoreDesktop.Logica.Contracts;
using CoreDesktopLogica.Mensajeria.Messages;

namespace CoreDesktop.Logica.Mensajeria.MessageHandlers;
public class ContainerModificadoHandler : IMessageHandler<ContainerModificado>
{
    private readonly IAsyncRepository<Objeto> _objetosServicio;

    public ContainerModificadoHandler(IAsyncRepository<Objeto> objetosServicio)
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
