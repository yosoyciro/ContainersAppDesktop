using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.Services;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace ContainersDesktop.ViewModels;
public partial class LoginViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPlayFabServicio _playFabServicio;
    private readonly ILocalSettingsService _localSettingsService;
    private readonly ILogger<LoginViewModel> _logger;
    public bool isLoggedIn
    {
        get; private set;
    }
    public LoginViewModel(IPlayFabServicio playFabServicio, ILocalSettingsService localSettingsService, ILogger<LoginViewModel> logger)
    {
        _playFabServicio = playFabServicio;
        _localSettingsService = localSettingsService;
        _logger = logger;
    }

    public void OnNavigatedFrom()
    {
    }

    public void OnNavigatedTo(object parameter)
    {
    }

    public async Task<bool> Login(string usuario, string password)
    {
        _logger.LogInformation("Inicia login");
        isLoggedIn = await _playFabServicio.Login(usuario, password);        
        if (isLoggedIn)
        {
            await _localSettingsService.SaveSettingAsync("Login", new { usuario, password });
        }
        _logger.LogInformation("Finaliza login con resultado: {0}", isLoggedIn);

        return isLoggedIn;
    }
}
