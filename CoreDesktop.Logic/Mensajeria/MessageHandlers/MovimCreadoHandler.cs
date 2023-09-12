using AutoMapper;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Contracts;
using ContainersDesktop.Logica.Mensajeria.Messages;

namespace ContainersDesktop.Logica.Mensajeria.MessageHandlers;
public class MovimCreadoHandler : IMessageHandler<MovimCreado>
{
    private readonly IAsyncRepository<Movim> _repository;
    private readonly IMapper _mapper;
    public MovimCreadoHandler(IAsyncRepository<Movim> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task Handle(MovimCreado @message)
    {
        message.MOVIM_ID_REG = 0;
        var entidad = _mapper.Map<Movim>(@message);        
        await _repository.AddAsync(entidad);

        return;
    }
}
