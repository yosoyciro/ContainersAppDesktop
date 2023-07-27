using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.Services;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models.Login;
using ContainersDesktop.Core.Models.Storage;
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
        var parentFolder = Directory.GetParent(Path.GetDirectoryName(typeof(Program).Assembly.Location));
        return $"{parentFolder.FullName}{_settings.Data2MovieProyecto}";
    }
}
