using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models.Login;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Helpers;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
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

    public ObservableCollection<string> Lenguajes = new();
    [ObservableProperty]
    public string lenguaje = string.Empty;

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

        Lenguajes.Add("Español");
        Lenguajes.Add("Inglés");
        Lenguajes.Add("Francés");
        Lenguajes.Add("Catalán");
        Lenguajes.Add("Portugués");
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

        Lenguaje = Configs.Where(x => x.Clave == "Idioma" && x.UI_CONFIG_USUARIO == _login!.Usuario).FirstOrDefault()?.Valor ?? "Español";
    }

    public async Task Guardar(string key, string value)
    {
        var entidad = await _uiConfigRepository.Leer(key);
        if (entidad == null)
        {
            return;
        }
        else
        {
            entidad.Valor = value;
            await _uiConfigRepository.Guardar(entidad);
        };
       
    }
}
