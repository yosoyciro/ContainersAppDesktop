using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public partial class ClaList : AuditableEntityObservable
{
    [ObservableProperty]
    public string? claList_Descrip;

    public bool ApplyFilter(string filter)
    {
        return claList_Descrip.Contains(filter, StringComparison.OrdinalIgnoreCase);
    }
}