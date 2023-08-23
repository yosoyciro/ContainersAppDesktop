using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models.Login;
using ContainersDesktop.Logica.Services;
using CoreDesktop.Infraestructura.Mensajeria.Services;

namespace ContainersDesktop.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    private readonly ILocalSettingsService _localSettingsService;
    private readonly AzureServiceBus _azureServiceBus;
    public MainViewModel(ILocalSettingsService localSettingsService, AzureServiceBus azureServiceBus)
    {
        _localSettingsService = localSettingsService;
        _azureServiceBus = azureServiceBus;
    }

    public async Task<Login> GetUsuarioLogueado() => await _localSettingsService.ReadSettingAsync<Login>("Login");

    public async Task<string> RecibirMensajes()
    {
        return await _azureServiceBus.RecibirMensajes();
    }
}
