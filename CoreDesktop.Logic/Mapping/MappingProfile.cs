using AutoMapper;
using ContainersDesktop.Dominio.DTO;
using ContainersDesktop.Dominio.Models;
using CoreDesktop.Logica.Mensajeria.Messages;

namespace CoreDesktop.Logica.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Model to resources
        CreateMap<TareaProgramada, TareaProgramadaModificada>()
            .ForMember(d => d.TAREAS_PROGRAMADAS_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.TAREAS_PROGRAMADAS_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        CreateMap<TareaProgramada, TareaProgramadaCreada>()
            .ForMember(d => d.TAREAS_PROGRAMADAS_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.TAREAS_PROGRAMADAS_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        CreateMap<TareaProgramada, TareaProgramadaDTO>()
            .ForMember(d => d.TAREAS_PROGRAMADAS_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.TAREAS_PROGRAMADAS_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        CreateMap<Movim, MovimCreado>()
            .ForMember(d => d.MOVIM_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.MOVIM_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.MOVIM_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        CreateMap<Movim, MovimDTO>()
            .ForMember(d => d.MOVIM_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.MOVIM_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.MOVIM_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        CreateMap<Objeto, ObjetosListaDTO>()
            .ForMember(d => d.OBJ_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.OBJ_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.OBJ_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        //Resources to model
        CreateMap<TareaProgramadaModificada, TareaProgramada>()
            .ForMember(a => a.ID, x => x.MapFrom(b => b.TAREAS_PROGRAMADAS_ID_REG));
        CreateMap<MovimCreado, Movim>()
            .ForMember(a => a.ID, x => x.MapFrom(b => b.MOVIM_ID_REG));
    }
}
