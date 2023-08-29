using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts.Repositories;
using CoreDesktop.Logic.Contracts;
using CoreDesktop.Logic.Mensajeria.Messages;

namespace CoreDesktop.Logic.Mensajeria.MessageHandlers;
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
        var entidad = _mapper.Map<Movim>(@message);        
        entidad.MOVIM_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);
        await _repository.AddAsync(entidad);

        return;
    }
}
