using System;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models.Login;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Helpers;
using ContainersDesktop.Infraestructura.Contracts.Services.Config;
using ContainersDesktop.Logica.Contracts.Services;
using ContainersDesktop.Logica.Services;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;

namespace ContainersDesktop.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IConfigRepository<UI_Config> _uiConfigRepository;
    private readonly ILocalSettingsService _localSettingsService;
    private Login _login;
    public List<UI_Config> Configs = new();

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IConfigRepository<UI_Config> uiConfigRepository, ILocalSettingsService localSettingsService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
        _uiConfigRepository = uiConfigRepository;
        _localSettingsService = localSettingsService;
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    public async Task CargarConfig()
    {
        _login = await _localSettingsService.ReadSettingAsync<Login>("Login");
        Configs = await _uiConfigRepository.LeerTodas();

        Console.WriteLine("configs", Configs);
    }

    public async Task Guardar(string key, string value)
    {
        var entidad = new UI_Config()
        {
            Id = Configs.FirstOrDefault(x => x.Clave == key)?.Id ?? 0,
            Clave = key,
            Valor = value,
            UI_CONFIG_USUARIO = _login.Usuario,
        };
        await _uiConfigRepository.Guardar(entidad);
    }
}
