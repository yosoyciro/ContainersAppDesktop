using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;

public sealed partial class TiposListasGridPage : Page
{
    private ClaList SelectedClaList = new();
    public TiposListasGridViewModel ViewModel
    {
        get;
    }

    public TiposListasGridPage()
    {
        ViewModel = App.GetService<TiposListasGridViewModel>();
        InitializeComponent();
    }

    private void btnAgregar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //ViewModel.CrearNuevoObjeto();
        var nuevoRegistro = new ClaList()
        {
            CLALIST_ID_ESTADO_REG = "A",
            CLALIST_DESCRIP = "NUEVA LISTA"            
        };
        ViewModel.Source.Add(nuevoRegistro);
    }

    private async void btnBorrar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            if (ClaListGrid!.SelectedItem != null)
            {
                SelectedClaList = ClaListGrid!.SelectedItem as ClaList;
                ViewModel.BorrarObjeto(SelectedClaList);
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
        Console.WriteLine(e.Row.DataContext);
        try
        {
            SelectedClaList = ClaListGrid!.SelectedItem as ClaList;
            ViewModel.GuardarClaList(SelectedClaList);
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
}
