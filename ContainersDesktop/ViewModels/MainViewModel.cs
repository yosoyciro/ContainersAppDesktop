using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models.Login;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Contracts.Services.Config;
using ContainersDesktop.Logica.Services;

namespace ContainersDesktop.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    private readonly ILocalSettingsService _localSettingsService;
    private readonly IConfigRepository<UI_Default> _defaultConfigRepository;
    private readonly IConfigRepository<UI_Config> _configRepository;
    public  Login Login;
    public MainViewModel(ILocalSettingsService localSettingsService, IConfigRepository<UI_Config> configRepository, IConfigRepository<UI_Default> defaultConfigRepository)
    {
        _localSettingsService = localSettingsService;
        _defaultConfigRepository = defaultConfigRepository;
        _configRepository = configRepository;
        GetUsuarioLogueado().ConfigureAwait(true);
    }

    private async Task GetUsuarioLogueado()
    {
        Login = await _localSettingsService.ReadSettingAsync<Login>("Login");
    }

    public async Task VerificarConfiguracion()
    {
        var defaultConfigs = await _defaultConfigRepository.LeerTodas();

        foreach (var config in defaultConfigs)
        {
            if (!string.IsNullOrEmpty(config.Clave))
            {
                var uiConfig = await _configRepository.Leer(config.Clave);
                if (uiConfig == null) 
                {
                    UI_Config newConfig = new UI_Config();
                    newConfig.Clave = config.Clave;
                    newConfig.Valor = config.Valor;
                    newConfig.UI_CONFIG_USUARIO = Login.Usuario;

                    await _configRepository.Guardar(newConfig);
                }
            }            
        }
    }
}
