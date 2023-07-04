using System.ComponentModel.DataAnnotations;

namespace ContainersDesktop.Core.Models;
public class Sincronizaciones
{
    [Key]
    public int SINCRONIZACIONES_ID_REG
    {
        get; set;
    }

	public string SINCRONIZACIONES_FECHA_HORA_INICIO
    {
        get; set;
    }

	public string SINCRONIZACIONES_FECHA_HORA_FIN
    {
        get; set;
    }

	public int SINCRONIZACIONES_DISPOSITIVO_ORIGEN
    {
        get; set;
    }

	public string SINCRONIZACIONES_RESULTADO
    {
        get; set;
    }
}
