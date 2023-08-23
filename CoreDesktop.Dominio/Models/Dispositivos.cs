using System.ComponentModel.DataAnnotations;

namespace ContainersDesktop.Dominio.Models;
public class Dispositivos
{
    [Key]
    public int DISPOSITIVOS_ID_REG
    {
        get;
        set;
    }

    public string DISPOSITIVOS_ID_ESTADO_REG
    {
        get;
        set;
    }

    public string DISPOSITIVOS_DESCRIP
    {
        get;
        set;
    }

    public string DISPOSITIVOS_CONTAINER
    {
        get;
        set;
    }

    public string DISPOSITIVOS_FECHA_ACTUALIZACION
    {
        get;
        set;
    }
}
