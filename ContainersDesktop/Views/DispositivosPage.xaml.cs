using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContainersDesktop.Views;
public sealed partial class DispositivosPage : Page
{
    private Dispositivos SelectedDispositivo = new();
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
                SelectedDispositivo = DispositivosGrid!.SelectedItem as Dispositivos;
                ViewModel.BorrarDispositivo(SelectedDispositivo);
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

    private void ClaListGrid_BeginningEdit(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridBeginningEditEventArgs e)
    {

    }

    private void ClaListGrid_CellEditEnding(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridCellEditEndingEventArgs e)
    {

    }

    private async void ClaListGrid_RowEditEnding(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEditEndingEventArgs e)
    {
        try
        {
            SelectedDispositivo = DispositivosGrid!.SelectedItem as Dispositivos;
            ViewModel.GuardarDispositivo(SelectedDispositivo);
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

    private void ClaListGrid_PreparingCellForEdit(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridPreparingCellForEditEventArgs e)
    {

    }

    private void ClaListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private async void btnSincronizar_Click(object sender, RoutedEventArgs e)
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
        catch (Exception)
        {

            throw;
        }        
    }
}
