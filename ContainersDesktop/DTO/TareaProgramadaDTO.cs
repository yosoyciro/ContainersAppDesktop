using System.ComponentModel;

namespace ContainersDesktop.DTO;
public class TareaProgramadaDTO
{
    public int TAREAS_PROGRAMADAS_ID_REG
    {
        get; set;
    }
    public int TAREAS_PROGRAMADAS_OBJETO_ID_REG
    {
        get; set;
    }
    public string? TAREAS_PROGRAMADAS_OBJETO_MATRICULA
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
    public string? TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION
    {
        get; set;
    }
    public int TAREAS_PROGRAMADAS_UBICACION_DESTINO
    {
        get; set;
    }
    public string? TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION
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
    public string? TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION
    {
        get; set;
    }
    public double TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD
    {
        get; set;
    }
    public double TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD
    {
        get; set;
    }
}
