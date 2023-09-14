using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Mensajeria;

namespace ContainersDesktop.Logica.Mensajeria.Messages;
public class TareaProgramadaArchivoCreado : Message
{
    public int ID { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int TAREAS_PROGRAMADAS_ARCHIVOS_TAREAS_PROGRAMADAS_ID_REG { get; set; }
    public string TAREAS_PROGRAMADAS_ARCHIVOS_URL_ARCHIVO { get; set; } = string.Empty;
    public string FechaActualizacion { get; set; } = string.Empty;
}
