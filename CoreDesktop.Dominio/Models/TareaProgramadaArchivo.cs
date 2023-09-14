using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public class TareaProgramadaArchivo : AuditableEntity
{
    public int TAREAS_PROGRAMADAS_ARCHIVOS_TAREAS_PROGRAMADAS_ID_REG { get; set; }
    public string TAREAS_PROGRAMADAS_ARCHIVOS_URL_ARCHIVO { get; set; } = string.Empty;
}
