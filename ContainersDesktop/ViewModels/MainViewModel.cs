﻿using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models.Login;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;

namespace ContainersDesktop.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    //private readonly ILocalSettingsService _localSettingsService;
    private readonly IConfigRepository<UI_Default> _defaultConfigRepository;
    private readonly IConfigRepository<UI_Config> _configRepository;
    private readonly SharedViewModel _sharedViewModel;

    public Login Login;
    public SharedViewModel SharedViewModel => _sharedViewModel;

    [ObservableProperty]
    public string mensajeBienvenida;

    public MainViewModel(IConfigRepository<UI_Config> configRepository, IConfigRepository<UI_Default> defaultConfigRepository, SharedViewModel sharedViewModel)
    {
        //_localSettingsService = localSettingsService;
        _defaultConfigRepository = defaultConfigRepository;
        _configRepository = configRepository;
        _sharedViewModel = sharedViewModel;
        //GetUsuarioLogueado().ConfigureAwait(true);        

    }

    //private async Task GetUsuarioLogueado()
    //{
    //    Login = await _localSettingsService.ReadSettingAsync<Login>("Login");
    //}

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
