using AutoMapper;
using ContainersDesktop.Dominio.DTO;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Logica.Mensajeria.Messages;

namespace CoreDesktop.Logica.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Model to resources
        CreateMap<TareaProgramada, TareaProgramadaModificada>()
            .ForMember(d => d.TAREAS_PROGRAMADAS_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.TAREAS_PROGRAMADAS_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion));            

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

        CreateMap<Objeto, ContainerCreado>()
            .ForMember(d => d.OBJ_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.OBJ_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.OBJ_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        CreateMap<ObjetosListaDTO, ContainerCreado>()
            .ReverseMap();

        CreateMap<Objeto, ContainerModificado>()
            .ForMember(d => d.OBJ_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.OBJ_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.OBJ_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        CreateMap<ObjetosListaDTO, ContainerModificado>()
            .ReverseMap();

        CreateMap<Lista, ListaCreada>().ReverseMap();
        CreateMap<Lista, ListaModificada>().ReverseMap();

        CreateMap<Dispositivo, DispositivoCreado>()
            .ForMember(d => d.DISPOSITIVOS_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.DISPOSITIVOS_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.DISPOSITIVOS_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        CreateMap<Dispositivo, DispositivoModificado>()
            .ForMember(d => d.DISPOSITIVOS_ID_REG, x => x.MapFrom(s => s.ID))
            .ForMember(d => d.DISPOSITIVOS_ID_ESTADO_REG, x => x.MapFrom(s => s.Estado))
            .ForMember(d => d.DISPOSITIVOS_FECHA_ACTUALIZACION, x => x.MapFrom(s => s.FechaActualizacion))
            .ReverseMap();

        //Resources to model
        CreateMap<TareaProgramadaModificada, TareaProgramada>()
            .ForMember(a => a.ID, x => x.MapFrom(b => b.TAREAS_PROGRAMADAS_ID_REG))
            .ForMember(a => a.Estado, x => x.MapFrom(b => b.TAREAS_PROGRAMADAS_ID_ESTADO_REG))
            .ForMember(a => a.FechaActualizacion, x => x.MapFrom(b => b.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION));

        CreateMap<MovimCreado, Movim>()
            .ForMember(a => a.ID, x => x.MapFrom(b => b.MOVIM_ID_REG))
            .ForMember(a => a.Estado, x => x.MapFrom(b => b.MOVIM_ID_ESTADO_REG))
            .ForMember(a => a.FechaActualizacion, x => x.MapFrom(b => b.MOVIM_FECHA_ACTUALIZACION));
    }
}
