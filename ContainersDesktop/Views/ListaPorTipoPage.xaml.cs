using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.ViewModels;
using ContainersDesktop.Core.Models;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.Core.Helpers;
using System.Data;

namespace ContainersDesktop.Views;

public sealed partial class ListaPorTipoPage : Page
{   
    public ListaPorTipoViewModel ViewModel
    {
        get;
    }

    public ListaPorTipoPage()
    {
        ViewModel = App.GetService<ListaPorTipoViewModel>();
        this.InitializeComponent();
        this.Loaded += ListaPorTipoPage_Loaded;
    }

    private void ListaPorTipoPage_Loaded(object sender, RoutedEventArgs e)
    {
        grdListaPorTipo.ItemsSource = ViewModel.AplicarFiltro(null, false);
    }
    
    public ICommand AgregarCommand => new AsyncRelayCommand(OpenAgregarDialog);
    public ICommand AgregarRegistroCommand => new AsyncRelayCommand(AgregarRegistro);
    public ICommand ModificarCommand => new AsyncRelayCommand(OpenModificarDialog);
    public ICommand ModificarRegistroCommand => new AsyncRelayCommand(ModificarRegistro);
    public ICommand BorrarCommand => new AsyncRelayCommand(BorrarRegistro);    
    public ICommand VolverCommand => new RelayCommand(Volver);
    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Execute);

    private async Task OpenAgregarDialog()
    {
        txtOrden.IsEnabled = false;

        AgregarDialog.Title = "Agregar entrada a la lista";
        AgregarDialog.PrimaryButtonText = "Confirmar";
        //AgregarDialog.IsPrimaryButtonEnabled = ViewModel.FormViewModel.IsValid;
        AgregarDialog.PrimaryButtonCommand = AgregarRegistroCommand;
        
        AgregarDialog.DataContext = new Listas() 
        { 
            LISTAS_ID_ESTADO_REG = "A",                        
        };

        ViewModel.FormViewModel.Orden = ViewModel.Source.OrderByDescending(x => x.LISTAS_ID_LISTA_ORDEN).FirstOrDefault().LISTAS_ID_LISTA_ORDEN + 1;
        ViewModel.FormViewModel.Descripcion = string.Empty;
        await AgregarDialog.ShowAsync();
    }

    private async Task OpenModificarDialog()
    {
        txtOrden.IsEnabled = true;

        AgregarDialog.Title = "Modificar entrada de la lista";
        AgregarDialog.PrimaryButtonText = "Confirmar";
        AgregarDialog.PrimaryButtonCommand = ModificarRegistroCommand;
        //AgregarDialog.IsPrimaryButtonEnabled = ViewModel.FormViewModel.IsValid;
        AgregarDialog.DataContext = ViewModel.SelectedLista;

        ViewModel.FormViewModel.Orden = ViewModel.SelectedLista.LISTAS_ID_LISTA_ORDEN;
        ViewModel.FormViewModel.Descripcion = ViewModel.SelectedLista.LISTAS_ID_LISTA_DESCRIP;
        await AgregarDialog.ShowAsync();
    }

    private async Task ExportarCommand_Execute()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"{ViewModel.claLista.CLALIST_DESCRIP}.csv");

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

    private async Task BorrarRegistro()
    {
        ContentDialog bajaRegistroDialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Atención!",
            Content = "Está seguro que desea dar de baja el registro?",
            PrimaryButtonText = "Sí",
            CloseButtonText = "No"
        };

        ContentDialogResult result = await bajaRegistroDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            try
            {
                await ViewModel.BorrarLista();
                grdListaPorTipo.ItemsSource = ViewModel.AplicarFiltro(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
                LimpiarIndicadorOden();
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
    }

    private async Task AgregarRegistro()
    {
        var lista = AgregarDialog.DataContext as Listas;
        lista.LISTAS_ID_LISTA_ORDEN = ViewModel.FormViewModel.Orden;
        lista.LISTAS_ID_LISTA_DESCRIP = ViewModel.FormViewModel.Descripcion;
        await ViewModel.AgregarLista(lista);

        grdListaPorTipo.ItemsSource = ViewModel.AplicarFiltro(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
        LimpiarIndicadorOden();
    }

    private async Task ModificarRegistro()
    {
        var lista = AgregarDialog.DataContext as Listas;
        lista.LISTAS_ID_LISTA_ORDEN = ViewModel.FormViewModel.Orden;
        lista.LISTAS_ID_LISTA_DESCRIP = ViewModel.FormViewModel.Descripcion;
        await ViewModel.ActualizarLista(lista);

        grdListaPorTipo.ItemsSource = ViewModel.AplicarFiltro(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
        LimpiarIndicadorOden();
    }

    private void Volver()
    {
        Frame.GoBack();
    }
    
    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {
        grdListaPorTipo.ItemsSource = ViewModel.AplicarFiltro(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
        LimpiarIndicadorOden();
    }

    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.QueryText != "")
        {
            grdListaPorTipo.ItemsSource = ViewModel.AplicarFiltro(args.QueryText, chkMostrarTodos.IsChecked ?? false);
            LimpiarIndicadorOden();
        }        
    }

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (sender.Text == "")
            {
                grdListaPorTipo.ItemsSource = ViewModel.AplicarFiltro(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
                LimpiarIndicadorOden();
            }
        }
    }

    private void ListaGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        // Add sorting indicator, and sort
        var isAscending = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending;
        grdListaPorTipo.ItemsSource = ViewModel.SortData(e.Column.Tag.ToString(), isAscending);
        e.Column.SortDirection = isAscending
            ? DataGridSortDirection.Ascending
            : DataGridSortDirection.Descending;

        foreach (var column in grdListaPorTipo.Columns)
        {
            if (column.Tag != null && column.Tag.ToString() != e.Column.Tag.ToString())
            {
                column.SortDirection = null;
            }
        }               
    }

    private void LimpiarIndicadorOden()
    {
        foreach (var column in grdListaPorTipo.Columns)
        {          
            column.SortDirection = null;
        }
    }    

    private void txtDescripcion_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        ViewModel.FormViewModel.Descripcion = sender.Text;

    }
}
