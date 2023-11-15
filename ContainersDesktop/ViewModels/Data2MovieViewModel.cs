using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models.Login;
using ContainersDesktop.Dominio.Models.Storage;
using ContainersDesktop.Logica.Contracts;
using ContainersDesktop.Logica.Services;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.ViewModels;

public partial class Data2MovieViewModel : ObservableObject
{
    private readonly ILocalSettingsService _localSettingsService;
    private readonly Settings _settings;
    private readonly SharedViewModel _sharedViewModel;
    private readonly IAzureTableStorage _azureTableStorage;
    private readonly IFileShareService _fileShareService;
    private readonly IOptions<InfoModulo> options;

    public SharedViewModel SharedViewModel => _sharedViewModel;
    public IAzureTableStorage AzureTableStorage => _azureTableStorage;
    public IFileShareService FileShareService => _fileShareService;
    public IOptions<InfoModulo> Options => options;

    public Data2MovieViewModel(ILocalSettingsService localSettingsService, IOptions<Settings> settings, SharedViewModel sharedViewModel, IAzureTableStorage azureTableStorage, IOptions<InfoModulo> options, IFileShareService fileShareService)
    {
        _localSettingsService = localSettingsService;
        _settings = settings.Value;
        _sharedViewModel = sharedViewModel;
        _azureTableStorage = azureTableStorage;
        this.options = options;
        _fileShareService = fileShareService;
    }

    //public async Task<Login> ObtenerLogin()
    //{
    //    return await _localSettingsService.ReadSettingAsync<Login>("Login");
    //}

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
