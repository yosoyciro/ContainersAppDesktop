using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;
public sealed partial class DispositivosPage : Page
{    
    public DispositivosViewModel ViewModel
    {
        get;
    }
    public DispositivosPage()
    {
        ViewModel = App.GetService<DispositivosViewModel>();
        InitializeComponent();
    }

    private void btnAgregar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //ViewModel.CrearNuevoObjeto();
        var nuevoRegistro = new Dispositivos()
        {
            DISPOSITIVOS_ID_ESTADO_REG = "A",
            DISPOSITIVOS_DESCRIP = "DESCRIPCION",
            DISPOSITIVOS_CONTAINER = "AZURE CONTAINER"
        };
        ViewModel.Source.Add(nuevoRegistro);
    }

    private async void btnBorrar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            if (DispositivosGrid!.SelectedItem != null)
            {
                ViewModel.SelectedDispositivo = DispositivosGrid!.SelectedItem as Dispositivos;
                ViewModel.BorrarDispositivo();
            }
        }
        catch (Exception ex)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Error";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = ex.Message;

            await dialog.ShowAsync();
        }
    }   
    
    private async Task SincronizarDatos()
    {
        try
        {
            await ViewModel.SincronizarInformacion();

            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Sincronización";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = "Sincronización realizada!";

            await dialog.ShowAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public ICommand NuevoCommand => new AsyncRelayCommand(OpenNewDialog);
    public ICommand BorrarCommand => new AsyncRelayCommand(BorrarDispositivo);
    public ICommand AgregarCommand => new AsyncRelayCommand(AgregarDispositivo);
    public ICommand SincronizarCommand => new AsyncRelayCommand(SincronizarDatos);

    private async Task OpenNewDialog()
    {
        AgregarDialog.Title = "Nuevo dispositivo";
        AgregarDialog.PrimaryButtonText = "Agregar";
        AgregarDialog.PrimaryButtonCommand = AgregarCommand;
        AgregarDialog.DataContext = new Dispositivos();
        await AgregarDialog.ShowAsync();
    }

    private async Task BorrarDispositivo()
    {
        try
        {
            await ViewModel.BorrarDispositivo();
        }
        catch (Exception ex)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Error";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = ex.Message;

            await dialog.ShowAsync();
        }
    }

    private async Task AgregarDispositivo()
    {
        await ViewModel.CrearDispositivo(AgregarDialog.DataContext as Dispositivos);
    }

    private async void DispositivosGrid_RowEditEnding(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEditEndingEventArgs e)
    {
        try
        {
            await ViewModel.ActualizarDispositivo(DispositivosGrid!.SelectedItem as Dispositivos);
        }
        catch (Exception ex)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Error";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = ex.Message;

            await dialog.ShowAsync();
        }
    }

    private void DispositivosGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SelectedDispositivo = DispositivosGrid!.SelectedItem as Dispositivos;
    }
}
