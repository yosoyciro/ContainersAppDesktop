using System.Windows.Input;
using Azure;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Helpers;
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
            await Dialogs.Aviso(this.XamlRoot, "Sincronización realizada!");          
        }
        catch (RequestFailedException ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
        catch (SystemException ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }

    #endregion

    private async Task ExportarCommand_Execute()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "HISTORIAL_SINCRONIZACIONES.csv");

        try
        {
            await Exportar.GenerarDatos(ViewModel.Source, filePath, this.XamlRoot);
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }

    private void DispositivosGrid_Sorting(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridColumnEventArgs e)
    {

    }
}
