using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;

namespace ContainersDesktop.ViewModels;
public partial class LoginViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPlayFabServicio _playFabServicio;
    private readonly ILocalSettingsServicio _localSettingsServicio;
    public bool isLoggedIn
    {
        get; private set;
    }
    public LoginViewModel(IPlayFabServicio playFabServicio, ILocalSettingsServicio localSettingsServicio)
    {
        _playFabServicio = playFabServicio;
        _localSettingsServicio = localSettingsServicio;
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
            _localSettingsServicio.GuardarElemento("Usuario", usuario);
            _localSettingsServicio.GuardarElemento("Password", password);
        }
        return isLoggedIn;
    }
}
