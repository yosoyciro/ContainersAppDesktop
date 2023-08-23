using System.ComponentModel.DataAnnotations.Schema;
using CoreDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models.UI_ConfigModels;
[Table("UI_CONFIG")]
public class UI_Config : ConfigBaseEntity
{    
    public string? UI_CONFIG_USUARIO
    {
        get;
        set;
    }
}
