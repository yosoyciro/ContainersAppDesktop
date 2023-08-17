using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.ViewModels;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using ContainersDesktop.DTO;
using ContainersDesktop.Core.Helpers;
using CommunityToolkit.WinUI.UI.Controls;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Helpers;
using Windows.Storage.Pickers;
using Windows.Storage;

namespace ContainersDesktop.Views;

public sealed partial class ListaPorTipoPage : Page
{   
    public ListaPorTipoViewModel ViewModel
    {
        get;
    }
    private StorageFile file;

    public ListaPorTipoPage()
    {
        ViewModel = App.GetService<ListaPorTipoViewModel>();
        this.InitializeComponent();
        this.Loaded += ListaPorTipoPage_Loaded;
    }

    private async void ListaPorTipoPage_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.CargarSource();
        grdListaPorTipo.ItemsSource = ViewModel.AplicarFiltro(null, false);
    }
    
    public ICommand AgregarCommand => new AsyncRelayCommand(OpenAgregarDialog);
    public ICommand AgregarRegistroCommand => new AsyncRelayCommand(AgregarRegistro);
    public ICommand ModificarCommand => new AsyncRelayCommand(OpenModificarDialog);
    public ICommand ModificarRegistroCommand => new AsyncRelayCommand(ModificarRegistro);
    public ICommand BorrarRecuperarCommand => new AsyncRelayCommand(BorrarRecuperarCommand_Execute);    
    public ICommand VolverCommand => new RelayCommand(Volver);
    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Execute);
    public ICommand ImportarCommand => new AsyncRelayCommand(ImportarCommand_Execute);
    public ICommand ImportarArchivoCommand => new AsyncRelayCommand(ImportarArchivoCommand_Execute);

    private async Task OpenAgregarDialog()
    {
        txtOrden.IsEnabled = false;

        AgregarDialog.Title = "Agregar entrada a la lista";
        AgregarDialog.PrimaryButtonText = "Confirmar";
        AgregarDialog.PrimaryButtonCommand = AgregarRegistroCommand;
        
        AgregarDialog.DataContext = new Listas() 
        { 
            LISTAS_ID_ESTADO_REG = "A",    
            LISTAS_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now),
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
            await Exportar.GenerarDatos(ViewModel.Source, filePath, this.XamlRoot);
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }

    private async Task ImportarCommand_Execute()
    {
        dialogImportar.Title = "Importar";
        dialogImportar.PrimaryButtonText = "Confirmar";
        dialogImportar.IsPrimaryButtonEnabled = false;
        dialogImportar.PrimaryButtonCommand = ImportarArchivoCommand;
        
        await dialogImportar.ShowAsync();
    }

    private async Task ImportarArchivoCommand_Execute()
    {
        try
        {
            using (var reader = new StreamReader(file.Path))
            {
                var i = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(i == 0 ? ',' : ';');

                    Listas lista = new()
                    {
                        LISTAS_ID_LISTA = ViewModel.claLista.CLALIST_ID_REG,
                        LISTAS_ID_LISTA_ORDEN = int.Parse(values[0]),
                        LISTAS_ID_LISTA_DESCRIP = values[1],
                        LISTAS_ID_ESTADO_REG = "A",
                        LISTAS_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now),
                    };
                    await ViewModel.AgregarLista(lista);

                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
        
    }

    private async Task BorrarRecuperarCommand_Execute()
    {
        var pregunta = ViewModel.EstadoActivo ? "Está seguro que desea dar de baja el registro?" : "Está seguro que desea recuperar el registro?";        
        ContentDialogResult result = await Dialogs.Pregunta(this.XamlRoot, pregunta);

        if (result == ContentDialogResult.Primary)
        {
            try
            {                
                await ViewModel.BorrarRecuperarLista();

                grdListaPorTipo.ItemsSource = ViewModel.AplicarFiltro(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
                LimpiarIndicadorOden();
            }
            catch (Exception ex)
            {
                await Dialogs.Error(this.XamlRoot, ex.Message);
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
        lista.LISTAS_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);
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

    private async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
    {
        // Clear previous returned file name, if it exists, between iterations of this scenario
        PickAPhotoOutputTextBlock.Text = "";

        // Create a file picker
        var openPicker = new FileOpenPicker();

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        var window = (Application.Current as App)?.Window as MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options for your file picker
        openPicker.ViewMode = PickerViewMode.List;
        openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        openPicker.FileTypeFilter.Add(".csv");

        // Open the picker for the user to pick a file
        file = await openPicker.PickSingleFileAsync();
        if (file != null)
        {
            PickAPhotoOutputTextBlock.Text = "Archivo seleccionado: " + file.Name;
            dialogImportar.IsPrimaryButtonEnabled = true;
        }
        else
        {
            PickAPhotoOutputTextBlock.Text = "Operación cancelada";
        }
    }
}