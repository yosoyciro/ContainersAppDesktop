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
        try
        {
            var entidad = (TareaProgramada)_mapper.Map(@message, typeof(TareaProgramadaModificada), typeof(TareaProgramada));

            //Workaround para mensajes viejos
            if (entidad.TAREAS_PROGRAMADAS_ESTADO_TAREA == null)
            {
                entidad.TAREAS_PROGRAMADAS_ESTADO_TAREA = string.IsNullOrEmpty(@message.TAREAS_PROGRAMADAS_FECHA_COMPLETADA) ? "Pendiente" : "Completada";
            }
            entidad.FechaActualizacion = FormatoFecha.FechaEstandar(DateTime.Now);

            await _repository.UpdateAsync(entidad);

            return;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
