using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.Services;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;

namespace ContainersDesktop.ViewModels;
public partial class LoginViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPlayFabServicio _playFabServicio;
    private readonly ILocalSettingsService _localSettingsService;
    public bool isLoggedIn
    {
        get; private set;
    }
    public LoginViewModel(IPlayFabServicio playFabServicio, ILocalSettingsService localSettingsService)
    {
        _playFabServicio = playFabServicio;
        _localSettingsService = localSettingsService;
    }

    public void OnNavigatedFrom()
    {
    }

    public void OnNavigatedTo(object parameter)
    {
    }

    public async Task<bool> Login(string usuario, string password)
    {
        isLoggedIn = await _playFabServicio.Login(usuario, password);        
        if (isLoggedIn)
        {
            await _localSettingsService.SaveSettingAsync("Login", new { usuario, password });
            //_localSettingsServicio.GuardarElemento("Password", password);
        }
        return isLoggedIn;
    }
}
