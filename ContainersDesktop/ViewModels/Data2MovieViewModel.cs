using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models.Storage;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.ViewModels;

public partial class Data2MovieViewModel : ObservableObject
{
    private readonly ILocalSettingsServicio _localSettingsServicio;
    private readonly Settings _settings;
    public Data2MovieViewModel(ILocalSettingsServicio localSettingsServicio, IOptions<Settings> settings)
    {
        _localSettingsServicio = localSettingsServicio;
        _settings = settings.Value;        
    }

    public string ObtenerUsuario()
    {
        return _localSettingsServicio.LeerElemento("Usuario");
    }

    public string ObtenerPassword()
    {
        return _localSettingsServicio.LeerElemento("Password");
    }

    public string ObtenerPathProyecto()
    {
        return _settings.Data2MovieProyecto;
    }
}
