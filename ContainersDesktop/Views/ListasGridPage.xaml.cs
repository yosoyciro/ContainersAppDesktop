using CommunityToolkit.WinUI.UI.Controls;
using System.Collections.ObjectModel;
using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace ContainersDesktop.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class ListasGridPage : Page
{
    private Listas SelectedLista = new();
    public ListasGridViewModel ViewModel
    {
        get;
    }

    public ListasGridPage()
    {
        ViewModel = App.GetService<ListasGridViewModel>();
        InitializeComponent();
    }

    private void btnAgregar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //ViewModel.CrearNuevoObjeto();
        var nuevoRegistro = new Listas()
        {
            LISTAS_ID_ESTADO_REG = "A",
            LISTAS_ID_LISTA = 1,
            LISTAS_ID_LISTA_ORDEN = 1,
            LISTAS_ID_LISTA_DESCRIP = "INGRESE VALOR"
        };
        ViewModel.Source.Add(nuevoRegistro);
    }

    private async void btnBorrar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ListaGrid!.SelectedItem != null)
            {
                SelectedLista = ListaGrid!.SelectedItem as Listas;
                ViewModel.BorrarLista(SelectedLista);
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

    private void ListaGrid_BeginningEdit(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridBeginningEditEventArgs e)
    {

    }

    private void ListaGrid_CellEditEnding(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridCellEditEndingEventArgs e)
    {

    }

    private async void ListaGrid_RowEditEnding(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEditEndingEventArgs e)
    {
        Console.WriteLine(e.Row.DataContext);
        try
        {
            SelectedLista = ListaGrid!.SelectedItem as Listas;
            ViewModel.GuardarLista(SelectedLista);
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

    private void ListaGrid_PreparingCellForEdit(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridPreparingCellForEditEventArgs e)
    {

    }

    private void ListaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void ListaGrid_Sorting(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridColumnEventArgs e)
    {
        //ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
        //Listas item = group.GroupItems[0] as Listas;
        //e.RowGroupHeader.PropertyValue = item.LISTAS_ID_LISTA;

        //if (e.Column.Tag.ToString() == "ClaseLista")
        //{
        //    //Implement sort on the column "Range" using LINQ
        //    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
        //    {
        //        ListaGrid.ItemsSource = new ObservableCollection<ClaList>(from item in ListaGrid.ItemsSource
        //                                                            orderby item.Range ascending
        //                                                            select item);
        //        e.Column.SortDirection = DataGridSortDirection.Ascending;
        //    }
        //    else
        //    {
        //        ListaGrid.ItemsSource = new ObservableCollection<ClaList>(from item in _items
        //                                                            orderby item.Range descending
        //                                                            select item);
        //        e.Column.SortDirection = DataGridSortDirection.Descending;
        //    }
        //}
        //// add code to handle sorting by other columns as required

        //// Remove sorting indicators from other columns
        //foreach (var dgColumn in dg.Columns)
        //{
        //    if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
        //    {
        //        dgColumn.SortDirection = null;
        //    }
        //}
    }

    private async void chkVerTodos_Click(object sender, RoutedEventArgs e)
    {
        bool verTodos = chkVerTodos.IsChecked!.Value == true ? true : false;
        ViewModel.Source.Clear();
        await ViewModel.LlenarSource(verTodos);
    }
}
