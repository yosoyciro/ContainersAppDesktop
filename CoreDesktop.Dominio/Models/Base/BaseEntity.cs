using System.ComponentModel.DataAnnotations;

namespace ContainersDesktop.Dominio.Models.Base;
public abstract class BaseEntity
{
    [Key]
    public int ID
    {
        get;
        set;
    }    
}
