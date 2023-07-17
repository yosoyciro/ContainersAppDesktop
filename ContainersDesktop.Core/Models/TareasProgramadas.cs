using System.ComponentModel.DataAnnotations.Schema;

namespace ContainersDesktop.Core.Models;
[Table("TAREAS_PROGRAMADAS")]
public class TareaProgramada
{
    public int TAREAS_PROGRAMADAS_ID_REG
    {
        get; set;
    }
    public int TAREAS_PROGRAMADAS_OBJETO_ID_REG
    {
        get; set;
    }
    public string TAREAS_PROGRAMADAS_FECHA_PROGRAMADA
    {
        get; set;
    }
    public string TAREAS_PROGRAMADAS_FECHA_COMPLETADA
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
    public string TAREAS_PROGRAMADAS_ORDENADO
    {
        get; set;
    }
    public int TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG
    {
        get; set;
    }
    public string TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION
    {
        get; set;
    }
}
