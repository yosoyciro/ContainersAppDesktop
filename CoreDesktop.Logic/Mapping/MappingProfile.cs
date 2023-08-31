using AutoMapper;
using ContainersDesktop.Dominio.DTO;
using ContainersDesktop.Dominio.Models;
using CoreDesktop.Logic.Mensajeria.Messages;

namespace CoreDesktop.Logic.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Model to resources
        CreateMap<TareaProgramada, TareaProgramadaModificada>();
        CreateMap<Movim, MovimCreado>();
        CreateMap<Objeto, ObjetosListaDTO>()
            .ForMember(d => d.OBJ_ID_REG, x => x.MapFrom(s => s.ID))
            .ReverseMap();

        //Resources to model
        CreateMap<TareaProgramadaModificada, TareaProgramada>()
            .ForMember(a => a.ID, x => x.MapFrom(b => b.TAREAS_PROGRAMADAS_ID_REG));
        CreateMap<MovimCreado, Movim>()
            .ForMember(a => a.ID, x => x.MapFrom(b => b.MOVIM_ID_REG));
    }
}
