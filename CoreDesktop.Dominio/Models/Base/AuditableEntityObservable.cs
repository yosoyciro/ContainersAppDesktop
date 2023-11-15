using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.Dominio.Models.Base;
public partial class AuditableEntityObservable : BaseEntityObservable
{
    [ObservableProperty]
    public string? estado;

    [ObservableProperty]
    public string? fechaActualizacion;
}
