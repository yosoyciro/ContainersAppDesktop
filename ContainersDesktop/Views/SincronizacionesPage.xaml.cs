using System.Windows.Input;
using Azure;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Services;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;

public sealed partial class SincronizacionesPage : Page
{
    public SincronizacionesViewModel ViewModel
    {
        get;
    }
    public SincronizacionesPage()
    {
        ViewModel = App.GetService<SincronizacionesViewModel>();
        InitializeComponent();
        Loaded += SincronizacionesPage_Loaded;
    }

    private async void SincronizacionesPage_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.CargarSource();
    }

    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Execute);
    public ICommand SincronizarCommand => new AsyncRelayCommand(SincronizarCommand_Execute);

    #region Sincronizacion

    private async Task SincronizarCommand_Execute()
    {
        try
        {
            await ViewModel.SincronizarInformacion();
            //grdSincronizaciones.ItemsSource = ViewModel.apli(null, chkMostrarTodos.IsChecked ?? false);

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
        catch (RequestFailedException ex)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Sincronización";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = "Error en la Sincronización: " + ex.Message;

            await dialog.ShowAsync();
        }
        catch (SystemException ex)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Sincronización";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = "Error en la Sincronización: " + ex.Message;

            await dialog.ShowAsync();
        }
    }

    #endregion

    private async Task ExportarCommand_Execute()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "HISTORIAL_SINCRONIZACIONES.csv");

        try
        {
            Exportar.GenerarDatos(ViewModel.Source, filePath);

            ContentDialog bajaRegistroDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Atención!",
                Content = $"Se generó el archivo {filePath}",
                CloseButtonText = "Ok"
            };

            await bajaRegistroDialog.ShowAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void DispositivosGrid_Sorting(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridColumnEventArgs e)
    {

    }
}
