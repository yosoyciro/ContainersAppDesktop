using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Core.Helpers;
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
    }

    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Execute);

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
