using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;

namespace ContainersDesktop.ViewModels;
public partial class LoginViewModel : ObservableRecipient, INavigationAware
{
    public bool isLoggedIn
    {
        get; private set;
    }
    public LoginViewModel()
    {
        
    }

    public void OnNavigatedFrom()
    {
    }

    public void OnNavigatedTo(object parameter)
    {
    }

    public async Task<bool> Login(string usuario, string password)
    {
        isLoggedIn = true;
        return true;
    }
}
