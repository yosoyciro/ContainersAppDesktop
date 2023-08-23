using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Helpers;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using System.Reflection;

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
        this.Loaded += SettingsPage_Loaded;
    }

    private async void SettingsPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.CargarConfig();

        //Colores grid
        colorPicker.Color = Colores.HexToColor(ViewModel.Configs.FirstOrDefault(x => x.Clave == "GridColor")?.Valor);        

        //Color combo
        comboColorPicker.Color = Colores.HexToColor(ViewModel.Configs.FirstOrDefault(x => x.Clave == "ComboColor")?.Valor);
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

    private async void colorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
    {
        colorPicker.Color = sender.Color;
        await ViewModel.Guardar("GridColor", sender.Color.ToString());
    }

    private async void comboColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
    {
        comboColorPicker.Color = sender.Color;
        await ViewModel.Guardar("ComboColor", sender.Color.ToString());
    }
}
