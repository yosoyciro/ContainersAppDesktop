using AutoMapper;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Contracts;
using ContainersDesktop.Logica.Mensajeria.Messages;

namespace ContainersDesktop.Logica.Mensajeria.MessageHandlers;
public class TareaProgramadaModificadaHandler : IMessageHandler<TareaProgramadaModificada>
{
    private readonly IAsyncRepository<TareaProgramada> _repository;
    private readonly IMapper _mapper;
    public TareaProgramadaModificadaHandler(IAsyncRepository<TareaProgramada> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task Handle(TareaProgramadaModificada @message)
    {
        //var entidad = await _repository.GetByIdAsync(@message.TAREAS_PROGRAMADAS_ID_REG);

        var entidad = (TareaProgramada)_mapper.Map(@message, typeof(TareaProgramadaModificada), typeof(TareaProgramada));
        entidad.FechaActualizacion = FormatoFecha.FechaEstandar(DateTime.Now);

        await _repository.UpdateAsync(entidad);

        return;
    }
}
