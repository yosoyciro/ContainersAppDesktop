using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Mensajeria;

namespace ContainersDesktop.Logica.Mensajeria.Messages;
public class TareaProgramadaModificada : Message
{
    public int TAREAS_PROGRAMADAS_ID_REG
    {
        get; set;
    }
    public string? TAREAS_PROGRAMADAS_ID_ESTADO_REG
    {
        get; set;
    }
    public int TAREAS_PROGRAMADAS_OBJETO_ID_REG
    {
        get; set;
    }
    public string? TAREAS_PROGRAMADAS_FECHA_PROGRAMADA
    {
        get; set;
    }
    public string? TAREAS_PROGRAMADAS_FECHA_COMPLETADA
    {
        get; set;
    }
    public int TAREAS_PROGRAMADAS_UBICACION_ORIGEN
    {
        get; set;
    }
    public int TAREAS_PROGRAMADAS_UBICACION_DESTINO
    {
        get; set;
    }
    public string? TAREAS_PROGRAMADAS_ORDENADO
    {
        get; set;
    }
    public int TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG
    {
        get; set;
    }
    public string? TAREAS_PROGRAMADAS_DISPOSITIVOS_CONTAINER
    {
        get; set;
    }
    public string? TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION
    {
        get; set;
    }
    public double TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD
    {
        get;
        set;
    }
    public double TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD
    {
        get;
        set;
    }
    public string? TAREAS_PROGRAMADAS_TAREAS_PROGRAMADAS_ESTADO_TAREA
    {
        get; set;
    }

    public TareaProgramadaModificada()
    {
        
    }

    //public TareaProgramadaModificada(TareaProgramada tarea)
    //{
    //    this.TAREAS_PROGRAMADAS_ID_REG = tarea.ID;
    //    this.TAREAS_PROGRAMADAS_ID_ESTADO_REG = tarea.Estado;     
    //    this.TAREAS_PROGRAMADAS_OBJETO_ID_REG = tarea.TAREAS_PROGRAMADAS_OBJETO_ID_REG;
    //    this.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = tarea.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG;
    //    this.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = tarea.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD;
    //    this.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = tarea.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD;
    //    this.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = tarea.FechaActualizacion;
    //    this.TAREAS_PROGRAMADAS_FECHA_COMPLETADA = tarea.TAREAS_PROGRAMADAS_FECHA_COMPLETADA;
    //    this.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = tarea.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA;
    //    this.TAREAS_PROGRAMADAS_ORDENADO = tarea.TAREAS_PROGRAMADAS_ORDENADO;
    //    this.TAREAS_PROGRAMADAS_UBICACION_DESTINO = tarea.TAREAS_PROGRAMADAS_UBICACION_DESTINO;
    //    this.TAREAS_PROGRAMADAS_UBICACION_ORIGEN = tarea.TAREAS_PROGRAMADAS_UBICACION_ORIGEN;
    //    this.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = tarea.FechaActualizacion;
    //}
}
