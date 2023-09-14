using AutoMapper;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Contracts;
using ContainersDesktop.Logica.Mensajeria.Messages;

namespace ContainersDesktop.Logica.Mensajeria.MessageHandlers;
public class TareaProgramadaArchivoCreadoHandler : IMessageHandler<TareaProgramadaArchivoCreado>
{
    private readonly IAsyncRepository<TareaProgramadaArchivo> _repository;
    private readonly IMapper _mapper;
    public TareaProgramadaArchivoCreadoHandler(IAsyncRepository<TareaProgramadaArchivo> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task Handle(TareaProgramadaArchivoCreado @message)
    {
        try
        {
            message.ID = 0;
            var entidad = _mapper.Map<TareaProgramadaArchivo>(@message);
            await _repository.AddAsync(entidad);

            return;
        }
        catch (Exception)
        {

            throw;
        }
        
    }
}
