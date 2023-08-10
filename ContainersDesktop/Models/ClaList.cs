using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.Core.Models;
public class ClaList : ObservableObject
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

    public bool ApplyFilter(string filter)
    {
        return CLALIST_DESCRIP.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
    }
}
