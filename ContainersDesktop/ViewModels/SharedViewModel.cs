using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.ViewModels;
public partial class SharedViewModel : ObservableObject
{
    [ObservableProperty]
    public string? usuarioCorreo;

    [ObservableProperty]
    public string? usuarioNombre;

    [ObservableProperty]
    public string? usuarioPassword;
}
