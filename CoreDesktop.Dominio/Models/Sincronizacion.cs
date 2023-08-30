using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public class Sincronizacion : BaseEntity
{    
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
