using System.ComponentModel.DataAnnotations;

namespace ContainersDesktop.Dominio.Models;
public class ClaList
{
    [Key]
    public int CLALIST_ID_REG
    {
        get;
        set;
    }

    public string CLALIST_ID_ESTADO_REG
    {
        get;
        set;
    }

    public string CLALIST_DESCRIP
    {
        get;
        set;
    }

    public string CLALIST_FECHA_ACTUALIZACION
    {
        get;
        set;
    }
}
