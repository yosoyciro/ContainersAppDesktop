using System.DirectoryServices.ActiveDirectory;
using AutoMapper;
using Azure.Core;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts.Repositories;
using CoreDesktop.Logic.Contracts;
using CoreDesktop.Logic.Mensajeria.Messages;
using Microsoft.Azure.Amqp.Encoding;
using Microsoft.Azure.Amqp.Framing;

namespace CoreDesktop.Logic.Mensajeria.MessageHandlers;
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
        var entidad = await _repository.GetByIdAsync(@message.TAREAS_PROGRAMADAS_ID_REG);

        entidad = (TareaProgramada)_mapper.Map(@message, entidad, typeof(TareaProgramadaModificada), typeof(TareaProgramada));
        entidad.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);
        await _repository.UpdateAsync(entidad);

        return;
    }
}
