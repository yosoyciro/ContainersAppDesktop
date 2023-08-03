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

    public ICommand GoToDispositivosCommand => new RelayCommand(GoToDispositivosCommand_Execute);
    public ICommand GoToListasCommand => new RelayCommand(GoToListasCommand_Execute);
    public ICommand GoToContenedoresCommand => new RelayCommand(GoToContenedoresCommand_Execute);
    public ICommand GoToSincronizacionesCommand => new RelayCommand(GoToSincronizacionesCommand_Execute);

    private void GoToDispositivosCommand_Execute()
    {
        Frame.Navigate(typeof(DispositivosPage), null);
    }

    private void GoToListasCommand_Execute()
    {
        Frame.Navigate(typeof(TiposListaDetailsPage), null);
    }

    private void GoToContenedoresCommand_Execute()
    {
        Frame.Navigate(typeof(ContainersGridPage), null);
    }

    private void GoToSincronizacionesCommand_Execute()
    {
        Frame.Navigate(typeof(SincronizacionesPage), null);
    }
}
