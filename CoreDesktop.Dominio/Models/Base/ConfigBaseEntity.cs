using System.ComponentModel.DataAnnotations;

namespace CoreDesktop.Dominio.Models.Base;
public abstract class ConfigBaseEntity
{
    [Key]
    public int Id
    {
        get;
        set;
    }
    public string? Clave
    {
        get;
        set;
    }

    public string? Valor
    {
        get;
        set;
    }
}
