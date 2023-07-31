using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    public ICommand GoToDispositivosCommand => new RelayCommand(GoToDispositivos);
    public ICommand GoToListasCommand => new RelayCommand(GoToListas);
    public ICommand GoToContenedoresCommand => new RelayCommand(GoToContenedores);

    private void GoToDispositivos()
    {
        Frame.Navigate(typeof(DispositivosPage), null);
    }

    private void GoToListas()
    {
        Frame.Navigate(typeof(TiposListaDetailsPage), null);
    }

    private void GoToContenedores()
    {
        Frame.Navigate(typeof(ContainersGridPage), null);
    }
}
