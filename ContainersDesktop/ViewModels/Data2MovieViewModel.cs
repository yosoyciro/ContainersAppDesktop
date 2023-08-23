using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models.Login;
using ContainersDesktop.Dominio.Models.Storage;
using ContainersDesktop.Logica.Services;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.ViewModels;

public partial class Data2MovieViewModel : ObservableObject
{
    private readonly ILocalSettingsService _localSettingsService;
    private readonly Settings _settings;
    public Data2MovieViewModel(ILocalSettingsService localSettingsService, IOptions<Settings> settings)
    {
        _localSettingsService = localSettingsService;
        _settings = settings.Value;
    }

    public async Task<Login> ObtenerLogin()
    {
        return await _localSettingsService.ReadSettingAsync<Login>("Login");
    }

    //public string ObtenerPassword()
    //{
    //    return _localSettingsServicio.LeerElemento("Password");
    //}

    public string ObtenerPathProyecto()
    {
        var parentFolder = Directory.GetParent(Path.GetDirectoryName(typeof(Data2MovieViewModel).Assembly.Location));
        return $"{parentFolder.FullName}{_settings.Data2MovieProyecto}";
    }
}
