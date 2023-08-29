using AutoMapper;
using ContainersDesktop.Dominio.Models;
using CoreDesktop.Logic.Mensajeria.Messages;

namespace CoreDesktop.Logic.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Model to handler
        CreateMap<TareaProgramada, TareaProgramadaModificada>();
        CreateMap<Movim, MovimCreado>();

        //Handler to model
        CreateMap<TareaProgramadaModificada, TareaProgramada>()
            .ForMember(a => a.ID, x => x.MapFrom(b => b.TAREAS_PROGRAMADAS_ID_REG));
        CreateMap<MovimCreado, Movim>()
            .ForMember(a => a.ID, x => x.MapFrom(b => b.MOVIM_ID_REG));
    }
}
